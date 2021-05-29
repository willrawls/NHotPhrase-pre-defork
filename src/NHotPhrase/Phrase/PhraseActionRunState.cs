namespace NHotPhrase.Phrase
{
    public class PhraseActionRunState
    {
        public HotPhraseKeySequence HotPhraseKeySequence { get; set; }

        public PhraseActionRunState(HotPhraseKeySequence hotPhraseKeySequence)
        {
            HotPhraseKeySequence = hotPhraseKeySequence;
        }
    }
}