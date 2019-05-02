using System;

namespace Medja.Utils
{
    public class CommandLineArgument
    {
        public string ShortForm { get; set; }
        public string LongForm { get; set; }
        public string HelpText { get; set; }
        
        /// <summary>
        /// If set to true the argument has a value following (--my-arg=myValue)
        /// </summary>
        public bool HasValue { get; set; }
        
        /// <summary>
        /// Action that will be used for default arguments. If HasSubArguments is null the parameter will be null too.
        /// </summary>
        public Action<ArraySegment<string>> Action { get; set; }
        
        /// <summary>
        /// Action that is used for positional or value arguments.
        /// </summary>
        public Action<string> PosOrValueAction { get; set; }

        /// <summary>
        /// Gets if this CommandLineArgument is positional (means no parameters than the argument itself and no fixed
        /// string) but a fixed position inside the arguments string.
        /// </summary>
        /// <remarks>
        /// Example:
        /// Program.exe fileName.txt  
        /// </remarks>
        public bool IsPositionalArgument { get; set; }
        
        public bool HasSubArguments { get; set; }
        
        public bool IsOptional { get; set; }
        
        /// <summary>
        /// Gets or sets the value string. (The part that will be printed in the help-text after the argument name)
        /// </summary>
        public string ValueString { get; set; }

        public override string ToString()
        {
            return $"ShortForm = {ShortForm}, LongForm = {LongForm}, HelpText = '{HelpText}', HasSubArguments = {HasSubArguments.ToString()}, HasValue = {HasValue.ToString()}";
        }
    }
}