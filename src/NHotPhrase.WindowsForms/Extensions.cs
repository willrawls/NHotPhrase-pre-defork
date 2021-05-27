using System.Windows.Forms;

namespace NHotPhrase.WindowsForms
{
    static class Extensions
    {
        public static bool HasFlag(this Keys keys, Keys flag)
        {
            return (keys & flag) == flag;
        }
    }
}
