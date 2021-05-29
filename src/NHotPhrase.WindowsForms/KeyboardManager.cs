using System;
using System.Linq;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms
{
    public class KeyboardManager : IDisposable
    {
        public GlobalKeyboardHook Hook { get; set; }
        public TriggerList Triggers { get; set; } = new();

        public KeyboardManager()
        {
        }

        public KeyboardManager CallThisEachTimeAKeyIsPressed(
            EventHandler<GlobalKeyboardHookEventArgs> keyEventHandler)
        {
            if (keyEventHandler == null)
                throw new ArgumentNullException(nameof(keyEventHandler));

            Hook.KeyboardPressedEvent += keyEventHandler;
            return this;
        }

        public KeyboardManager AddOrReplace(string name, Keys[] keys,
            EventHandler<HotPhraseEventArgs> hotPhraseEventArgs)
        {
            return AddOrReplace(new HotPhraseKeySequence(name, keys, hotPhraseEventArgs));
        }

        public KeyboardManager AddOrReplace(HotPhraseKeySequence hotPhraseKeySequence)
        {
            var existingPhraseKeySequence = Triggers
                .FirstOrDefault(x => x.Name
                    .Equals(hotPhraseKeySequence.Name,
                        StringComparison.InvariantCultureIgnoreCase));

            if (existingPhraseKeySequence != null)
                Triggers.Remove(existingPhraseKeySequence);
            Triggers.Add(hotPhraseKeySequence);
            return this;
        }

        public void Dispose()
        {
            Hook?.Dispose();
        }

        public static KeyboardManager Factory(
            EventHandler<GlobalKeyboardHookEventArgs> onManagerKeyboardPressEvent)
        {
            if (onManagerKeyboardPressEvent == null)
                throw new ArgumentNullException(nameof(onManagerKeyboardPressEvent));

            var manager = new KeyboardManager
            {
                Hook = new GlobalKeyboardHook(onManagerKeyboardPressEvent)
            };
            return manager;
        }
    }
}