using System.IO;
using Medja.Utils.IO;
using Xunit;

namespace Medja.Utils.Test
{
    public class PathUtilsTest
    {
        [Fact]
        public void GetDirectoryPathIsAbsolute()
        {
            var dir = PathUtils.GetDirectoryPath(".");

            Assert.True(dir.Length > 1);
            Assert.True(Directory.Exists(dir));
            Assert.True(Path.IsPathRooted(dir));
        }
    }
}