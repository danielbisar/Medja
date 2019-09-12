using Medja.Input;
using Xunit;
using Medja.OpenTk.Input;
using OpenTK.Input;

namespace Medja.OpenTk.Test.Input
{
    public class KeyMapTest
    {
        // TODO write test for GetModifierKeys

        [Fact]
        public void GetKeyCharForSpecialKeys()
        {
            var map = new KeyMap();
            
            Assert.Equal(Keys.Backspace, map.GetKey(Key.BackSpace));
            Assert.Equal(Keys.Delete, map.GetKey(Key.Delete));
            Assert.Equal(Keys.Left, map.GetKey(Key.Left));
            Assert.Equal(Keys.Right, map.GetKey(Key.Right));
            Assert.Equal(Keys.Up, map.GetKey(Key.Up));
            Assert.Equal(Keys.Down, map.GetKey(Key.Down));
        }

    }
}