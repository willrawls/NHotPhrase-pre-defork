using System;
using System.Windows.Forms;

namespace NHotPhrase.WindowsForms
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design",
        "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "This is a singleton; disposing it would break it")]
    public class HotPhraseManager : HotkeyManagerBase
    {
        #region Singleton implementation

        public static HotPhraseManager Current => LazyInitializer.Instance;

        public static class LazyInitializer
        {
            static LazyInitializer() { }
            public static readonly HotPhraseManager Instance = new HotPhraseManager();
        }

        #endregion

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        public readonly PhraseMessageWindow PhraseMessageWindow;

        public HotPhraseManager()
        {
            PhraseMessageWindow = new PhraseMessageWindow(this);
            SetHwnd(PhraseMessageWindow.Handle);
        }

        public void AddOrReplace(string name, Keys keys, bool noRepeat, EventHandler<HotkeyEventArgs> handler)
        {
            var flags = GetFlags(keys, noRepeat);
            var vk = unchecked((uint)(keys & ~Keys.Modifiers));
            AddOrReplace(name, vk, flags, handler);
        }

        public void AddOrReplace(string name, Keys keys, EventHandler<HotkeyEventArgs> handler)
        {
            AddOrReplace(name, keys, false, handler);
        }

        public static HotPhraseFlags GetFlags(Keys hotkey, bool noRepeat)
        {
            var noMod = hotkey & ~Keys.Modifiers;
            var flags = HotPhraseFlags.None;
            if (hotkey.HasFlag(Keys.Alt))
                flags |= HotPhraseFlags.Alt;
            if (hotkey.HasFlag(Keys.Control))
                flags |= HotPhraseFlags.Control;
            if (hotkey.HasFlag(Keys.Shift))
                flags |= HotPhraseFlags.Shift;
            if (noMod == Keys.LWin || noMod == Keys.RWin)
                flags |= HotPhraseFlags.Windows;
            if (noRepeat)
                flags |= HotPhraseFlags.NoRepeat;
            return flags;
        }
    }
}
