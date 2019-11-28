using System;
using Medja.OpenTk.Components3D;
using Medja.OpenTk.Components3D.Vertices;
using Xunit;

namespace Medja.OpenTk.Test.Components3D
{
    public class ShaderFactoryTest
    {
        [Fact]
        public void CreateDefaultVertexShaderCodeThrowsNullRefExIfVAOIsNotSet()
        {
            var config = new VertexShaderGenConfig();
            
            Assert.Null(config.VertexArrayObject);
            Assert.Throws<NullReferenceException>(() => ShaderFactory.CreateDefaultVertexShaderCode(config, ""));
        }
    }
}