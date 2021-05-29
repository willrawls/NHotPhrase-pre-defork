using System.Collections.Generic;
using System.Linq;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms
{
    public class TriggerList : List<HotPhraseKeySequence>
    {
        public static readonly object SyncRoot = new();
        public HotPhraseKeySequence FirstMatch(KeyPressHistory history)
        {
            lock (SyncRoot)
            {
                var cloneOfHistory = history.Clone();
                return this.FirstOrDefault(trigger => trigger.IsAMatch(cloneOfHistory));
            }
        }
    }
}