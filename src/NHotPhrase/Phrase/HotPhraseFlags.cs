﻿using System;

namespace NHotPhrase.Phrase
{
    [Flags]
    public enum HotPhraseFlags : uint
    {
        None = 0x0000,
        Alt = 0x0001,
        Control = 0x0002,
        Shift = 0x0004,
        Windows = 0x0008,
        NoRepeat = 0x4000
    }
}