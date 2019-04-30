using System;
using Xunit;
using Xunit.Abstractions;

namespace Medja.NativeInterfaces.Test
{
    public class MedjaNativeTest
    {
        private readonly ITestOutputHelper _output;

        public MedjaNativeTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void HasNativeDll()
        {
            // this is not a real test but to allow debugging 
            // on which library implementation the test run
            var medjaNative = MedjaNativeFactory.Create();
            _output.WriteLine("MedjaNative implementation: " + medjaNative.GetType().FullName);
        }
        
        [Fact]
        public void GetSystemInfo()
        {
            var medjaNative = MedjaNativeFactory.Create();
            var info = medjaNative.GetSystemInfo();
            
            Assert.NotEmpty(info.Screens);
        }
    }
}