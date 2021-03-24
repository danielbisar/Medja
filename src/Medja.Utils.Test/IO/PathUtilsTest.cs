using System.IO;
using Medja.Utils.IO;
using Xunit;

namespace Medja.Utils.Test.IO
{
    public class PathUtilsTest
    {
        [Fact]
        public void GetDirectoryPathIsAbsolute()
        {
            var dir = PathUtils.GetDirectoryPath("file.txt");

            Assert.True(dir.Length > 1);
            Assert.True(Directory.Exists(dir));
            Assert.True(Path.IsPathRooted(dir));
        }

        [Fact]
        public void GetDirectoryPathFromFileName()
        {
            var dir = Path.GetFullPath(".");
            var fileName = "./test.txt";
            var dirFromFileName = PathUtils.GetDirectoryPath(fileName);

            Assert.Equal(dir, dirFromFileName);
        }

        [Fact]
        public void GetDirectoryPathFromFileName2()
        {
            var dir = Path.GetFullPath(".");
            var fileName = "test.txt";
            var dirFromFileName = PathUtils.GetDirectoryPath(fileName);

            Assert.Equal(dir, dirFromFileName);
        }
    }
}
