using System.ComponentModel;

namespace NHotPhrase.Keyboard
{
    public class GlobalKeyboardHookEventArgs : HandledEventArgs
    {
        public KeyboardState KeyboardState { get; set; }
        public LowLevelKeyboardInputEvent KeyboardData { get; set; }

        public GlobalKeyboardHookEventArgs(
            LowLevelKeyboardInputEvent keyboardData,
            KeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }
    }
}