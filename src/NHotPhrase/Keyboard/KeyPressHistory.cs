﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NHotPhrase.Keyboard
{
    public class KeyPressHistory : List<Keys>
    {
        // public List<Keys> History { get; set; } = new();
        public DateTime LastPressAt { get; set; } = DateTime.MinValue;
        public int MaxHistoryLength { get; set; } = 8;
        public int ClearAfterThisManySeconds { get; set; } = 5;

        public KeyPressHistory()
        {
        }

        public KeyPressHistory(int maxHistoryLength, int clearAfterThisManySeconds, DateTime lastPressAt, List<Keys> history)
        {
            MaxHistoryLength = maxHistoryLength;
            ClearAfterThisManySeconds = clearAfterThisManySeconds;
            LastPressAt = lastPressAt;
            History.AddRange(history);
        }

        public KeyPressHistory Add(Keys key)
        {
            // If too much time has gone by, clear the queue
            if (DateTime.Now.Subtract(LastPressAt).Seconds > ClearAfterThisManySeconds)
            {
                History.Clear();
            }

            // If the history is too long, truncate it keeping the newest entries
            while (History.Count > MaxHistoryLength)
            {
                History.RemoveAt(0);
            }

            LastPressAt = DateTime.Now;
            History.Add(key);
            return this;
        }

        public KeyPressHistory Clone()
        {
            return new KeyPressHistory(MaxHistoryLength, ClearAfterThisManySeconds, LastPressAt, History);
        }
    }
}