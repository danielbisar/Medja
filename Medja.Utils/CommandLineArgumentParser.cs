using System;
using System.Collections.Generic;
using System.Linq;

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
                    PositionalAction = action
            });
        }

        public CommandLineArgument AddHelpArgument()
        {
            return AddArgument("--help", "-h", "prints this screen", PrintHelp);
        }

        private CommandLineArgument AddArgument(CommandLineArgument argument)
        {
            if(argument == null)
                throw new ArgumentNullException(nameof(argument));

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

        public void Parse(string[] args, int firstArg = 0)
        {
            var posArgs = _arguments.Where(p => p.IsPositionalArgument).ToList();
            var posArgsEnumerator = new PositionalArgumentsEnumerator(posArgs);
            var mustHaveArgumentCount = posArgs.Count(p => !p.IsOptional);

            if (mustHaveArgumentCount + MinArgumentCount > args.Length - firstArg)
            {
                Console.WriteLine("Missing argument.");
                PrintHelp();
                return;
            }
            
            for (int i = firstArg; i < args.Length; i++)
            {
                var arg = args[i];
                var foundArgument = _arguments.FirstOrDefault(p => p.ShortForm == arg || p.LongForm == arg);
                
                if(foundArgument == null)
                {
                    if (posArgsEnumerator.MoveNext())
                    {
                        var current = posArgsEnumerator.Current;
                        current.PositionalAction(arg);
                    }
                    else
                    {
                        Console.WriteLine("Unknown argument: " + arg);
                        _helpArgument?.Action(EmptyForwardedArguments);
                    }
                    
                    return;
                }

                ArraySegment<string> forwardedArguments;
                var nextI = i + 1;
                var appliedSubArgs = false;

                if (foundArgument.HasSubArguments && nextI < args.Length)
                {
                    appliedSubArgs = true;
                    forwardedArguments = new ArraySegment<string>(args, nextI, args.Length - nextI);
                }
                else
                    forwardedArguments = EmptyForwardedArguments;

                foundArgument.Action(forwardedArguments);

                if (appliedSubArgs)
                    return;
            }
        }
    }
}