using System.IO;
using Medja.Utils.IO;
using Xunit;

namespace Medja.Utils.Test
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
    }
}