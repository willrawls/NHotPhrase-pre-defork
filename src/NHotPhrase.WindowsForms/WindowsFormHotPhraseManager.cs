using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms
{
    public class HotPhraseManager : IDisposable
    {
        public Form Parent { get; set; }
        public HotPhraseKeyboardManager Watcher { get; set; }
        public KeyPressHistory History { get; set; } = new();

        public HotPhraseManager(Form parent)
        {
            Parent = parent;
            Watcher = HotPhraseKeyboardManager.Factory(OnManagerKeyboardPressEvent);
        }

        public void OnManagerKeyboardPressEvent(object? sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == KeyboardState.KeyUp)
            {
                History.Add(e.KeyboardData.Key);
                var trigger = Watcher.Triggers.FirstMatch(History);
                if (trigger == null)
                    return;

                History.History.
                trigger.Run();
            }
        }

        public void Dispose()
        {
            Watcher?.Dispose();
        }
    }

}
