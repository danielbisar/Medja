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

        private void TestValueArgumentParser(CommandLineArgumentParser parser, ref string value)
        {
            var args = new[] {"-v:val"};
            var args2 = new[] {"--value=val2"};
            var args3 = new[] {"--value="};
            var args4 = new[] {"--value'='"};

            Assert.True(parser.Parse(args));
            Assert.Equal("val", value);

            Assert.True(parser.Parse(args2));
            Assert.Equal("val2", value);
            
            Assert.True(parser.Parse(args3));
            Assert.Equal("", value);
            
            Assert.False(parser.Parse(args4));
        }

        [Fact]
        public void ValueArgumentTest()
        {
            var value = string.Empty;
            
            var parser = new CommandLineArgumentParser();
            parser.AddArgument(new CommandLineArgument
            {
                    HasValue = true,
                    ShortForm = "-v",
                    LongForm = "--value",
                    PosOrValueAction = v => value = v 
            });

            TestValueArgumentParser(parser, ref value);

            value = string.Empty;
            parser = new CommandLineArgumentParser();
            parser.AddValueArgument("--value", "-v", "bla", v => value = v);
            
            TestValueArgumentParser(parser, ref value);
        }

        [Fact]
        public void FlagTest()
        {
            var flag = false;
            var parser = new CommandLineArgumentParser();
            parser.AddArgument("--flag", "-f", "Some flag", () => flag = true);
            
            var args = new[] {"-f"};
            var args2 = new[] {"--flag"};
            var args3 = new[] {"--value=val2"};
            var args4 = new[] {"--value=val2"};
            
            Assert.True(parser.Parse(args));
            Assert.True(flag);

            flag = false;
            Assert.True(parser.Parse(args2));
            Assert.True(flag);

            flag = false;

            Assert.False(parser.Parse(args3));
            Assert.False(flag);

            Assert.False(parser.Parse(args4));
            Assert.False(flag);
        }
    }
}