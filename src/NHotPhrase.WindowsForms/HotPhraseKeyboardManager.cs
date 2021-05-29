using System;
using System.Linq;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms
{
    public class HotPhraseKeyboardManager : IDisposable
    {
        public GlobalKeyboardHook Hook { get; set; }
        public TriggerList Triggers { get; set; } = new();

        public HotPhraseKeyboardManager()
        {
        }

        public HotPhraseKeyboardManager CallThisEachTimeAKeyIsPressed(
            EventHandler<GlobalKeyboardHookEventArgs> keyEventHandler)
        {
            if (keyEventHandler == null)
                throw new ArgumentNullException(nameof(keyEventHandler));

            Hook.KeyboardPressedEvent += keyEventHandler;
            return this;
        }

        public HotPhraseKeyboardManager AddOrReplace(string name, Keys[] keys,
            EventHandler<HotPhraseEventArgs> hotPhraseEventArgs)
        {
            return AddOrReplace(new HotPhraseKeySequence(name, keys, hotPhraseEventArgs));
        }

        public HotPhraseKeyboardManager AddOrReplace(HotPhraseKeySequence hotPhraseKeySequence)
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

        public static HotPhraseKeyboardManager Factory(
            EventHandler<GlobalKeyboardHookEventArgs> onManagerKeyboardPressEvent)
        {
            if (onManagerKeyboardPressEvent == null)
                throw new ArgumentNullException(nameof(onManagerKeyboardPressEvent));

            var manager = new HotPhraseKeyboardManager
            {
                Hook = new GlobalKeyboardHook(onManagerKeyboardPressEvent)
            };
            return manager;
        }
    }
}