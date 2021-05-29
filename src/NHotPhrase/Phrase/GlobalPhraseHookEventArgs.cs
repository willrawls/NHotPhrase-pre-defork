using System.ComponentModel;

namespace NHotPhrase.Phrase
{
    public class GlobalPhraseHookEventArgs : HandledEventArgs
    {
        public HotPhraseKeySequence Target { get; set; }

        public GlobalPhraseHookEventArgs(HotPhraseKeySequence target)
        {
            target = Target;
        }
    }
}