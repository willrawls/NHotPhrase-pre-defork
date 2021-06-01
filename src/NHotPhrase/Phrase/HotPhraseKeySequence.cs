using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public class HotPhraseKeySequence
    {
        public string Name { get; set; }

        public List<Keys> Sequence = new();
        
        public WildcardMatchType WildcardMatchType { get; set; }
        public int WildcardCount { get; set; }
        
        public PhraseActions Actions { get; set; } = new();


        public HotPhraseKeySequence(string name, Keys[] keys, EventHandler<HotPhraseEventArgs> hotPhraseEventArgs)
        {
            Name = name;
            Sequence.AddRange(keys);
            ThenCall(hotPhraseEventArgs);
        }

        public HotPhraseKeySequence()
        {
        }

        public static HotPhraseKeySequence Named(string name)
        {
            return new()
            {
                Name = name
            };
        }

        public HotPhraseKeySequence WhenKeyRepeats(Keys repeatKey, int repeatCount)
        {
            for (var i = 0; i < repeatCount; i++) Sequence.Add(repeatKey);
            return this;
        }

        public HotPhraseKeySequence WhenKeyReleased(Keys key)
        {
            Sequence.Add(key);
            return this;
        }

        public HotPhraseKeySequence WhenKeysReleased(IList<Keys> keys)
        {
            Sequence.AddRange(keys);
            return this;
        }

        public bool Run(MatchResult matchResult)
        {
            var state = new PhraseActionRunState(this, matchResult);
            foreach (var action in Actions)
            {
                if (!action.RunNow(state))
                    return false;
            }
            return true;
        }

        public bool IsAMatch(List<Keys> keyList, out MatchResult matchResult)
        {
            matchResult = null;

            var sequencePlusWildcardCount = Sequence.Count + WildcardCount;
            if (keyList.Count < sequencePlusWildcardCount)
                return false;

            var possibleMatchRange = keyList.Count == sequencePlusWildcardCount
                ? keyList
                : keyList.GetRange(keyList.Count - sequencePlusWildcardCount, sequencePlusWildcardCount);

            for (var i = 0; i < Sequence.Count; i++)
            {
                if (!SendKeysKeyword.IsAMatch(Sequence[i], possibleMatchRange[i]))
                    return false;
            }

            if (WildcardMatchType is WildcardMatchType.Unknown or WildcardMatchType.None 
                || WildcardCount < 1) 
                return true;

            var possibleWildcardRange = keyList.Count == WildcardCount
                ? keyList
                : keyList.GetRange(keyList.Count - WildcardCount, WildcardCount);

            if (possibleWildcardRange.Count != WildcardCount)
                return true;

            switch (WildcardMatchType)
            {
                case WildcardMatchType.Digits:
                    if (possibleWildcardRange.OnlyDigits())
                    {
                        matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.Letters:
                    if (possibleWildcardRange.OnlyLetters())
                    {
                        matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.AlphaNumeric:
                    if (possibleWildcardRange.OnlyLetters()
                        || possibleWildcardRange.OnlyDigits()
                    )
                    {
                        matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.NotAlphaNumeric:
                    if (!possibleWildcardRange.OnlyLetters() 
                        && possibleWildcardRange.OnlyDigits()
                    )
                    {
                        matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.Anything:
                    matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    break;
            }

            return true;
        }

        public HotPhraseKeySequence ThenCall(EventHandler<HotPhraseEventArgs> handler)
        {
            var sequence = new PhraseAction(this, handler);
            Actions.Add(sequence);
            return this;
        }

        public HotPhraseKeySequence WhenKeyPressed(Keys key)
        {
            Sequence.Clear();
            Sequence.Add(key);
            return this;
        }

        public HotPhraseKeySequence WhenKeysPressed(params Keys[] keys)
        {
            Sequence.Clear();
            Sequence.AddRange(keys);
            return this;
        }

        public HotPhraseKeySequence ThenKeyPressed(Keys key)
        {
            Sequence.Add(key);
            return this;
        }

        public HotPhraseKeySequence FollowedByWildcards(WildcardMatchType wildcardMatchType, int wildcardCount)
        {
            WildcardMatchType = wildcardMatchType;
            WildcardCount = wildcardCount;
            return this;
        }
    }
}