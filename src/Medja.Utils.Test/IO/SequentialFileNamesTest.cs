using System.IO;
using Medja.Utils.IO;
using Xunit;

namespace Medja.Utils.Test.IO
{
    public class SequentialFileNamesTest
    {
        [Fact]
        public void DoubleDotFullNameTest()
        {
            // the given baseName lead to an error (only when used the FullName, because the way we
            // extracted the dir path was wrong
            var sequentialFileNames = new SequentialFileNames(new FileInfo("./baseName.1024.").FullName);
            sequentialFileNames.Load();
        }

        [Fact]
        public void UseFullNameTest()
        {
            var fileInfo = new FileInfo("./baseName.1024");
            var firstCreatedFullName = new FileInfo("./baseName.1024.0").FullName;
            var sequentialFileNames = new SequentialFileNames(fileInfo.FullName);

            CreateFile(fileInfo.FullName + ".0");

            sequentialFileNames.Load();

            Assert.Equal(firstCreatedFullName, sequentialFileNames.FileNames[0]);

            File.Delete(firstCreatedFullName);
        }

        private void CreateFile(string fileName)
        {
            File.Create(fileName).Dispose();
        }

        [Fact]
        public void IgnoreWrongFilesTest()
        {
            CreateFile("./base.0");
            CreateFile("./base..0");
            CreateFile("./base...0");

            var baseName = "./base.";

            var sequentialFileNames = new SequentialFileNames(baseName);
            sequentialFileNames.Load();

            Assert.Collection(sequentialFileNames.FileNames, p => Assert.Equal(new FileInfo("./base..0").FullName, p));

            File.Delete("./base.0");
            File.Delete("./base..0");
            File.Delete("./base...0");
        }
    }
}
