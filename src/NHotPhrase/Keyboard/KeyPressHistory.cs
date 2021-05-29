using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NHotPhrase.Keyboard
{
    public class KeyHistory : List<Keys>
    {
        // public List<Keys> History { get; set; } = new();
        public DateTime LastPressAt { get; set; } = DateTime.MinValue;
        public int MaxHistoryLength { get; set; } = 8;
        public int ClearAfterThisManySeconds { get; set; } = 5;

        public KeyHistory()
        {
        }

        public KeyHistory(int maxHistoryLength, int clearAfterThisManySeconds, DateTime lastPressAt, List<Keys> history)
        {
            MaxHistoryLength = maxHistoryLength;
            ClearAfterThisManySeconds = clearAfterThisManySeconds;
            LastPressAt = lastPressAt;
            AddRange(history);
        }

        public new void Add(Keys key)
        {
            AddKeyPress(key);
        }
        public KeyHistory AddKeyPress(Keys key)
        {
            // If too much time has gone by, clear the queue
            if (Count > 0 && DateTime.Now.Subtract(LastPressAt).Seconds > ClearAfterThisManySeconds)
            {
                Clear();
            }

            // If the history is too long, truncate it keeping the newest entries
            while (Count > MaxHistoryLength)
            {
                RemoveAt(0);
            }

            LastPressAt = DateTime.Now;
            base.Add(key);
            return this;
        }

        public static object SyncRoot = new();
        public List<Keys> KeyList()
        {
            lock (SyncRoot)
            {
                return new List<Keys>(this);
            }
        }
    }
}