using Medja.Input;
using Medja.OpenTk.Input;
using OpenTK.Input;
using Xunit;

namespace Medja.OpenTk.Test.Input
{
    public class KeyMapTest
    {
        // OpenTKs keyboard state cannot be created except via reflection so we leaf this for now, since i guess this
        // will change with version 4
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