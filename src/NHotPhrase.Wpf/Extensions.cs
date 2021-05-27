using System.Windows.Input;

namespace NHotPhrase.Wpf
{
    static class Extensions
    {
        public static bool HasFlag(this ModifierKeys modifiers, ModifierKeys flag)
        {
            return (modifiers & flag) == flag;
        }

        public static bool HasFlag(this HotPhraseFlags flags, HotPhraseFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}
