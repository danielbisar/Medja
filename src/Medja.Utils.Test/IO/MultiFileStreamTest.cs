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
            var fullBaseName = new FileInfo(baseName).FullName;
            
            WriteTest(baseName, 10, 10*3, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(fullBaseName+".0",p),
                                  p => Assert.Equal(fullBaseName+".1",p),
                                  p => Assert.Equal(fullBaseName+".2",p));
            });
        }
        
        [Fact]
        public void WriteSmallerBufferTest()
        {
            var baseName = "./smallerBuffer";
            var fullBaseName = new FileInfo(baseName).FullName;
            
            WriteTest(baseName, 100, 10, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(fullBaseName+".0",p));
            });
        }
        
        [Fact]
        public void WriteFileSizeBufferTest()
        {
            var baseName = "./fileSizeBuffer";
            var fullBaseName = new FileInfo(baseName).FullName;
            
            WriteTest(baseName, 100, 100, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(fullBaseName+".0",p));
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
            var fullBaseName = new FileInfo(baseName).FullName;
            
            WriteTest(baseName, 10, 10*3, fileNames =>
            {
                
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(fullBaseName+".0",p),
                                  p => Assert.Equal(fullBaseName+".1",p),
                                  p => Assert.Equal(fullBaseName+".2",p));
            }, FileMode.Create);
            
            File.Create(fullBaseName + ".0").Dispose();
            File.Create(fullBaseName + ".1").Dispose();
            
            WriteTest(baseName, 10, 10*3, fileNames =>
            {
                Assert.Collection(fileNames, 
                                  p => Assert.Equal(fullBaseName+".0",p),
                                  p => Assert.Equal(fullBaseName+".1",p),
                                  p => Assert.Equal(fullBaseName+".2",p));
            }, FileMode.Create);
        }
        
        [Fact]
        public void FileModeCreateNewTest()
        {
            var baseName = "./fileModeCreateNewTest";
            var fullBaseName = new FileInfo(baseName).FullName;
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
                                  p => Assert.Equal(fullBaseName+".0",p),
                                  p => Assert.Equal(fullBaseName+".1",p),
                                  p => Assert.Equal(fullBaseName+".2",p));
            }, FileMode.CreateNew);
        }

        [Fact]
        public void SeekTest()
        {
            var baseName = "./seekTest";
            var fileNames = WriteFiles(baseName, 100, 255, FileMode.CreateNew);

            try
            {
                using (var multiFileStream = new MultiFileStream(new MultiFileStreamSettings
                {
                        BaseName = baseName,
                        FileMode = FileMode.Open,
                        FileAccess = FileAccess.Read
                }))
                {
                    Assert.True(multiFileStream.CanSeek);
                
                    var buffer = new byte[3];
                
                    multiFileStream.Seek(100, SeekOrigin.Begin);
                    multiFileStream.Read(buffer);
                
                    Assert.Collection(buffer, 
                                      b => Assert.Equal(100, b),
                                      b => Assert.Equal(101, b),
                                      b => Assert.Equal(102, b));

                    multiFileStream.Seek(-103, SeekOrigin.Current);
                    multiFileStream.Read(buffer);
                
                    Assert.Collection(buffer, 
                                      b => Assert.Equal(0, b),
                                      b => Assert.Equal(1, b),
                                      b => Assert.Equal(2, b));
                
                    multiFileStream.Seek(-3, SeekOrigin.End);
                    multiFileStream.Read(buffer);
                    
                    Assert.Collection(buffer, 
                                      b => Assert.Equal(252, b),
                                      b => Assert.Equal(253, b),
                                      b => Assert.Equal(254, b));
                }
            }
            finally
            {
                DeleteFiles(fileNames);
            }
        }

        private void DeleteFiles(IEnumerable<string> fileNames)
        {
            foreach(var fileName in fileNames)
                File.Delete(fileName);
        }
    }
}