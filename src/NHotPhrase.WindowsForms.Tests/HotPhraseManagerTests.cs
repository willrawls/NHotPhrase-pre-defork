using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class HotPhraseManagerTests
    {
        [TestMethod]
        public void RightControl3TimesInARow()
        {
            var keys = new[] {Keys.ControlKey, Keys.ControlKey, Keys.ControlKey};

            void hotPhraseEventArgs(object? _, HotPhraseEventArgs e) => e.Handled = true;
            void hotGlobalKeyboardHookEventArgs(object? _, GlobalKeyboardHookEventArgs e) => e.Handled = true;

            var hotPhrase = new HotPhraseKeySequence("RightControl3TimesInARow", keys, hotPhraseEventArgs);
            var hotPhraseManager = new HotPhraseManager(hotGlobalKeyboardHookEventArgs);
            hotPhraseManager.AddOrReplace(hotPhrase);

            var callCount = 0;
            hotPhraseManager.CallEachTimeKeyIsPressed((_, args) =>
            {
                Assert.AreEqual(Keys.RControlKey, args.KeyboardData.Key);
                ++callCount;
                args.Handled = true;
            });

            var lowLevelKeyboardInputEvent = new LowLevelKeyboardInputEvent
            {
                AdditionalInformation = IntPtr.Zero,
                HardwareScanCode = (int) Keys.RControlKey,
                Flags = 0,
                TimeStamp = 0,
                VirtualCode = (int) Keys.RControlKey
            };
            var keyboardState = KeyboardState.KeyUp;
            var eventArguments = new GlobalKeyboardHookEventArgs(lowLevelKeyboardInputEvent, keyboardState);

            hotPhraseManager.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            hotPhraseManager.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            hotPhraseManager.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);

            Assert.AreEqual(3, callCount);
        }

        [TestMethod]
        public void RightControl3TimesInARow_Dissected()
        {
            var keys = new[] {Keys.ControlKey, Keys.ControlKey, Keys.ControlKey};
            var hotPhrase = new HotPhraseKeySequence("RightControlRightControlRightControl",
                keys, (_, e) => e.Handled = true);

            hotPhraseManager.AddOrReplace(hotPhrase);
            
            Assert.IsNotNull(hotPhraseManager.Triggers[0].Sequence);
            Assert.AreEqual(3, hotPhraseManager.Triggers[0].Sequence.Count);
            Assert.AreEqual(Keys.ControlKey, hotPhraseManager.Triggers[0].Sequence[0]);
            Assert.AreEqual(Keys.ControlKey, hotPhraseManager.Triggers[0].Sequence[1]);
            Assert.AreEqual(Keys.ControlKey, hotPhraseManager.Triggers[0].Sequence[2]);
            
            Assert.IsNotNull(hotPhraseManager.Triggers[0].Actions[0].Handler);
            var hotPhraseEventArgs = new HotPhraseEventArgs("Frank");
            hotPhraseManager.Triggers[0].Actions[0].Handler(null, hotPhraseEventArgs);
            Assert.IsTrue(hotPhraseEventArgs.Handled);
        }
    }
}