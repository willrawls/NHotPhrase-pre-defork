using System;
using System.Threading;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms.Demo
{
    public partial class Form1 : Form
    {
        public static readonly Keys IncrementKeys = Keys.Control | Keys.Alt | Keys.Up;
        public static readonly Keys DecrementKeys = Keys.Control | Keys.Alt | Keys.Down;
        public int _value;

        public HotPhraseManager Manager { get; set; }

        public Form1()
        {
            InitializeComponent();
            SetupHotPhrases();
        }

        private void SetupHotPhrases()
        {
            Manager?.Dispose();
            Manager = new HotPhraseManager(this);

            Manager.Watcher.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Toggle phrase activation")
                    .WhenKeyRepeats(Keys.RControlKey, 3)
                    .ThenCall(OnTogglePhraseActivation)
            );

            Manager.Watcher.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Increment")
                    .WhenKeyPressed(Keys.ControlKey)
                    .ThenKeyPressed(Keys.Shift)
                    .ThenKeyPressed(Keys.Alt)
                    .ThenCall(OnIncrement)
            );

            // Spell it out long hand
            Manager.Watcher.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Decrement")
                    .WhenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.D)
                    .ThenKeyPressed(Keys.Back)
                    .ThenCall(OnDecrement)
            );

            // Or use the NHotkey like syntax 
            Manager.Watcher.AddOrReplace("Decrement", new[] {Keys.CapsLock, Keys.CapsLock, Keys.D, Keys.Back}, OnDecrement);
        }

        private void OnTogglePhraseActivation(object sender, HotPhraseEventArgs e)
        {
            EnableGlobalHotkeysCheckBox_CheckedChanged(sender, null);
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

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                lblValue.Text = _value.ToString();
            }
        }

        public static object SyncRoot = new object();
        public static bool UiChanging = false;

        public delegate void CheckedChangedDelegate(object sender, System.EventArgs e);
        public void EnableGlobalHotkeysCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new CheckedChangedDelegate(EnableGlobalHotkeysCheckBox_CheckedChanged), null, null);
                return;
            }

            if (!Monitor.TryEnter(SyncRoot)) return;
            if (UiChanging == true)
                return;
            UiChanging = true;
            try
            {
                if (EnableGlobalHotkeysCheckBox.Checked)
                {
                    SetupHotPhrases();
                }
                else
                {
                    Manager?.Dispose();
                    Manager = null;
                }
            }
            finally
            {
                UiChanging = false;
                Monitor.Exit(SyncRoot);
            }
        }
    }
}
