using System;
using Medja.Controls;
using Medja.Input;
using OpenTK;
using OpenTK.Input;

namespace Medja.OpenTk.Input
{
    public class OpenTKKeyboardHandler
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        // to make clear we have reference via the event registration in the ctor
        private readonly GameWindow _window;
        
        private readonly FocusManager _focusManager;
        private readonly KeyMap _keyMap;
        private ModifierKeys _modifierKeys;

        public OpenTKKeyboardHandler(GameWindow window, FocusManager focusManager)
        {
            _window = window;
            _focusManager = focusManager;

            _keyMap = new KeyMap();
            
            _window.KeyPress += OnKeyPressed;
            _window.KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            _modifierKeys = _keyMap.GetModifierKeys(e.Keyboard);
            var key = _keyMap.GetKey(e.Key);
            
            if(key == 0)
                return;
            
            // on key presses will not trigger for
            //
            // ctrl+key
            // arrow keys and others
            //
            // so we create the event manually
            if(_modifierKeys.HasControl() && key != 0 
               || !char.IsLetterOrDigit((char)key))
                NotifyKeyPressed(new KeyboardEventArgs(key, _modifierKeys));
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            // ignores the key press that is send from mac os x on pressing fn+backspace
            // which gets translated into a OnKeyDown Delete + additional OnKeyPressed with this code
            if(e.KeyChar == 63272) 
                return;
            
            // does not trigger for ctrl+key only for shift+key or alt+key, at least on linux
            NotifyKeyPressed(new KeyboardEventArgs(e.KeyChar, _modifierKeys));
        }

        private void NotifyKeyPressed(KeyboardEventArgs keyboardEventArgs)
        {
            var focusedControl = _focusManager.GetFocused();

            if (focusedControl != null)
            {
                focusedControl.InputState.NotifyKeyPressed(keyboardEventArgs);
            }
        }
    }
}
