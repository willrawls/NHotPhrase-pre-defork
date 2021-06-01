namespace NHotPhrase.Phrase
{
    public class PhraseActionRunState
    {
        public HotPhraseKeySequence HotPhraseKeySequence { get; set; }
        public MatchResult MatchResult { get; set; }

        public PhraseActionRunState(HotPhraseKeySequence hotPhraseKeySequence, MatchResult matchResult)
        {
            HotPhraseKeySequence = hotPhraseKeySequence;
            MatchResult = matchResult;
        }
    }
}