using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design",
        "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "This is a singleton; disposing it would break it")]
    public class HotPhraseManager : IDisposable
    {
        public GlobalKeyboardHook Hook { get; set; }
        public TriggerList Triggers { get; set; } = new();

        private static HotPhraseManager _current;
        public static HotPhraseManager Current
        {
            get => _current ??= new HotPhraseManager(DefaultDoNothingOnKeyPressed);
            set
            {
                _current?.Dispose();
                _current = value;
            }
        }

        private static void DefaultDoNothingOnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
        }

        public HotPhraseManager(EventHandler<GlobalKeyboardHookEventArgs> keyboardPressedEvent = null)
        {
            Hook = new GlobalKeyboardHook(keyboardPressedEvent ?? DefaultDoNothingOnKeyPressed);
        }

        public HotPhraseManager CallEachTimeKeyIsPressed(EventHandler<GlobalKeyboardHookEventArgs> keyEventHandler)
        {
            if (keyEventHandler == null)
                throw new ArgumentNullException(nameof(keyEventHandler));

            Hook.KeyboardPressedEvent += keyEventHandler;
            return this;
        }

        public HotPhraseManager AddOrReplace(string name, Keys[] keys, EventHandler<HotPhraseEventArgs> hotPhraseEventArgs)
        {
            return AddOrReplace(new HotPhraseKeySequence(name, keys, hotPhraseEventArgs));
        }

        public HotPhraseManager AddOrReplace(HotPhraseKeySequence hotPhraseKeySequence)
        {
            var existingPhraseKeySequence = Triggers.FirstOrDefault(x => x.Name.Equals(hotPhraseKeySequence.Name, StringComparison.InvariantCultureIgnoreCase));
            if (existingPhraseKeySequence != null)
                Triggers.Remove(existingPhraseKeySequence);
            Triggers.Add(hotPhraseKeySequence);
            return this;
        }

        public static HotPhraseManager ReinitializeCurrent()
        {
            Current.Dispose();
            Current = new HotPhraseManager(DefaultDoNothingOnKeyPressed);
            return Current;
        }

        public static void StopListening()
        {
            Current.Dispose();
            Current = null;
        }

        public void Dispose()
        {
            Hook?.Dispose();
        }
    }
}
