namespace NHotPhrase.Phrase
{
    public class MatchResult
    {
        public HotPhraseKeySequence Parent { get; set; }
        public WildcardMatchType MatchType { get; set; }
        public string Value { get; set; }

        public MatchResult(HotPhraseKeySequence parent, WildcardMatchType matchType, string value)
        {
            Parent = parent;
            MatchType = matchType;
            Value = value;
        }

        public int ValueAsInt() =>
            int.TryParse(Value, out var valueAsInt) 
                ? valueAsInt 
                : 0;
    }
}