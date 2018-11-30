using System;
using System.Linq;
using System.Text;

namespace Medja.Utils
{
    public class CommandLineHelpPrinter
    {
        private static readonly string SubArgumentStr = "<option>";
        private readonly CommandLineArgumentParser _parser;
        
        public CommandLineHelpPrinter(CommandLineArgumentParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public void Print()
        {
            var helpText = GetHelpText();
            
            Console.WriteLine(helpText);
        }

        public string GetHelpText()
        {
            var firstColumnWidth = GetFirstColumnWidth();
            
            var sb = new StringBuilder();

            if (_parser.HelpHeader == null)
                sb.AppendLine("Usage: ");
            else
                sb.AppendLine(_parser.HelpHeader);

            var sortedArguments = _parser.Arguments.OrderBy(p => p.LongForm);
            //var argumentFormat = string.Format("{{0,-{0}}}\t{{1,-{1}}}", firstColumnWidth, 80 - firstColumnWidth);
            var argumentDescFormat = "{0,-" + (80 - firstColumnWidth).ToString() + "}";

            foreach (var argument in sortedArguments)
            {
                var addedLength = argument.LongForm.Length;
                
                sb.Append(argument.LongForm);

                if (!string.IsNullOrEmpty(argument.ShortForm))
                {
                    sb.Append(", ");
                    sb.Append(argument.ShortForm);

                    addedLength += 2 + argument.ShortForm.Length;
                }

                if (argument.HasSubArguments)
                {
                    sb.Append(" ");
                    sb.Append(SubArgumentStr);
                    
                    addedLength += 1 + SubArgumentStr.Length;
                }

                sb.Append(' ', firstColumnWidth - addedLength);
                
                sb.AppendFormat(argumentDescFormat, argument.HelpText);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private int GetFirstColumnWidth()
        {
            return _parser.Arguments.Max(GetArgumentLength);
        }

        private int GetArgumentLength(CommandLineArgument argument)
        {
            // at least one space after the text before the description
            var result = argument.LongForm.Length + 1;

            // 2 for ", "
            if (!string.IsNullOrEmpty(argument.ShortForm))
                result += 2 + argument.ShortForm.Length;

            // 1 for " " before the <option> string
            if (argument.HasSubArguments)
                result += 1 + SubArgumentStr.Length;

            // at least 10 chars for the first column
            return System.Math.Max(10, result);
        }
    }
}