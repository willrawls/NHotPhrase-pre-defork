using System.Collections.Generic;
using System.Linq;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms
{
    public class TriggerList : List<HotPhraseKeySequence>
    {
        public static readonly object SyncRoot = new();
        public HotPhraseKeySequence FirstMatch(KeyHistory history)
        {
            lock (SyncRoot)
            {
                var cloneOfHistory = history.KeyList();
                return this.FirstOrDefault(trigger => trigger.IsAMatch(cloneOfHistory));
            }
        }
    }
}