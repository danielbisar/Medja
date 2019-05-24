using System.Collections.Generic;
using Medja.Input;
using OpenTK.Input;

namespace Medja.OpenTk.Input
{
    /// <summary>
    /// Translates OpenTK modifier keys to Medja modifier keys.
    /// </summary>
    public class KeyMap
    {
        private static readonly Dictionary<Key, ModifierKeys> ModifierMap;
        private static readonly Dictionary<Key, Keys> KeysMap;

        static KeyMap()
        {
            ModifierMap = new Dictionary<Key, ModifierKeys>
            {
                {Key.ShiftLeft, ModifierKeys.LeftShift},
                {Key.ShiftRight, ModifierKeys.RightShift},
				
                {Key.AltLeft, ModifierKeys.LeftAlt},
                {Key.AltRight, ModifierKeys.RightAlt},
				
                {Key.ControlLeft, ModifierKeys.LeftCtrl},
                {Key.ControlRight, ModifierKeys.RightCtrl},
				
                {Key.WinLeft, ModifierKeys.LeftSuper},
                {Key.WinRight, ModifierKeys.RightSuper}
            };

            KeysMap = new Dictionary<Key, Keys>
            {
                {Key.BackSpace, Keys.Backspace},
                {Key.Delete, Keys.Delete},
                {Key.Left, Keys.Left},
                {Key.Right, Keys.Right},
                {Key.Up, Keys.Up},
                {Key.Down, Keys.Down},
                {Key.Enter, Keys.Return},
                {Key.KeypadEnter, Keys.Return},
                {Key.Tab, Keys.Tab}
            };
        }
        
        public ModifierKeys GetModifierKeys(KeyboardState keyboardState)
        {
            var modifierKeys = ModifierKeys.None;

            foreach (var key in ModifierMap.Keys)
            {
                if (keyboardState.IsKeyDown(key))
                    modifierKeys |= ModifierMap[key];
            }

            return modifierKeys;
        }

        public Keys GetKey(Key key)
        {
            if (KeysMap.TryGetValue(key, out var result))
                return result;

            if (key >= Key.A && key <= Key.Z)
                return (Keys)(key - 18);
            
            return 0;
        }
    }
}