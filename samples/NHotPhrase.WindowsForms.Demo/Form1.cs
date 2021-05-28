using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms.Demo
{
    public partial class Form1 : Form
    {
        public static readonly Keys IncrementKeys = Keys.Control | Keys.Alt | Keys.Up;
        public static readonly Keys DecrementKeys = Keys.Control | Keys.Alt | Keys.Down;

        public int _value;

        public Form1()
        {
            InitializeComponent();

            chkEnableGlobalHotkeys.Checked = true;
        }

        private void SetupHotPhrases()
        {
            HotPhraseManager.Current.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Toggle phrase activation")
                    .WhenKeyRepeats(Keys.RControlKey, 3)
                    .Call(OnTogglePhraseActivation)
            );

            HotPhraseManager.Current.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Increment")
                    .WhenKeyPressed(Keys.ControlKey)
                    .ThenKeyPressed(Keys.Shift)
                    .ThenKeyPressed(Keys.Alt)
                    .Call(OnIncrement)
            );

            // Spell it out long hand
            HotPhraseManager.Current.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Decrement")
                    .WhenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.D)
                    .ThenKeyPressed(Keys.Back)
                    .Call(OnDecrement)
            );

            // Or use the NHotkey like syntax 
            HotPhraseManager.Current.AddOrReplace("Decrement", new[] {Keys.CapsLock, Keys.CapsLock, Keys.D, Keys.Back},
                OnDecrement);
        }

        private void OnTogglePhraseActivation(object? sender, HotPhraseEventArgs e)
        {
            chkEnableGlobalHotkeys.Checked = !chkEnableGlobalHotkeys.Checked;

            if (chkEnableGlobalHotkeys.Checked)
            {
                SetupHotPhrases();
            }
            else
            {
                HotPhraseManager.StopListening();
            }
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

        public void chkEnableGlobalHotkeys_CheckedChanged(object sender, System.EventArgs e)
        {
            HotPhraseManager.Current.IsEnabled = chkEnableGlobalHotkeys.Checked;
        }

        public void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
