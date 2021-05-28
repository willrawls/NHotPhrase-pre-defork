using System;
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

        public static bool IsAMatch(Keys exactingKey, Keys simplifiableKey)
        {
            if (IsExacting(exactingKey))
            {
                return exactingKey == simplifiableKey;
            }
            return Simplified(exactingKey) == Simplified(simplifiableKey);
        }

        // If they user says for instance "LShiftKey" then it has to be the left shift key. But "ShiftKey" means either of the shift keys
        public static bool IsExacting(Keys key)
        {
            switch(key)  
            {
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.RControlKey:
                case Keys.LControlKey:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.LWin:
                case Keys.RWin:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                    return true;
            }

            return false;
        }

        public static Keys Simplified(Keys key)
        {
            switch(key)
            {
                case Keys.Return:
                    return Keys.Enter; // Enter returns itself

                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    return Keys.ShiftKey;

                case Keys.Control:
                case Keys.ControlKey:
                case Keys.RControlKey:
                case Keys.LControlKey:
                    return Keys.Control;

                case Keys.Alt:
                case Keys.LMenu:
                case Keys.RMenu:
                    return Keys.Alt;

                case Keys.LWin:
                case Keys.RWin:
                    return Keys.LWin;

                case Keys.D0:
                case Keys.NumPad0:
                    return Keys.D0;

                case Keys.D1:
                case Keys.NumPad1:
                    return Keys.D1;

                case Keys.D2:
                case Keys.NumPad2:
                    return Keys.D2;

                case Keys.D3:
                case Keys.NumPad3:
                    return Keys.D3;

                case Keys.D4:
                case Keys.NumPad4:
                    return Keys.D4;

                case Keys.D5:
                case Keys.NumPad5:
                    return Keys.D5;

                case Keys.D6:
                case Keys.NumPad6:
                    return Keys.D6;

                case Keys.D7:
                case Keys.NumPad7:
                    return Keys.D7;

                case Keys.D8:
                case Keys.NumPad8:
                    return Keys.D8;

                case Keys.D9:
                case Keys.NumPad9:
                    return Keys.D9;
                
                case Keys.Oemplus:
                case Keys.Add:
                    return Keys.Oemplus;

                case Keys.OemMinus:
                case Keys.Separator:
                    return Keys.OemMinus;
            }

            return key;
        }
    }
}