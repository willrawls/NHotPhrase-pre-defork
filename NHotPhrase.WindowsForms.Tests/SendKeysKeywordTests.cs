using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class SendKeysKeywordTests
    {
        [TestMethod]
        public void IsExacting_Exacting()
        {
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.LShiftKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.RShiftKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.RMenu));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.LControlKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.NumPad5));

            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.A));
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.Enter));
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.D0));
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.Home));
        }


        [TestMethod]
        public void IsAMatch_Similar()
        {
            Assert.IsTrue(SendKeysKeyword.IsAMatch())
        }
    }
}