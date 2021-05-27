using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace NHotPhrase.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public static readonly KeyGesture IncrementGesture = new KeyGesture(Key.Up, ModifierKeys.Control | ModifierKeys.Alt);
        public static readonly KeyGesture DecrementGesture = new KeyGesture(Key.Down, ModifierKeys.Control | ModifierKeys.Alt);

        public MainWindow()
        {
            HotPhraseManager.HotPhraseAlreadyRegistered += HotPhraseManagerHotPhraseAlreadyRegistered;

            HotPhraseManager.Current.AddOrReplace("Increment", IncrementGesture, OnIncrement);
            HotPhraseManager.Current.AddOrReplace("Decrement", DecrementGesture, OnDecrement);
        }

        public void HotPhraseManagerHotPhraseAlreadyRegistered(object sender, HotkeyAlreadyRegisteredEventArgs e)
        {
            MessageBox.Show($"The hotkey {e.Name} is already registered by another application");
        }

        public void OnIncrement(object sender, HotPhraseEventArgs e)
        {
            Value++;
            e.Handled = true;
        }

        public void OnDecrement(object sender, HotPhraseEventArgs e)
        {
            Value--;
            e.Handled = true;
        }

        public int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand _negateCommand;
        public ICommand NegateCommand => _negateCommand ??= new DelegateCommand(Negate);

        public DelegateCommand _testCommand;
        public ICommand TestCommand => _testCommand ??= new DelegateCommand(Test);

        public string IncrementHotkey => IncrementGesture.GetDisplayStringForCulture(null);
        public string DecrementHotkey => DecrementGesture.GetDisplayStringForCulture(null);

        public bool IsHotkeyManagerEnabled
        {
            get => HotPhraseManager.Current.IsEnabled;
            set => HotPhraseManager.Current.IsEnabled = value;
        }

        public void Test()
        {
            MessageBox.Show("Test");
        }


        public void Negate()
        {
            Value = -Value;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
