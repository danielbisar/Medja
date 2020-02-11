using System;
using System.IO;
using System.Text;
using System.Threading;
using Medja.Utils.IO;
using Xunit;

namespace Medja.Utils.Test
{
    public class FileFollowReaderTest
    {
        [Fact]
        public void GetNotifiedWhenWritten()
        {
            var fileName = "notified_when_written.txt";
            var filePath = Path.GetFullPath(fileName);

            using (var stream = new StreamWriter(filePath))
            {
                stream.WriteLine("test");
                stream.Flush();

                var dataChangedEventCount = 0;
                var content = new StringBuilder();

                using (var ffr = new FileFollowReader(fileName))
                {
                    ffr.DataRead += (s, e) =>
                    {
                        content.Append(e.Data);
                        dataChangedEventCount++;
                    };
                    ffr.Start();

                    Assert.Equal(1, dataChangedEventCount);
                    Assert.Equal("test" + Environment.NewLine, content.ToString());

                    stream.WriteLine("test2");
                    stream.Flush();

                    Thread.Sleep(100);

                    Assert.Equal(2, dataChangedEventCount);
                    Assert.Equal("test" + Environment.NewLine + "test2" + Environment.NewLine,
                        content.ToString());
                }
            }
        }

        [Fact]
        public void NoNotificationAfterDispose()
        {
            var fileName = "nonotification_after_dispose.txt";
            var filePath = Path.GetFullPath(fileName);

            using (var stream = new StreamWriter(filePath))
            {
                stream.WriteLine("test");
                stream.Flush();

                var dataChangedEventCount = 0;
                var content = new StringBuilder();

                var ffr = new FileFollowReader(fileName);
                ffr.DataRead += (s, e) =>
                {
                    content.Append(e.Data);
                    dataChangedEventCount++;
                };
                ffr.Start();

                Assert.Equal(1, dataChangedEventCount);
                Assert.Equal("test" + Environment.NewLine, content.ToString());

                ffr.Dispose();

                // write after ffr is disposed
                stream.WriteLine("test2");
                stream.Flush();

                Thread.Sleep(100);

                Assert.Equal(1, dataChangedEventCount);
                Assert.Equal("test" + Environment.NewLine,
                    content.ToString());
            }
        }
    }
}