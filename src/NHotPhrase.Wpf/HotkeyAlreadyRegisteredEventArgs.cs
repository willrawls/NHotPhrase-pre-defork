using System;

namespace NHotPhrase.Wpf
{
    public class HotkeyAlreadyRegisteredEventArgs : EventArgs
    {
        private readonly string _name;

        public HotkeyAlreadyRegisteredEventArgs(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
