using System;
using System.Diagnostics;

namespace NHotPhrase.Keyboard
{
    public class HookSampleUsage : IDisposable
    {
        public GlobalKeyboardHook _globalKeyboardHook;

        public void SetupKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        public void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            Debug.WriteLine(e.KeyboardData.VirtualCode);

            /*if (e.KeyboardData.VirtualCode != GlobalKeyboardHook.LlkhfAltdown)
                return;

            // seems, not needed in the life.
            //if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown &&
            //    e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown)
            //{
            //    MessageBox.Show("Alt + Print Screen");
            //    e.Handled = true;
            //}
            //else*/

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