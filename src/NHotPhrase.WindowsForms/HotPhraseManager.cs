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

        public HotPhraseManager(EventHandler<GlobalKeyboardHookEventArgs> keyboardPressedEvent)
        {
            if (keyboardPressedEvent == null)
                throw new ArgumentNullException(nameof(keyboardPressedEvent));
            
            Hook = new GlobalKeyboardHook(keyboardPressedEvent);
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

        public void Dispose()
        {
            Hook?.Dispose();
        }
    }
}
