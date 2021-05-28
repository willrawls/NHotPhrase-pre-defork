using System;

namespace NHotPhrase.Keyboard
{
    public class HotPhraseSequence
    {
        public string Name { get; set; }
        public HotPhraseKeySequence HotPhraseKeySequence { get; set; }
        public EventHandler<HotPhraseEventArgs> Handler { get; set; }
    }
}