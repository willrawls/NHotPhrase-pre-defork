using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace NHotPhrase.Wpf
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design",
        "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "This is a singleton; disposing it would break it")]
    public class HotPhraseManager
    {
        #region Singleton implementation

        public static HotPhraseManager Current => LazyInitializer.Instance;

        public static class LazyInitializer
        {
            static LazyInitializer() { }
            public static readonly HotPhraseManager Instance = new HotPhraseManager();
        }

        #endregion

        #region Attached property for KeyBindings

        [AttachedPropertyBrowsableForType(typeof(KeyBinding))]
        public static bool GetRegisterGlobalHotkey(KeyBinding binding)
        {
            return (bool)binding.GetValue(RegisterGlobalHotkeyProperty);
        }

        public static void SetRegisterGlobalHotkey(KeyBinding binding, bool value)
        {
            binding.SetValue(RegisterGlobalHotkeyProperty, value);
        }

        public static readonly DependencyProperty RegisterGlobalHotkeyProperty =
            DependencyProperty.RegisterAttached(
                "RegisterGlobalHotkey",
                typeof(bool),
                typeof(HotPhraseManager),
                new PropertyMetadata(
                    false,
                    RegisterGlobalHotkeyPropertyChanged));

        public static void RegisterGlobalHotkeyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var keyBinding = d as KeyBinding;
            if (keyBinding == null)
                return;

            var oldValue = (bool) e.OldValue;
            var newValue = (bool) e.NewValue;

            if (DesignerProperties.GetIsInDesignMode(d))
                return;

            if (oldValue && !newValue)
            {
                Current.RemoveKeyBinding(keyBinding);
            }
            else if (newValue && !oldValue)
            {
                Current.AddKeyBinding(keyBinding);
            }
        }

        #endregion

        #region HotkeyAlreadyRegistered event

        public static event EventHandler<HotkeyAlreadyRegisteredEventArgs> HotPhraseAlreadyRegistered;

        public static void ONHotPhraseAlreadyRegistered(string name)
        {
            var handler = HotPhraseAlreadyRegistered;
            if (handler != null)
                handler(null, new HotkeyAlreadyRegisteredEventArgs(name));
        }

        #endregion

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        public readonly HwndSource _source;
        public readonly WeakReferenceCollection<KeyBinding> _keyBindings;

        public HotPhraseManager()
        {
            _keyBindings = new WeakReferenceCollection<KeyBinding>();

            var parameters = new HwndSourceParameters("Hotkey sink")
                             {
                                 HwndSourceHook = HandleMessage,
                                 ParentWindow = WindowMessage
                             };
            _source = new HwndSource(parameters);
            SetHwnd(_source.Handle);
        }

        public void AddOrReplace(string name, KeyGesture gesture, EventHandler<HotPhraseEventArgs> handler)
        {
            AddOrReplace(name, gesture, false, handler);
        }

        public void AddOrReplace(string name, KeyGesture gesture, bool noRepeat, EventHandler<HotPhraseEventArgs> handler)
        {
            AddOrReplace(name, gesture.Key, gesture.Modifiers, noRepeat, handler);
        }

        public void AddOrReplace(string name, Key key, ModifierKeys modifiers, EventHandler<HotPhraseEventArgs> handler)
        {
            AddOrReplace(name, key, modifiers, false, handler);
        }

        public void AddOrReplace(string name, Key key, ModifierKeys modifiers, bool noRepeat, EventHandler<HotPhraseEventArgs> handler)
        {
            var flags = GetFlags(modifiers, noRepeat);
            var vk = (uint)KeyInterop.VirtualKeyFromKey(key);
            AddOrReplace(name, vk, flags, handler);
        }

        public static HotPhraseFlags GetFlags(ModifierKeys modifiers, bool noRepeat)
        {
            var flags = HotPhraseFlags.None;
            if (modifiers.HasFlag(ModifierKeys.Shift))
                flags |= HotPhraseFlags.Shift;
            if (modifiers.HasFlag(ModifierKeys.Control))
                flags |= HotPhraseFlags.Control;
            if (modifiers.HasFlag(ModifierKeys.Alt))
                flags |= HotPhraseFlags.Alt;
            if (modifiers.HasFlag(ModifierKeys.Windows))
                flags |= HotPhraseFlags.Windows;
            if (noRepeat)
                flags |= HotPhraseFlags.NoRepeat;
            return flags;
        }

        public static ModifierKeys GetModifiers(HotPhraseFlags flags)
        {
            var modifiers = ModifierKeys.None;
            if (flags.HasFlag(HotPhraseFlags.Shift))
                modifiers |= ModifierKeys.Shift;
            if (flags.HasFlag(HotPhraseFlags.Control))
                modifiers |= ModifierKeys.Control;
            if (flags.HasFlag(HotPhraseFlags.Alt))
                modifiers |= ModifierKeys.Alt;
            if (flags.HasFlag(HotPhraseFlags.Windows))
                modifiers |= ModifierKeys.Windows;
            return modifiers;
        }

        public void AddKeyBinding(KeyBinding keyBinding)
        {
            var gesture = (KeyGesture)keyBinding.Gesture;
            var name = GetNameForKeyBinding(gesture);
            try
            {
                AddOrReplace(name, gesture.Key, gesture.Modifiers, null);
                _keyBindings.Add(keyBinding);
            }
            catch (HotkeyAlreadyRegisteredException)
            {
                ONHotPhraseAlreadyRegistered(name);
            }
        }

        public void RemoveKeyBinding(KeyBinding keyBinding)
        {
            var gesture = (KeyGesture)keyBinding.Gesture;
            var name = GetNameForKeyBinding(gesture);
            Remove(name);
            _keyBindings.Remove(keyBinding);
        }

        public readonly KeyGestureConverter _gestureConverter = new KeyGestureConverter();
        public string GetNameForKeyBinding(KeyGesture gesture)
        {
            var name = gesture.DisplayString;
            if (string.IsNullOrEmpty(name))
                name = _gestureConverter.ConvertToString(gesture);
            return name;
        }

        public IntPtr HandleMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            Hotkey hotkey;
            var result = HandleHotkeyMessage(hwnd, msg, wparam, lparam, ref handled, out hotkey);
            if (handled)
                return result;

            if (hotkey != null)
                handled = ExecuteBoundCommand(hotkey);

            return result;
        }

        public bool ExecuteBoundCommand(Hotkey hotkey)
        {
            var key = KeyInterop.KeyFromVirtualKey((int)hotkey.VirtualKey);
            var modifiers = GetModifiers(hotkey.Flags);
            var handled = false;
            foreach (var binding in _keyBindings)
            {
                if (binding.Key == key && binding.Modifiers == modifiers)
                {
                    handled |= ExecuteCommand(binding);
                }
            }
            return handled;
        }

        public static bool ExecuteCommand(InputBinding binding)
        {
            var command = binding.Command;
            var parameter = binding.CommandParameter;
            var target = binding.CommandTarget;

            if (command == null)
                return false;

            var routedCommand = command as RoutedCommand;
            if (routedCommand != null)
            {
                if (routedCommand.CanExecute(parameter, target))
                {
                    routedCommand.Execute(parameter, target);
                    return true;
                }
            }
            else
            {
                if (command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                    return true;
                }
            }
            return false;
        }
    }
}
