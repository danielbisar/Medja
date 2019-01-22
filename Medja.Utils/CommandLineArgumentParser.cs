using System;
using System.Collections.Generic;
using System.Linq;
using Medja.Utils.Text;

namespace Medja.Utils
{
    public class CommandLineArgumentParser
    {
        private static readonly ArraySegment<string> EmptyForwardedArguments = new ArraySegment<string>(new string[0]);
        
        private readonly List<CommandLineArgument> _arguments;
        public IReadOnlyList<CommandLineArgument> Arguments
        {
            get { return _arguments; }
        }

        private CommandLineArgument _helpArgument;
        
        public string HelpHeader { get; set; }
        
        /// <summary>
        /// Sets the amount of minimum expected arguments.
        /// This is helpful if you have non optional positional arguments.
        /// The IsOptional is not yet implemented well so this is a workaround.
        /// </summary>
        public int MinArgumentCount { get; set; }

        public CommandLineArgumentParser()
        {
            _arguments = new List<CommandLineArgument>();
        }

        public CommandLineArgument AddArgument(string longForm, string shortForm, string helpText, Action action)
        {
            return AddArgument(new CommandLineArgument
            {
                    LongForm = longForm,
                    ShortForm = shortForm,
                    HelpText = helpText,
                    Action = p => action()
            });
        }

        public CommandLineArgument AddArgumentWithSubArgs(string longForm, string shortForm, string helpText, Action<ArraySegment<string>> action)
        {
            return AddArgument(new CommandLineArgument
            {
                    LongForm = longForm,
                    ShortForm = shortForm,
                    HelpText = helpText,
                    Action = action,
                    HasSubArguments = true
            });
        }
        
        public CommandLineArgument AddPositionalArgument(string name, string helpText, Action<string> action)
        {
            return AddArgument(new CommandLineArgument
            {
                    HelpText = helpText,
                    LongForm = name,
                    PosOrValueAction = action,
                    IsPositionalArgument = true
            });
        }

        public CommandLineArgument AddHelpArgument()
        {
            return AddArgument("--help", "-h", "prints this screen", PrintHelp);
        }

        /// <summary>
        /// Adds an argument that needs a value (f.e. --my-arg=value)
        /// </summary>
        public CommandLineArgument AddValueArgument(string longForm, string shortForm, string helpTest,
                                                    Action<string> valueAction)
        {
            return AddArgument(new CommandLineArgument
            {
                    LongForm = longForm,
                    ShortForm = shortForm,
                    HelpText = helpTest,
                    HasValue = true,
                    PosOrValueAction = valueAction
            });
        }

        /// <summary>
        /// Adds an argument.
        /// </summary>
        /// <param name="argument">The argument to add.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><see cref="argument"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The given argument has a
        /// <see cref="CommandLineArgument.ShortForm"/> or <see cref="CommandLineArgument.LongForm"/> that was added
        /// already.</exception>
        public CommandLineArgument AddArgument(CommandLineArgument argument)
        {
            if(argument == null)
                throw new ArgumentNullException(nameof(argument));
            
            if(argument.HasValue && argument.HasSubArguments)
                throw new InvalidOperationException("An argument cannot have a value and sub arguments at the " +
                                                    "same time.");

            if (!string.IsNullOrEmpty(argument.ShortForm)
                && _arguments.Any(p => p.ShortForm == argument.ShortForm))
                throw new InvalidOperationException($"A command line argument with the same short form was already added. ({argument})");
            
            if(_arguments.Any(p => p.LongForm == argument.LongForm))
                throw new InvalidOperationException($"A command line argument with the same long form was already added. ({argument})");
                
            _arguments.Add(argument);

            if (argument.LongForm == "--help" || argument.ShortForm == "-h" && _helpArgument == null)
                _helpArgument = argument;
            
            return argument;
        }
        
        public void PrintHelp()
        {
            var helpPrinter = new CommandLineHelpPrinter(this);
            helpPrinter.Print();
        }

        public void Parse(ArraySegment<string> args)
        {
            Parse(args.Array, args.Offset);
        }

        public bool Parse(string[] args, int firstArg = 0)
        {
            var posArgs = _arguments.Where(p => p.IsPositionalArgument).ToList();
            var posArgsEnumerator = new PositionalArgumentsEnumerator(posArgs);
            var mustHaveArgumentCount = posArgs.Count(p => !p.IsOptional);

            if (mustHaveArgumentCount + MinArgumentCount > args.Length - firstArg)
            {
                Console.WriteLine("Missing argument.");
                PrintHelp();
                return false;
            }
            
            for (int i = firstArg; i < args.Length; i++)
            {
                var arg = args[i];
                var valuePart = string.Empty;
                var valueSeparatorIndex = GetValueSeparatorIndex(arg);

                if (valueSeparatorIndex != -1)
                {
                    if (valueSeparatorIndex + 1 < arg.Length)
                        valuePart = arg.Substring(valueSeparatorIndex + 1);
                    
                    arg = arg.Substring(0, valueSeparatorIndex);
                }
                
                var foundArgument = _arguments.FirstOrDefault(p => p.ShortForm == arg || p.LongForm == arg);
                
                if(foundArgument == null)
                {
                    if (posArgsEnumerator.MoveNext())
                    {
                        var current = posArgsEnumerator.Current;
                        current.PosOrValueAction(arg);
                        continue;
                    }

                    Console.WriteLine("Unknown argument: " + arg);
                    _helpArgument?.Action(EmptyForwardedArguments);
                    return false;
                }

                if (foundArgument.HasSubArguments)
                {
                    var nextI = i + 1;

                    foundArgument.Action(nextI < args.Length
                                                 ? new ArraySegment<string>(args, nextI, args.Length - nextI)
                                                 : EmptyForwardedArguments);

                    return true;
                }
                
                if (foundArgument.HasValue)
                    foundArgument.PosOrValueAction(valuePart);
                else if (!string.IsNullOrWhiteSpace(valuePart))
                {
                    Console.WriteLine("Argument " + arg + " does not support values.");
                    _helpArgument?.Action(EmptyForwardedArguments);
                    return false;
                }
                else
                    foundArgument.Action(EmptyForwardedArguments);
            }

            return true;
        }

        private int GetValueSeparatorIndex(string arg)
        {
            var index = arg.IndexOfOutsideQuotes('=');

            if (index == -1)
                index = arg.IndexOfOutsideQuotes(':');

            return index;
        }
    }
}