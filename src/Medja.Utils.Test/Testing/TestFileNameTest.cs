using System.IO;
using Medja.Utils.Testing;
using Xunit;

namespace Medja.Utils.Test.Testing
{
    public class TestFileNameTest
    {
        [Fact]
        public void GetNotExisting()
        {
            var testFileName = TestFileName.GetNotExisting();
            
            Assert.True(!File.Exists(testFileName));

            using (var file = File.Create(testFileName))
            {
                file.Close();
            }
            
            File.Delete(testFileName);
        }
    }
}