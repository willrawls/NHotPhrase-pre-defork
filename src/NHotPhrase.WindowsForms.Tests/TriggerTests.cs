using System;
using System.Collections.Generic;
using System.Linq;
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
        public void VariousSequences_IsAMatch_True(Keys[] simulatedHistory, Keys[] sequence)
        {
            for (var i = 0; i < 100; i++)
            {
                var simulatedHistoryList = simulatedHistory.ToList();
                var sequenceList = sequence.ToList();

                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, true);

                simulatedHistoryList.Insert(0, RandomKey());
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, true);
            
                simulatedHistoryList.Add(RandomKey());
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, false);
            }
        }

        [DataTestMethod]
        [DataRow(new[]{ Keys.A }, new[]{ Keys.B })]
        public void VariousSequences_IsAMatch_False(Keys[] simulatedHistory, Keys[] sequence)
        {
            for (var i = 0; i < 100; i++)
            {
                var simulatedHistoryList = simulatedHistory.ToList();
                var sequenceList = sequence.ToList();

                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, false);
            
                simulatedHistoryList.Insert(0, RandomKey());
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, false);
            
                simulatedHistoryList.Add(RandomKey());
                VariousSequences_PokingAround(simulatedHistoryList, sequenceList, false);
            }
        }

        public static void VariousSequences_PokingAround(List<Keys> simulatedHistory, List<Keys> sequence, bool expected)
        {
            var hotPhraseKeySequence = new HotPhraseKeySequence("Fred", sequence.ToArray(), (sender, args) => args.Handled = true);
            var keyHistory = new KeyHistory(8, 8, DateTime.Now, simulatedHistory.ToList());
            var actual = hotPhraseKeySequence.IsAMatch(keyHistory);
            var debugText = $"\n    Sequence: {KeyListToString(sequence)}\n     History: {KeyListToString(simulatedHistory)}";
            Assert.AreEqual(expected, actual, debugText);
        }

        private static string KeyListToString(List<Keys> list)
        {
            return list.Aggregate("", (current, item) => 
                current + (" " + item));
        }

        public static Random Random = new Random();
        public static Keys RandomKey()
        {
            return (Keys) Random.Next(50, 150);
        }
    }
}
