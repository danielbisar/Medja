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
            
            Assert.Equal((char)Keys.Backspace, map.GetKeyChar(Key.BackSpace));
            Assert.Equal((char)Keys.Delete, map.GetKeyChar(Key.Delete));
            Assert.Equal((char)Keys.Left, map.GetKeyChar(Key.Left));
            Assert.Equal((char)Keys.Right, map.GetKeyChar(Key.Right));
            Assert.Equal((char)Keys.Up, map.GetKeyChar(Key.Up));
            Assert.Equal((char)Keys.Down, map.GetKeyChar(Key.Down));
        }
        
        [Fact]
        public void GetKeyCharReturnsNullForStandardKey()
        {
            var map = new KeyMap();
            
            Assert.Null(map.GetKeyChar(Key.A));
        }
    }
}