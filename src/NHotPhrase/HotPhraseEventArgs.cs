using System;

namespace NHotPhrase
{
    public class HotPhraseEventArgs : EventArgs
    {
        public HotPhraseEventArgs(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool Handled { get; set; }
    }
}