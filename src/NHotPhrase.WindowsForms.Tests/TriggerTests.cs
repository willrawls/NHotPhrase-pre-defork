using System;
using System.Collections.Generic;
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
    }
}
