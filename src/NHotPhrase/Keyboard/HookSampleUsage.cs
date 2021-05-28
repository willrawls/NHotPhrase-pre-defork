using System;
using System.Diagnostics;

namespace NHotPhrase.Keyboard
{
    public class HookSampleUsage : IDisposable
    {
        public GlobalKeyboardHook _globalKeyboardHook;

        public void SetupKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook(OnKeyPressed);
        }

        public void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            Debug.WriteLine(e.KeyboardData.VirtualCode);

            if (e.KeyboardState != KeyboardState.KeyDown) return;
            
            Debug.WriteLine("Print Screen");
            e.Handled = true;
        }

        public void Dispose()
        {
            _globalKeyboardHook?.Dispose();
        }
    }
}