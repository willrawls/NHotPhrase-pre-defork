using System.Linq;
using System.Windows.Forms;

namespace NHotPhrase.Keyboard
{
    public class SendKeysKeyword
    {
        public static string KeyToSendKey(Keys key)
        {
            var keyword = Keywords.FirstOrDefault(k => k.Number == (int) key);
            return keyword != null 
                ? keyword.Name 
                : key.ToString();
        }

        public static readonly SendKeysKeyword[] Keywords = new SendKeysKeyword[49]
        {
            new("ENTER", 13),
            new("TAB", 9),
            new("ESC", 27),
            new("ESCAPE", 27),
            new("HOME", 36),
            new("END", 35),
            new("LEFT", 37),
            new("RIGHT", 39),
            new("UP", 38),
            new("DOWN", 40),
            new("PGUP", 33),
            new("PGDN", 34),
            new("NUMLOCK", 144),
            new("SCROLLLOCK", 145),
            new("PRTSC", 44),
            new("BREAK", 3),
            new("BACKSPACE", 8),
            new("BKSP", 8),
            new("BS", 8),
            new("CLEAR", 12),
            new("CAPSLOCK", 20),
            new("INS", 45),
            new("INSERT", 45),
            new("DEL", 46),
            new("DELETE", 46),
            new("HELP", 47),
            new("F1", 112),
            new("F2", 113),
            new("F3", 114),
            new("F4", 115),
            new("F5", 116),
            new("F6", 117),
            new("F7", 118),
            new("F8", 119),
            new("F9", 120),
            new("F10", 121),
            new("F11", 122),
            new("F12", 123),
            new("F13", 124),
            new("F14", 125),
            new("F15", 126),
            new("F16", sbyte.MaxValue),
            new("MULTIPLY", 106),
            new("ADD", 107),
            new("SUBTRACT", 109),
            new("DIVIDE", 111),
            new("+", 107),
            new("%", 65589),
            new("^", 65590)
        };

        public string Name { get; }
        public int Number { get; }

        public SendKeysKeyword(string name, int number)
        {
            Name = name;
            Number = number;
        }
   }
}