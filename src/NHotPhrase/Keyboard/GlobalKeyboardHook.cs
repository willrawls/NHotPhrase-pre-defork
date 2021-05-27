using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NHotPhrase.Keyboard
{
    //Based on https://gist.github.com/Stasonix
    public class GlobalKeyboardHook : IDisposable
    {
        public const int WH_KEYBOARD_LL = 13;
        //public const int KfAltdown = 0x2000;
        //public const int LlkhfAltdown = KfAltdown >> 8;
        //public const int HC_ACTION = 0;

        public HookProc _hookProc;
        public IntPtr _user32LibraryHandle;
        public IntPtr _windowsHookHandle;

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        // EDT: Added an optional parameter (registeredKeys) that accepts keys to restrict
        // the logging mechanism.
        /// <summary>
        /// </summary>
        /// <param name="registeredKeys">Keys that should trigger logging. Pass null for full logging.</param>
        public GlobalKeyboardHook()
        {
            _windowsHookHandle = IntPtr.Zero;
            _user32LibraryHandle = IntPtr.Zero;
            _hookProc = LowLevelKeyboardProc; // we must keep alive _hookProc, because GC is not aware about SetWindowsHookEx behaviour.

            _user32LibraryHandle = Win32.LoadLibrary("User32");
            if (_user32LibraryHandle == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode,
                    $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

            _windowsHookHandle = Win32.SetWindowsHookEx(WH_KEYBOARD_LL, _hookProc, _user32LibraryHandle, 0);
            if (_windowsHookHandle == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode,
                    $"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }
        }

        public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardPressed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // because we can unhook only in the same thread, not in garbage collector thread
                if (_windowsHookHandle != IntPtr.Zero)
                {
                    if (!Win32.UnhookWindowsHookEx(_windowsHookHandle))
                    {
                        var errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(errorCode,
                            $"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                    }

                    _windowsHookHandle = IntPtr.Zero;

                    // ReSharper disable once DelegateSubtraction
                    _hookProc -= LowLevelKeyboardProc;
                }

            if (_user32LibraryHandle == IntPtr.Zero) return;
            
            if (!Win32.FreeLibrary(_user32LibraryHandle)) // reduces reference to library by 1.
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode,
                    $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

            _user32LibraryHandle = IntPtr.Zero;
        }

        ~GlobalKeyboardHook()
        {
            Dispose(false);
        }

        public IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var fEatKeyStroke = false;

            var wparamTyped = wParam.ToInt32();
            if (Enum.IsDefined(typeof(KeyboardState), wparamTyped))
            {
                var o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));
                var p = (LowLevelKeyboardInputEvent) o;

                var eventArguments = new GlobalKeyboardHookEventArgs(p, (KeyboardState) wparamTyped);

                // EDT: Removed the comparison-logic from the usage-area so the user does not need to mess around with it.
                // Either the incoming key has to be part of RegisteredKeys (see constructor on top) or RegisterdKeys
                // has to be null for the event to get fired.
                var key = (Keys) p.VirtualCode;

                var handler = KeyboardPressed;
                handler?.Invoke(this, eventArguments);
                fEatKeyStroke = eventArguments.Handled;
            }

            return fEatKeyStroke ? (IntPtr) 1 : Win32.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}