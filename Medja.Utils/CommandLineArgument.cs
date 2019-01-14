using System;

namespace Medja.Utils
{
    public class CommandLineArgument
    {
        public string ShortForm { get; set; }
        public string LongForm { get; set; }
        public string HelpText { get; set; }
        
        /// <summary>
        /// Action that will be used for default arguments. If HasSubArguments is null the parameter will be null too.
        /// </summary>
        public Action<ArraySegment<string>> Action { get; set; }
        
        /// <summary>
        /// Action that is used for positional arguments.
        /// </summary>
        public Action<string> PositionalAction { get; set; }

        /// <summary>
        /// Gets if this CommandLineArgument is positional (means no parameters than the argument itself and no fixed
        /// string) but a fixed position inside the arguments string.
        /// </summary>
        /// <remarks>
        /// Example:
        /// Program.exe fileName.txt  
        /// </remarks>
        public bool IsPositionalArgument
        {
            get { return PositionalAction != null; }
        }
        
        public bool HasSubArguments { get; set; }
        
        public bool IsOptional { get; set; }

        public override string ToString()
        {
            return $"ShortForm = {ShortForm}, LongForm = {LongForm}, HelpText = '{HelpText}', HasSubArguments = {HasSubArguments.ToString()}";
        }
    }
}