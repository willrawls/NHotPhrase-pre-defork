using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
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

        public bool Run()
        {
            var state = new PhraseActionRunState(this);
            foreach (var action in Actions)
            {
                if (!action.RunNow(state))
                    return false;
            }
            return true;
        }

        public bool IsAMatch(List<Keys> keyList)
        {
            if (keyList.Count < Sequence.Count)
                return false;

            var possibleMatchRange = keyList.Count == Sequence.Count
                ? keyList
                : keyList.GetRange(keyList.Count - Sequence.Count, Sequence.Count);

            for (var i = 0; i < Sequence.Count; i++)
            {
                if (!SendKeysKeyword.IsAMatch(Sequence[i], possibleMatchRange[i]))
                    return false;
            }

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