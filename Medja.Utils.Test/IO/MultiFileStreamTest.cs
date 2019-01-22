using System;
using System.Collections.Generic;
using System.IO;
using Medja.Utils.IO;
using Xunit;

namespace Medja.Utils.Test
{
    public class MultiFileStreamTest
    {
        private IReadOnlyList<string> WriteFiles(string baseName, long maxFileSize, byte bufferSize, FileMode fileMode = FileMode.OpenOrCreate)
        {
            using (var multiFileStream = new MultiFileStream(new MultiFileStreamSettings
            {
                    BaseName = baseName,
                    FileMode = fileMode,
                    MaxFileSize = maxFileSize
            }))
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
        
        private void WriteTest(string baseName, long maxFileSize, byte bufferSize, Action<IReadOnlyList<string>> assertFiles, FileMode fileMode = FileMode.OpenOrCreate)
        {
            IReadOnlyList<string> fileNames = null;
            
            try
            {
                fileNames = WriteFiles(baseName, maxFileSize, bufferSize, fileMode);
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

            using (var multiFileStream = new MultiFileStream(new MultiFileStreamSettings
            {
                    BaseName = baseName, FileMode = FileMode.Open
            }))
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

            using (var multiFileStream = new MultiFileStream(new MultiFileStreamSettings
            {
                    BaseName = baseName, FileMode = FileMode.Open
            }))
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

            using (var multiFileStream = new MultiFileStream(new MultiFileStreamSettings
            {
                    BaseName = baseName, FileMode = FileMode.Open
            }))
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
            
            using (multiFileStream = new MultiFileStream(new MultiFileStreamSettings
            {
                    BaseName = baseName, FileMode = FileMode.OpenOrCreate,
                    MaxFileSize = 10
            }))
            {
                Assert.True(multiFileStream.CanWrite);
            }
            
            File.Delete(baseName + ".0");
            Assert.False(multiFileStream.CanWrite);
        }

        [Fact]
        public void FileModeCreateTest()
        {
            var baseName = "./fileModeCreateTest";
            
            WriteTest(baseName, 10, 10*3, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(baseName+".0",p),
                                  p => Assert.Equal(baseName+".1",p),
                                  p => Assert.Equal(baseName+".2",p));
            }, FileMode.Create);
            
            File.Create(baseName + ".0").Dispose();
            File.Create(baseName + ".1").Dispose();
            
            WriteTest(baseName, 10, 10*3, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(baseName+".0",p),
                                  p => Assert.Equal(baseName+".1",p),
                                  p => Assert.Equal(baseName+".2",p));
            }, FileMode.Create);
        }
        
        [Fact]
        public void FileModeCreateNewTest()
        {
            var baseName = "./fileModeCreateNewTest";
            File.Create(baseName + ".0").Dispose();

            Assert.Throws<IOException>(() => 
            {
                using (var multiFileStream = new MultiFileStream(new MultiFileStreamSettings
                {
                        BaseName = baseName,
                        FileMode = FileMode.CreateNew
                }))
                {
                    // we should never reach that point
                    Assert.False(multiFileStream.CanWrite);
                }
            });
            
            File.Delete(baseName + ".0");
            
            WriteTest(baseName, 10, 10*3, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(baseName+".0",p),
                                  p => Assert.Equal(baseName+".1",p),
                                  p => Assert.Equal(baseName+".2",p));
            }, FileMode.CreateNew);
        }
    }
}