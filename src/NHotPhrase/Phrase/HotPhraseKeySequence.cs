using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool Run()
        {
            var state = new PhraseActionRunState(this);
            foreach (var action in Actions)
            {
                if (!action.RunNow(state))
                    return false;
            }
            return true;
        }

        public bool IsAMatch(List<Keys> keyList, out int wildcardNumber, int defaultNumber = 0)
        {
            var result = IsAMatch(keyList, out string wildcardString);
            if (!result)
            {
                wildcardNumber = defaultNumber;
            }
            else if(!int.TryParse(wildcardString, out int wildcardNumberAttempt))
            {
                wildcardNumber = defaultNumber;
            }
            else
            {
                wildcardNumber = wildcardNumberAttempt;
            }
            return result;
        }

        public bool IsAMatch(List<Keys> keyList)
        {
            wildcards = null;
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

            if (WildcardCount <= 0) return true;

            var possibleWildcardRange = keyList.Count == sequencePlusWildcardCount
                ? keyList
                : keyList.GetRange(keyList.Count - WildcardCount, WildcardCount);

            if (possibleWildcardRange.Count != WildcardCount)
                return true;

            switch (WildcardMatchType)
            {
                case WildcardMatchType.Digits:
                    if (possibleWildcardRange.OnlyDigits())
                    {
                        WildcardString = possibleWildcardRange.ToString();
                        WildcardDigits = int.Parse(WildcardString);
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.Letters:
                    if (possibleWildcardRange.OnlyLetters())
                    {
                        WildcardString = possibleWildcardRange.ToString();
                        WildcardDigits = 0;
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
                        WildcardString = possibleWildcardRange.ToString();
                        WildcardDigits = 0;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.Symbols:
                //    break;
                case WildcardMatchType.Anything:
                    WildcardString = possibleWildcardRange.ToString();
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