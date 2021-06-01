namespace NHotPhrase.Phrase
{
    public class MatchResult
    {
        public HotPhraseKeySequence HotPhraseKeySequence { get; set; }
        public string Value { get; set; }

        public MatchResult(HotPhraseKeySequence hotPhraseKeySequence, string value)
        {
            HotPhraseKeySequence = hotPhraseKeySequence;
            Value = value;
        }

        public int ValueAsInt() =>
            int.TryParse(Value, out var valueAsInt) 
                ? valueAsInt 
                : 0;
    }
}