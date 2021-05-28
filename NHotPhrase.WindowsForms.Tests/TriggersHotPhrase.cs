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
            var hotPhrase = new HotPhraseSequence
            {
                Name = "RightControlRightControlRightControl",
                HotPhraseKeySequence = HotPhraseKeySequence.Named().WhenKeyRepeats(Keys.RControlKey, 3),
                Handler = new EventHandler<HotPhraseEventArgs>((sender, e) => e.Handled = true),
            };

            HotPhraseManager.Current.AddOrReplace(hotPhrase);
        }

        public void AddOrReplace(string name, Keys keys, bool noRepeat, EventHandler<HotPhraseEventArgs> handler)
        {
            var flags = GetFlags(keys, noRepeat);
            var vk = unchecked((uint)(keys & ~Keys.Modifiers));
            AddOrReplace(name, vk, flags, handler);
        }

    }
}
