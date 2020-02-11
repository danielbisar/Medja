using System.IO;
using System.Threading;
using Medja.Utils.IO;
using Xunit;

namespace Medja.Utils.Test
{
    public class FileCreatedWatcherTest
    {
        [Fact]
        public void GetNotifiedOnFileCreated()
        {
            var fileName = "fcw_new_file.txt";

            if (File.Exists(fileName))
                File.Delete(fileName);

            using (var watcher = new FileCreatedWatcher(fileName))
            {
                var createdCount = 0;
                var createdFileName = string.Empty;
                
                watcher.Created += (s, e) =>
                {
                    createdCount++;
                    createdFileName = e.FullFilePath;
                };

                watcher.Start();

                File.WriteAllText(fileName, "...");
                Thread.Sleep(100);

                Assert.Equal(1, createdCount);
                Assert.Equal(Path.GetFullPath(fileName), createdFileName);
            }
        }

        [Fact]
        public void GetNotNotifiedOnWriteSecondTime()
        {
            // since we use LastWrite as event, verify we still only receive created events
            var fileName = "fcw_not_new_file.txt";

            if (File.Exists(fileName))
                File.Delete(fileName);

            using (var watcher = new FileCreatedWatcher(fileName))
            {
                var createdCount = 0;
                var createdFileName = string.Empty;

                watcher.Created += (s, e) =>
                {
                    createdCount++;
                    createdFileName = e.FullFilePath;
                };

                watcher.Start();

                File.WriteAllText(fileName, "...");
                Thread.Sleep(100);

                using (var sw = new StreamWriter(fileName, true))
                {
                    sw.WriteLine("abc");
                }

                Thread.Sleep(100);

                Assert.Equal(1, createdCount);
            }
        }

        [Fact]
        public void GetNotNotifiedOnRecreation()
        {
            // since we use LastWrite as event, verify we still only receive created events
            var fileName = "fcw_on_recreation.txt";

            if (File.Exists(fileName))
                File.Delete(fileName);

            using (var watcher = new FileCreatedWatcher(fileName))
            {
                var createdCount = 0;
                var createdFileName = string.Empty;

                watcher.Created += (s, e) =>
                {
                    createdCount++;
                    createdFileName = e.FullFilePath;
                };

                watcher.Start();

                File.WriteAllText(fileName, "...");
                Thread.Sleep(100);

                File.Delete(fileName);

                File.WriteAllText(fileName, "...");
                Thread.Sleep(100);

                Assert.Equal(2, createdCount);
            }
        }

        [Fact]
        public void GetNotNotifiedOnOtherFilesCreation()
        {
            // since we use LastWrite as event, verify we still only receive created events
            var fileName = "fcw_other_file_creation.txt";
            var otherFileName = "fcw_other_file_creation.txt";

            if (File.Exists(fileName))
                File.Delete(fileName);

            if (File.Exists(otherFileName))
                File.Delete(otherFileName);

            using (var watcher = new FileCreatedWatcher(fileName))
            {
                var createdCount = 0;
                var createdFileName = string.Empty;

                watcher.Created += (s, e) =>
                {
                    createdCount++;
                    createdFileName = e.FullFilePath;
                };

                watcher.Start();

                File.WriteAllText(fileName, "...");
                Thread.Sleep(100);

                File.WriteAllText(otherFileName, "...");
                Thread.Sleep(100);

                Assert.Equal(1, createdCount);
            }
        }
    }
}