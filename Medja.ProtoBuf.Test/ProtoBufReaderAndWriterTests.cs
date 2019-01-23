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
    }
}