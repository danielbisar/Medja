using System.Linq;
using System.Threading.Tasks;
using Medja.Utils.Collections.Concurrent;
using Medja.Utils.Threading.Tasks;
using Xunit;

namespace Medja.Utils.Test
{
    public class CommandLineArgumentParserTest
    {
        [Fact]
        public void PositionalArgumentTest()
        {
            var targetFileName = "";
            
            // the test was created as a bug repo
            var parser = new CommandLineArgumentParser();
            parser.AddHelpArgument();
            parser.AddArgumentWithSubArgs("--benchmark", "-b", "runs different benchmarks", subArgs =>
            {
                var subParser = new CommandLineArgumentParser();
                subParser.AddHelpArgument();
                subParser.AddPositionalArgument("target", "The target (file) to use for the benchmark", val => targetFileName = val);
                subParser.Parse(subArgs);
            });

            var args = new[] {"-b", "targetFileName"};
            
            parser.Parse(args);
            
            Assert.Equal("targetFileName", targetFileName);
        }
    }
}