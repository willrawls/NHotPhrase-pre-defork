using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class TriggersHotPhrase
    {
        [TestMethod]
        public void RightControlRightControlRightControl()
        {
            var keys = new Keys[] {Keys.ControlKey, Keys.ControlKey, Keys.ControlKey};
            var hotPhrase = new HotPhraseKeySequence(
                "RightControlRightControlRightControl",
                keys,
                (sender, e) => e.Handled = true);


            HotPhraseManager.Current.AddOrReplace(hotPhrase);
            
            Assert.IsNotNull(HotPhraseManager.Current.Triggers[0].Sequence);
            Assert.AreEqual(3, HotPhraseManager.Current.Triggers[0].Sequence.Count);
            Assert.AreEqual(Keys.ControlKey, HotPhraseManager.Current.Triggers[0].Sequence[0]);
            Assert.AreEqual(Keys.ControlKey, HotPhraseManager.Current.Triggers[0].Sequence[1]);
            Assert.AreEqual(Keys.ControlKey, HotPhraseManager.Current.Triggers[0].Sequence[2]);
            
            Assert.IsNotNull(HotPhraseManager.Current.Triggers[0].Actions[0].Handler);
            var hotPhraseEventArgs = new HotPhraseEventArgs("Frank");
            HotPhraseManager.Current.Triggers[0].Actions[0].Handler(null, hotPhraseEventArgs);
            Assert.IsTrue(hotPhraseEventArgs.Handled);
        }
    }
}
