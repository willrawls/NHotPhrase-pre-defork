using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class WildcardTriggerTests
    {
        [TestMethod]
        public void ShiftShiftAnySingleDigit_IsAMatch_True()
        {
            var data = new HotPhraseKeySequence("Fred",
                    new[] {Keys.Shift, Keys.Shift}, (sender, args) => args.Handled = true)
                .FollowedByWildcards(WildcardMatchType.Digits, 1);

            var history = new List<Keys>
            {
                Keys.LShiftKey,
                Keys.RShiftKey,
                Keys.D1,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone, out var matchResult);

            Assert.IsTrue(actual);
            Assert.IsNotNull(matchResult);
            Assert.AreEqual(1, matchResult.ValueAsInt());
        }
        
    }
}