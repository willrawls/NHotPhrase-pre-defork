using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace NHotPhrase.Keyboard
{
    public class HotPhraseKeySequence
    {
        public List<Keys> Sequence = new();
        public string Name { get; set; }
        public PhraseActions Actions { get; set; } = new();

        public HotPhraseKeySequence(string name, Keys[] keys, EventHandler<HotPhraseEventArgs> hotPhraseEventArgs)
        {
            Name = name;
            Sequence.AddRange(keys);
            ThenCall(hotPhraseEventArgs);
        }

        public HotPhraseKeySequence()
        {
        }

        public static HotPhraseKeySequence Named(string name)
        {
            return new()
            {
                Name = name
            };
        }

        public HotPhraseKeySequence WhenKeyRepeats(Keys repeatKey, int repeatCount)
        {
            for (var i = 0; i < repeatCount; i++) Sequence.Add(repeatKey);
            return this;
        }

        public HotPhraseKeySequence WhenKeyReleased(Keys key)
        {
            Sequence.Add(key);
            return this;
        }

        public HotPhraseKeySequence WhenKeysReleased(IList<Keys> keys)
        {
            Sequence.AddRange(keys);
            return this;
        }

        public void OnPhrase(object sender, GlobalPhraseHookEventArgs e)
        {
            Debug.WriteLine(e.Target.Name);
            e.Handled = true;
        }

        public bool IsAMatch(KeyPressHistory keyPressHistoryClone)
        {
            if (keyPressHistoryClone.History.Count < Sequence.Count)
                return false;

            var possibleMatchRange =
                keyPressHistoryClone.History.GetRange(keyPressHistoryClone.History.Count - Sequence.Count,
                    Sequence.Count);

            for (var i = 0; i < Sequence.Count; i++)
                if(!SendKeysKeyword.IsAMatch(Sequence[i], possibleMatchRange[i]))
                    return false;

            return true;
        }

        public HotPhraseKeySequence ThenCall(EventHandler<HotPhraseEventArgs> handler)
        {
            var sequence = new PhraseAction(this, handler);
            Actions.Add(sequence);
            return this;
        }

        public HotPhraseKeySequence WhenKeyPressed(Keys key)
        {
            Sequence.Clear();
            Sequence.Add(key);
            return this;
        }

        public HotPhraseKeySequence ThenKeyPressed(Keys key)
        {
            Sequence.Add(key);
            return this;
        }
    }
}