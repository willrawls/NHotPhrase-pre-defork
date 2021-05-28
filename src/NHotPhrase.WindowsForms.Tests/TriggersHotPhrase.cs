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


        [TestMethod]
        public void RightControl3TimesInARow()
        {
            var callCount = 0;
            var hotPhrase = new HotPhraseKeySequence("RightControlRightControlRightControl",
                RControl3Times, (_, e) =>
                {
                    e.Handled = true;
                    callCount++;
                });
            HotPhraseManager.Current.AddOrReplace(hotPhrase);

            HotPhraseManager.Current.CallEachTimeKeyIsPressed((sender, args) =>
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

            HotPhraseManager.Current.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            HotPhraseManager.Current.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            HotPhraseManager.Current.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);

            Assert.AreEqual(3, callCount);
        }

    }

    [TestClass]
    public class HotPhraseManagerTests
    {
        [TestMethod]
        public void RightControl3TimesInARow()
        {
            var keys = new[] {Keys.ControlKey, Keys.ControlKey, Keys.ControlKey};
            var hotPhrase = new HotPhraseKeySequence("RightControlRightControlRightControl",
                keys, (_, e) => e.Handled = true);
            HotPhraseManager.Current.AddOrReplace(hotPhrase);

            var callCount = 0;
            HotPhraseManager.Current.CallEachTimeKeyIsPressed((sender, args) =>
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

            HotPhraseManager.Current.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            HotPhraseManager.Current.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            HotPhraseManager.Current.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);

            Assert.AreEqual(3, callCount);
        }

        [TestMethod]
        public void RightControl3TimesInARow_Dissected()
        {
            var keys = new[] {Keys.ControlKey, Keys.ControlKey, Keys.ControlKey};
            var hotPhrase = new HotPhraseKeySequence("RightControlRightControlRightControl",
                keys, (_, e) => e.Handled = true);

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
