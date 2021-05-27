using System;

namespace NHotPhrase
{
    public class HotkeyEventArgs : EventArgs
    {
        public HotkeyEventArgs(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool Handled { get; set; }
    }
}