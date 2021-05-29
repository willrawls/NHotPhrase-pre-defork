using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms
{
    public class WindowsFormHotPhraseManager : IDisposable
    {
        public Form Parent { get; set; }
        public HotPhraseKeyboardManager Current { get; set; }

        public WindowsFormHotPhraseManager(Form parent)
        {
            Parent = parent;
            Current = HotPhraseKeyboardManager.Factory(OnManagerKeyboardPressEvent);
        }

        public void OnManagerKeyboardPressEvent(object? sender, GlobalKeyboardHookEventArgs e)
        {
            
        }

        public void Dispose()
        {
            Current?.Dispose();
        }
    }

}
