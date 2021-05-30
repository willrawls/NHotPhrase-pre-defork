# NHotPhrase
A managed library to handle global "hot phrases" in Windows Forms. 

A hot phrase is a key sequence that activates a piece of your code from any other windows application. 
Uses include anywhere you could usea global hot key.

So where a hotkey is something like 
- Push ctrl + alt + 1 and then release all 3 which might cause your email address to be typed into the current application

A hot phrase would be 
- First press and release the ctrl key, 
- Then press and release the alt key, 
- Then press and release the  1 key 
- Then type your email (or whatever you want) into the current application.

An example hot phrase from NHotPhrase.WindowsForms.Demo does this
- Click CapsLock, 
- Click CapsLock again, 
- Type "WRG" (or "wrg" it's not case sensitive)
- Then whatever text is in the DemoForm's text box will be typed into the current application.

The code for this might look like
>>
            // Write some text
            Manager.Keyboard.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Write some text")
                    .WhenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.W)
                    .ThenKeyPressed(Keys.R)
                    .ThenKeyPressed(Keys.G)
                    .ThenCall(OnWriteEmail)
...                    
        private void OnWriteEmail(object? sender, HotPhraseEventArgs e)
        {
            // Since the user typed WRG, let's get rid of that
            SendKeys.SendWait("{BACKSPACE}{BACKSPACE}{BACKSPACE}");
            Thread.Sleep(2);

            // Make sure what's in the text box is ready for sending through SendKeys
            var textPartsToSend = TextToSend.Text.MakeReadyForSendKeys();
            if (textPartsToSend.Count <= 0) return;

            foreach (var part in textPartsToSend)
            {
                // The text to send has been broken into strings of no longer than 8 characters or one command
                // This gives the receiving application pleaty of time to process it (but still pretty quickly)
                SendKeys.SendWait(part);
                Thread.Sleep(2);
            }
            // Instead of sending text, the code could pop up a dialog and ask the user to pick something from a list
            // My XLG and Quick Scripts 
        }
                    
>>

A hot phrase could be press and release just ctrl, then shift, then alt to trigger a piece of your code. The possible combinations are practically limitless. 

So why? Well, I've always wanted the ability to activate a piece of my code globally without tripping up existing hotkeys. 
I've got a couple of other programs, xlg and quick scripts which have regeneration and run functions which I can now tie to a global phrase.

Maybe it's just me who wants to do this, but hey. I'd love to hear how you use this guy.

Thanks to NHotkey for the base code and layout. I changed a lot because there's a fundamentally different approach when clicking one key combination as opposed to a sequence of keys. 
I tried to take care to make sure NHotKeys and NHotPhrases could exist side by side.


