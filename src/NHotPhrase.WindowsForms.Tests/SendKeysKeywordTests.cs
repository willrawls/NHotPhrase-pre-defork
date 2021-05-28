using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class SendKeysKeywordTests
    {
        [TestMethod]
        public void IsExacting_True()
        {
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.LShiftKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.RShiftKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.RMenu));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.LControlKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.NumPad5));
        }

        [TestMethod]
        public void IsExacting_False()
        {
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.A));
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.Enter));
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.D0));
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.Home));
        }

        [TestMethod]
        public void IsAMatch_WhenTheSame_True()
        {
            Assert.IsTrue(SendKeysKeyword.IsAMatch(Keys.A, Keys.A));
        }

        [TestMethod]
        public void IsAMatch_WhenSimilar_True()
        {
            Assert.IsTrue(SendKeysKeyword.IsAMatch(Keys.ControlKey, Keys.RControlKey));
        }

        [TestMethod]
        public void IsAMatch_WhenSimilar_False()
        {
            Assert.IsFalse(SendKeysKeyword.IsAMatch(Keys.RControlKey, Keys.ControlKey));
        }
    }
}