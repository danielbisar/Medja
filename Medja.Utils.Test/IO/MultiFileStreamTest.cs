using System;
using System.Collections.Generic;
using System.IO;
using Medja.Utils.IO;
using Xunit;

namespace Medja.Utils.Test
{
    public class MultiFileStreamTest
    {
        private IReadOnlyList<string> WriteFiles(string baseName, long maxFileSize, byte bufferSize)
        {
            using (var multiFileStream = new MultiFileStream(baseName, FileMode.OpenOrCreate, maxFileSize))
            {
                var buffer = new byte[bufferSize];

                for (byte i = 0; i < buffer.Length; i++)
                    buffer[i] = i;
                
                // the actual test
                multiFileStream.Write(buffer);
                
                multiFileStream.Flush();
                
                return multiFileStream.GetFileNames();
            }
        }
        
        private void WriteTest(string baseName, long maxFileSize, byte bufferSize, Action<IReadOnlyList<string>> assertFiles)
        {
            IReadOnlyList<string> fileNames = null;
            
            try
            {
                fileNames = WriteFiles(baseName, maxFileSize, bufferSize);
                assertFiles(fileNames);
            }
            finally
            {
                if (fileNames != null)
                {
                    var nonExistingFiles = new List<string>();
                    
                    // remove the files, at the same time this tests if the files existed
                    foreach (var file in fileNames)
                    {
                        if(!File.Exists(file))
                            nonExistingFiles.Add(file);
                        else
                            File.Delete(file);
                    }
                    
                    Assert.Empty(nonExistingFiles);
                }
            }
            
        }
        
        [Fact]
        public void ReadSmallerBufferTest()
        {
            var baseName = "./readSmallerBuffer";
            
            WriteFiles(baseName, 10, 30);

            using (var multiFileStream = new MultiFileStream(baseName, FileMode.Open, 0))
            {
                var buffer = new byte[5];
                var readBytes = multiFileStream.Read(buffer);
                
                Assert.Equal(5, readBytes);
                
                for(byte i = 0; i < readBytes; i++)
                    Assert.Equal(i, buffer[i]);
            }
        }
        
        [Fact]
        public void ReadFileSizeBufferTest()
        {
            var baseName = "./readFileSizeBufferTest";
            
            WriteFiles(baseName, 10, 30);

            using (var multiFileStream = new MultiFileStream(baseName, FileMode.Open, 0))
            {
                var buffer = new byte[10];
                var readBytes = multiFileStream.Read(buffer);
                
                Assert.Equal(10, readBytes);
                
                for(byte i = 0; i < readBytes; i++)
                    Assert.Equal(i, buffer[i]);
            }
        }
        
        [Fact]
        public void ReadMultipleOverflowTest()
        {
            var baseName = "./readMultioverflowTest";
            
            WriteFiles(baseName, 10, 30);

            using (var multiFileStream = new MultiFileStream(baseName, FileMode.Open, 0))
            {
                var buffer = new byte[30];
                var readBytes = multiFileStream.Read(buffer);
                
                Assert.Equal(30, readBytes);
                
                for(byte i = 0; i < readBytes; i++)
                    Assert.Equal(i, buffer[i]);
            }
        }
        
        [Fact]
        public void WriteMultiOverflowBufferTest()
        {
            var baseName = "./multiOverflowBuffer";
            
            WriteTest(baseName, 10, 10*3, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(baseName+".0",p),
                                  p => Assert.Equal(baseName+".1",p),
                                  p => Assert.Equal(baseName+".2",p));
            });
        }
        
        [Fact]
        public void WriteSmallerBufferTest()
        {
            var baseName = "./smallerBuffer";
            
            WriteTest(baseName, 100, 10, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(baseName+".0",p));
            });
        }
        
        [Fact]
        public void WriteFileSizeBufferTest()
        {
            var baseName = "./fileSizeBuffer";
            
            WriteTest(baseName, 100, 100, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(baseName+".0",p));
            });
        }

        [Fact]
        public void CanWriteTest()
        {
            MultiFileStream multiFileStream = null;

            var baseName = "./canWrite";
            
            using (multiFileStream = new MultiFileStream(baseName, FileMode.OpenOrCreate, 10))
            {
                Assert.True(multiFileStream.CanWrite);
            }
            
            File.Delete(baseName + ".0");
            Assert.False(multiFileStream.CanWrite);
        }
    }
}