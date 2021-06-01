using System;
using System.Diagnostics;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

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
                MatchResult matchResult;
                var trigger = Keyboard.Triggers.FirstMatch(History, out matchResult);
                if (trigger == null)
                    return;

                Debug.WriteLine($"Trigger {trigger.Name}");

                if(!string.IsNullOrEmpty(matchResult.Value))
                    Debug.WriteLine($"  Wilds {matchResult.Value}");

                History.Clear();
                trigger.Run(matchResult);
            }
        }

        public void Dispose()
        {
            Keyboard?.Dispose();
        }
    }

}
