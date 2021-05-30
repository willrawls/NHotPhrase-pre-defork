using System;
using System.Diagnostics;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms
{
    public class HotPhraseManager : IDisposable
    {
        public Form Parent { get; set; }
        public KeyboardManager Keyboard { get; set; }
        public KeyHistory History { get; set; } = new();

        public HotPhraseManager(Form parent)
        {
            Parent = parent;
            Keyboard = KeyboardManager.Factory(OnManagerKeyboardPressEvent);
        }

        public void OnManagerKeyboardPressEvent(object? sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == KeyboardState.KeyUp)
            {
                Debug.WriteLine($"Key {e.KeyboardData.Key}");
                History.AddKeyPress(e.KeyboardData.Key);
                var trigger = Keyboard.Triggers.FirstMatch(History);
                if (trigger == null)
                    return;

                Debug.WriteLine($"Trigger {trigger.Name}");

                History.Clear();
                trigger.Run();
            }
        }

        public void Dispose()
        {
            Keyboard?.Dispose();
        }
    }

}
