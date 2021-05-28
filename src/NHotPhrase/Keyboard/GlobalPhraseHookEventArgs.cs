using System.ComponentModel;

namespace NHotPhrase.Keyboard
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