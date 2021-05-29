using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class TriggerTests
    {
        public static Keys[] RControl3Times = new[] {Keys.ControlKey, Keys.ControlKey, Keys.ControlKey};

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
            var keyPressHistoryClone = new KeyPressHistory(8, 8, DateTime.Now, history);
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
            var keyPressHistoryClone = new KeyPressHistory(8, 8, DateTime.Now, history);
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
            var keyPressHistoryClone = new KeyPressHistory(8, 8, DateTime.Now, history);
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
            var keyPressHistoryClone = new KeyPressHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone);

            Assert.IsFalse(actual);
        }
    }
}
