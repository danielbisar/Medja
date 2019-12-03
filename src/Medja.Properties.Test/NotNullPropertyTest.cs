using System;
using Xunit;

namespace Medja.Properties.Test
{
    public class NotNullPropertyTest
    {
        [Fact]
        public void CannotInitWithNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NotNullProperty<object>(null));
        }

        [Fact]
        public void CannotSetSilentToNull()
        {
            var prop = new NotNullProperty<object>(new object());
            Assert.Throws<ArgumentNullException>(() => prop.SetSilent(null));
        }

        [Fact]
        public void CannotSetToNull()
        {
            var prop = new NotNullProperty<object>(new object());
            Assert.Throws<ArgumentNullException>(() => prop.Set(null));
        }
    }
}