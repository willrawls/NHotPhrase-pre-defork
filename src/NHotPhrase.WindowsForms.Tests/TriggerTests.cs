using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class TriggerTests
    {
        public static Keys[] RControl3Times = new[] {Keys.RControlKey, Keys.RControlKey, Keys.RControlKey};
        public static Keys[] Shift3Times = new[] {Keys.Shift, Keys.Shift, Keys.Shift};

        [TestMethod]
        public void RControl3Times_IsAMatch_True()
        {
            var callCount = 0;
            var data = new HotPhraseKeySequence("Fred", RControl3Times, (sender, args) =>
            {
                callCount++;
                args.Handled = true;
            });

            var history = new List<Keys>
            {
                Keys.RControlKey,
                Keys.RControlKey,
                Keys.RControlKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void ShiftShiftAnySingleDigit_IsAMatch_True()
        {
            var data = new HotPhraseKeySequence("Fred",new[] {Keys.Shift, Keys.Shift}, (sender, args) => args.Handled = true)
                .FollowedByWildcards(Wildcard.Digits, 1);

            var history = new List<Keys>
            {
                Keys.LShiftKey,
                Keys.RShiftKey,
                Keys.D0,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Shift3Times3Times_IsAMatch_True()
        {
            var data = new HotPhraseKeySequence("Fred", Shift3Times, (sender, args) => args.Handled = true);

            var history = new List<Keys>
            {
                Keys.LShiftKey,
                Keys.RShiftKey,
                Keys.ShiftKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void RControl3Times_IsAMatch_True_WhenMoreThan3EntriesInHistory()
        {
            var callCount = 0;
            var data = new HotPhraseKeySequence("Fred", RControl3Times, (sender, args) =>
            {
                callCount++;
                args.Handled = true;
            });

            var history = new List<Keys>
            {
                Keys.RControlKey,
                Keys.Enter,
                Keys.RControlKey,
                Keys.RControlKey,
                Keys.RControlKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void RControl3Times_IsAMatch_False_Simple()
        {
            var callCount = 0;
            var data = new HotPhraseKeySequence("Fred", RControl3Times, (sender, args) =>
            {
                callCount++;
                args.Handled = true;
            });

            var history = new List<Keys>
            {
                Keys.RControlKey,
                Keys.A,
                Keys.RControlKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void RControl3Times_IsAMatch_False_NotEnoughKeys()
        {
            var callCount = 0;
            var data = new HotPhraseKeySequence("Fred", RControl3Times, (sender, args) =>
            {
                callCount++;
                args.Handled = true;
            });

            var history = new List<Keys>
            {
                Keys.RControlKey,
                Keys.RControlKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone);

            Assert.IsFalse(actual);
        }

        [DataTestMethod]
        [DataRow(new[]{ Keys.A }, new[]{ Keys.A })]
        [DataRow(new[]{ Keys.A, Keys.A }, new[]{ Keys.A, Keys.B, Keys.A, Keys.A })]
        [DataRow(new[]{ Keys.CapsLock, Keys.Control }, new[]{ Keys.A, Keys.B, Keys.CapsLock, Keys.RControlKey })]
        [DataRow(new[]{ Keys.Control, Keys.LControlKey }, new[]{ Keys.A, Keys.RControlKey, Keys.LControlKey })]
        [DataRow(new[]{ Keys.RControlKey, Keys.RControlKey }, new[]{ Keys.RControlKey, Keys.B, Keys.RControlKey, Keys.RControlKey })]
        [DataRow(new[]{ Keys.RControlKey, Keys.Control }, new[]{ Keys.RControlKey, Keys.B, Keys.RControlKey, Keys.LControlKey })]

        [DataRow(new[]{ Keys.RControlKey, Keys.RControlKey, Keys.RControlKey }, new[]{ Keys.RControlKey, Keys.RControlKey, Keys.RControlKey })]
        [DataRow(new[]{ Keys.Control, Keys.Shift, Keys.Alt}, new[]{ Keys.LControlKey, Keys.RShiftKey, Keys.LMenu })]
        
        [DataRow(new[]{ Keys.CapsLock, Keys.Capital, Keys.D, Keys.Back }, new[]{ Keys.RControlKey, Keys.Capital, Keys.Capital, Keys.D, Keys.Back })]
        [DataRow(new[]{ Keys.CapsLock, Keys.CapsLock, Keys.D, Keys.Back }, new[]{ Keys.RControlKey, Keys.CapsLock, Keys.CapsLock, Keys.D, Keys.Back })]
        
        public void VariousSequences_IsAMatch_True(Keys[] hotPhraseSequence, Keys[] userTyped)
        {
            for (var i = 0; i < 100; i++)
            {
                var simulatedHistoryList = userTyped.ToList();
                var sequenceList = hotPhraseSequence.ToList();

                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, true);

                simulatedHistoryList.Insert(0, RandomKey(hotPhraseSequence));
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, true);
            
                simulatedHistoryList.Add(RandomKey(hotPhraseSequence));
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, false);
            }
        }

        [DataTestMethod]
        [DataRow(new[]{ Keys.B }, new[]{ Keys.A })]
        [DataRow(new[]{ Keys.A, Keys.A }, new[]{ Keys.A, Keys.B, Keys.A, Keys.B })]
        [DataRow(new[]{ Keys.CapsLock, Keys.Control }, new[]{ Keys.A, Keys.CapsLock, Keys.B, Keys.RControlKey })]
        [DataRow(new[]{ Keys.Control, Keys.LControlKey }, new[]{ Keys.A, Keys.RControlKey, Keys.RControlKey })]
        [DataRow(new[]{ Keys.RControlKey, Keys.RControlKey }, new[]{ Keys.RControlKey, Keys.B, Keys.LControlKey, Keys.RControlKey })]
        [DataRow(new[]{ Keys.RControlKey, Keys.Control }, new[]{ Keys.RControlKey, Keys.C })]
        public void VariousSequences_IsAMatch_False(Keys[] hotPhraseSequence, Keys[] userTyped)
        {
            for (var i = 0; i < 100; i++)
            {
                var simulatedHistoryList = userTyped.ToList();
                var sequenceList = hotPhraseSequence.ToList();

                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, false);
            
                simulatedHistoryList.Insert(0, RandomKey(hotPhraseSequence));
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, false);
            
                simulatedHistoryList.Add(RandomKey(hotPhraseSequence));
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, false);
            }
        }
        
        [TestMethod]
        public void SingleKey_IsAMatch_True()
        {
            var keysEnumValues = KeysEnumValues();
            foreach (Keys key in keysEnumValues)
            {
                var sequence = new List<Keys> {key}.ToArray();
                var simulatedHistoryList = sequence.ToList();
                var sequenceList = sequence.ToList();

                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, true);

                simulatedHistoryList.Insert(0, RandomKey(sequence));
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, true);
            
                simulatedHistoryList.Add(RandomKey(sequence));
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, false);
            }
        }

        public static void VariousSequences_PokingAround(List<Keys> simulatedHistory, List<Keys> sequence, bool expected)
        {
            var hotPhraseKeySequence = new HotPhraseKeySequence("Fred", sequence.ToArray(), (sender, args) => args.Handled = true);
            var keyHistory = new KeyHistory(8, 8, DateTime.Now, simulatedHistory.ToList());
            var actual = hotPhraseKeySequence.IsAMatch(keyHistory);
            if (actual && !expected)
            {
                // Check the last to see if it's a simplified, if so, actual == expected
                if (SendKeysKeyword.ShouldBeSimplified(sequence[0]))
                {
                    if (SendKeysKeyword.IsAMatch(sequence[0], keyHistory[^1]))
                    {
                        // It's all good
                        actual = expected;
                    }
                }
            }
            var debugText = $"\n    Sequence: {KeyListToString(sequence)}\n     History: {KeyListToString(simulatedHistory)}";
            Assert.AreEqual(expected, actual, debugText);
        }

        private static string KeyListToString(List<Keys> list)
        {
            return list.Aggregate("", (current, item) => current + (" " + item));
        }

        public static List<Keys> KeysToNotGenerateRandomly = null;
        public static Random Random = new Random();
        public static Keys RandomKey(Keys[] butNotTheseKeys)
        {
            if (KeysToNotGenerateRandomly == null)
            {
                KeysToNotGenerateRandomly = new List<Keys>();
                var keysEnumValues = KeysEnumValues();
                foreach (Keys key in keysEnumValues)
                    if(SendKeysKeyword.ShouldBeSimplified(key))
                        KeysToNotGenerateRandomly.Add(key);
            }

            var randomKey = (Keys) Random.Next(32, 165);

            while(butNotTheseKeys.Any(k => k == randomKey) 
                  || KeysToNotGenerateRandomly.Any(k => k == randomKey))
            {
                randomKey = (Keys) Random.Next(32, 165);
            }

            return randomKey;
        }

        private static List<Keys> _keysEnumValues = null;
        private static List<Keys> KeysEnumValues()
        {
            if (_keysEnumValues != null) 
                return _keysEnumValues;

            _keysEnumValues = new List<Keys>();
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if(key != Keys.None) 
                    _keysEnumValues .Add(key);
            }
            return _keysEnumValues;
        }
    }
}
