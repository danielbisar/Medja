using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Medja.Utils.Test.Serialization
{
    public class MedjaSerializerTest
    {
        [Theory]
        [InlineData("MedjaSerializer")]
        [InlineData("IMedjaSerializer")]
        public void UnsupportedSerializationTypesAreCompileTimeObsolete(string typeName)
        {
            var type = Type.GetType("Medja.Utils.Serialization." + typeName + ", Medja.Utils", true);
            var obsolete = type.GetCustomAttribute<ObsoleteAttribute>();

            Assert.NotNull(obsolete);
            Assert.True(obsolete.IsError);
            Assert.Contains("not implemented", obsolete.Message);
        }

        [Theory]
        [InlineData("WriteTo")]
        [InlineData("ReadFrom")]
        public void UnsupportedSerializationFailsWithoutChangingStream(string methodName)
        {
            // Reflection permits checking the runtime contract of the compile-time obsolete API.
            var type = Type.GetType("Medja.Utils.Serialization.MedjaSerializer, Medja.Utils", true);
            var serializer = Activator.CreateInstance(type, new object[] { new object() });
            var data = new byte[] { 1, 2, 3 };

            using (var stream = new MemoryStream(data))
            {
                stream.Position = 1;

                var exception = Assert.Throws<TargetInvocationException>(() =>
                    type.GetMethod(methodName).Invoke(serializer, new object[] { stream }));

                Assert.IsType<NotSupportedException>(exception.InnerException);
                Assert.Equal(1L, stream.Position);
                Assert.Equal(data, stream.ToArray());
            }
        }
    }
}
