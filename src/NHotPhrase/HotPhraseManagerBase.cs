/*
using System;
using System.Collections.Generic;

namespace NHotPhrase
{
    public abstract class HotPhraseManagerBase
    {
        public readonly Dictionary<int, string> HotkeyNames = new Dictionary<int, string>();
        public readonly Dictionary<string, Hotkey> Hotkeys = new Dictionary<string, Hotkey>();
        public IntPtr WindowHandle;
        public static readonly IntPtr WindowMessage = (IntPtr)(-3);

        public HotPhraseManagerBase()
        {
        }

        public void AddOrReplace(string name, uint virtualKey, HotPhraseFlags flags, EventHandler<HotPhraseEventArgs> handler)
        {
            var hotkey = new Hotkey(virtualKey, flags, handler);
            lock (Hotkeys)
            {
                Remove(name);
                Hotkeys.Add(name, hotkey);
                HotkeyNames.Add(hotkey.Id, name);
                if (WindowHandle != IntPtr.Zero)
                    hotkey.Register(WindowHandle, name);
            }
        }

        public void Remove(string name)
        {
            lock (Hotkeys)
            {
                Hotkey hotkey;
                if (Hotkeys.TryGetValue(name, out hotkey))
                {
                    Hotkeys.Remove(name);
                    HotkeyNames.Remove(hotkey.Id);
                    if (WindowHandle != IntPtr.Zero)
                        hotkey.Unregister();
                }
            }
        }

        public bool IsEnabled { get; set; } = true;

        public void SetHwnd(IntPtr hwnd)
        {
            WindowHandle = hwnd;
        }

        public const int WmHotkey = 0x0312;

        public IntPtr HandleHotkeyMessage(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled,
            out Hotkey hotkey)
        {
            hotkey = null;
            if (IsEnabled && msg == WmHotkey)
            {
                var id = wParam.ToInt32();
                string name;
                if (HotkeyNames.TryGetValue(id, out name))
                {
                    hotkey = Hotkeys[name];
                    var handler = hotkey.Handler;
                    if (handler != null)
                    {
                        var e = new HotPhraseEventArgs(name);
                        handler(this, e);
                        handled = e.Handled;
                    }
                }
            }
            return IntPtr.Zero;
        }
    }
}
*/
