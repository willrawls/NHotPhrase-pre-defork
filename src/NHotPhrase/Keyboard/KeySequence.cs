using System.Collections.Generic;
using System.Windows.Forms;

namespace NHotPhrase.Keyboard
{
    public class KeySequence
    {
        public static KeySequence As()
        {
            return new();
        }

        public readonly List<Keys> Sequence = new();

        public KeySequence WhenKeyRepeats(Keys repeatKey, int repeatCount)
        {
            for (var i = 0; i < repeatCount; i++)
            {
                Sequence.Add(repeatKey);
            }
            return this;
        }

        public KeySequence WhenKeyReleased(Keys key)
        {
            Sequence.Add(key);
            return this;
        }
    }
}