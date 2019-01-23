using System.IO;
using Xunit;

namespace Medja.ProtoBuf.Test
{
   
    public class ProtoBufReaderAndWriterTests
    {
        [Fact]
        public void SimpleReadWriteTest()
        {
            using (var memoryStream = new MemoryStream())
            {
                var testObj1 = new ProtoTestObj();
                testObj1.SomeInt = 1;
                testObj1.SomeText = "My text 1";
                
                var testObj2 = new ProtoTestObj();
                testObj2.SomeInt = 2;
                testObj2.SomeText = "My text 2";
                
                var writer = new ProtoBufWriter(memoryStream, false);
                writer.Write(testObj1);
                writer.Write(testObj2);
                writer.Dispose();

                memoryStream.Seek(0, SeekOrigin.Begin);
                
                var reader = new ProtoBufReader(memoryStream, false);
                var obj1 = reader.Read();
                var obj2 = reader.Read();
                reader.Dispose();
                
                Assert.NotSame(obj1,testObj1);
                Assert.NotSame(obj2,testObj2);
                
                Assert.Equal(obj1, testObj1);
                Assert.Equal(obj2, testObj2);
            }
        }

        [Fact]
        public void ReaderSeekTest()
        {
            using (var memoryStream = new MemoryStream())
            {
                var testObj1 = new ProtoTestObj();
                testObj1.SomeInt = 1;
                testObj1.SomeText = "My text 1";
                
                var testObj2 = new ProtoTestObj();
                testObj2.SomeInt = 2;
                testObj2.SomeText = "My text 2";
                
                var testObj3 = new ProtoTestObj();
                testObj2.SomeInt = 3;
                testObj2.SomeText = "My text 3";
                
                var testObj4 = new ProtoTestObj();
                testObj2.SomeInt = 4;
                testObj2.SomeText = "My text 4";
                
                var writer = new ProtoBufWriter(memoryStream, false);
                writer.Write(testObj1);
                writer.Write(testObj2);
                writer.Write(testObj3);
                writer.Write(testObj4);
                writer.Dispose();

                memoryStream.Seek(0, SeekOrigin.Begin);
                
                var reader = new ProtoBufReader(memoryStream, false);
                var obj1 = reader.Read();
                var obj2 = reader.Read();
                
                Assert.Equal(testObj1, obj1);
                Assert.Equal(testObj2, obj2);
                
                reader.Seek(0, SeekOrigin.Begin);
                
                Assert.Equal(obj1, reader.Read());
                Assert.Equal(obj2, reader.Read());
                
                // seek further then read until now
                reader.Seek(3, SeekOrigin.Begin); // seek for the last message in the stream
                
                Assert.Equal(testObj4, reader.Read());
                
                reader.Dispose();
                
                Assert.Equal(obj1, testObj1);
                Assert.Equal(obj2, testObj2);
            }
        }
    }
}