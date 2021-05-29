using System;

namespace NHotPhrase.Phrase
{
    public class HotPhraseEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public PhraseAction Action { get; set; }
        public PhraseActionRunState State { get; set; }
        public string Name => State?.HotPhraseKeySequence?.Name;

        public HotPhraseEventArgs(PhraseAction action, PhraseActionRunState state)
        {
            Action = action;
            State = state;
        }
    }
}