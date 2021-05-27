using System;

namespace NHotPhrase.Keyboard
{
    public class HotPhraseSequence
    {
        public string Name { get; set; }
        public KeySequence KeySequence { get; set; }
        public EventHandler<HotPhraseEventArgs> Handler { get; set; }
    }
}