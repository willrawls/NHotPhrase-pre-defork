using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NHotPhrase.Keyboard
{
    public class PhraseAction
    {
        public PhraseActions Parent;
        public EventHandler<HotPhraseEventArgs> Handler;
        public List<Keys> KeysToSend;
        public int MillisecondPauseBetweenKeys = 2;

        public PhraseAction(PhraseActions parent)
        {
            Parent = parent;
        }

        public PhraseActions ThenCall(EventHandler<HotPhraseEventArgs> handler)
        {
            Handler = handler;
            return Parent;
        }

        public PhraseActions RunNow(PhraseActionRunState phraseActionRunState)
        {
            if (Handler != null)
            {
                var hotPhraseEventArgs = new HotPhraseEventArgs("Fred");
                Handler(this, hotPhraseEventArgs);
            }

            if (KeysToSend is {Count: > 0})
                foreach (var key in KeysToSend)
                    SendKeys.SendWait(SendKeysKeyword.KeyToSendKey(key));

            return Parent;
        }
    }
}