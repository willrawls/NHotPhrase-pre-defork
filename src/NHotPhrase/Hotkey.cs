using System;
using System.Runtime.InteropServices;
using NHotPhrase.Keyboard;

namespace NHotPhrase
{
    public class Hotkey
    {
        public static int _nextId;

        public Hotkey(uint virtualKey, HotPhraseFlags flags, EventHandler<HotPhraseEventArgs> handler)
        {
            Id = ++_nextId;
            VirtualKey = virtualKey;
            Flags = flags;
            Handler = handler;
        }

        public int Id { get; }

        public uint VirtualKey { get; }

        public HotPhraseFlags Flags { get; }

        public EventHandler<HotPhraseEventArgs> Handler { get; }

        public IntPtr WindowHandle;

        public void Register(IntPtr windowHandle, string name)
        {
            if (!Win32.RegisterHotKey(windowHandle, Id, Flags, VirtualKey))
            {
                var hr = Marshal.GetHRForLastWin32Error();
                var ex = Marshal.GetExceptionForHR(hr);
                if ((uint) hr == 0x80070581)
                    throw new HotkeyAlreadyRegisteredException(name, ex);
                throw ex;
            }
            WindowHandle = windowHandle;
        }

        public void Unregister()
        {
            if (WindowHandle == IntPtr.Zero) return;

            if (!Win32.UnregisterHotKey(WindowHandle, Id))
            {
                var hr = Marshal.GetHRForLastWin32Error();
                throw Marshal.GetExceptionForHR(hr);
            }
            WindowHandle = IntPtr.Zero;
        }
    }
}