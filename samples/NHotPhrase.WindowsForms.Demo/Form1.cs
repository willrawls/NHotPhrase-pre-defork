using System.Windows.Forms;

namespace NHotPhrase.WindowsForms.Demo
{
    public partial class Form1 : Form
    {
        private static readonly Keys IncrementKeys = Keys.Control | Keys.Alt | Keys.Up;
        private static readonly Keys DecrementKeys = Keys.Control | Keys.Alt | Keys.Down;

        private int _value;

        public Form1()
        {
            InitializeComponent();

            HotPhraseManager.Current.AddOrReplace("Increment", IncrementKeys, OnIncrement);
            HotPhraseManager.Current.AddOrReplace("Decrement", DecrementKeys, OnDecrement);

            chkEnableGlobalHotkeys.Checked = HotPhraseManager.Current.IsEnabled;
        }

        private void OnIncrement(object sender, HotkeyEventArgs e)
        {
            Value++;
            e.Handled = true;
        }

        private void OnDecrement(object sender, HotkeyEventArgs e)
        {
            Value--;
            e.Handled = true;
        }

        private int Value
        {
            get => _value;
            set
            {
                _value = value;
                lblValue.Text = _value.ToString();
            }
        }

        private void chkEnableGlobalHotkeys_CheckedChanged(object sender, System.EventArgs e)
        {
            HotPhraseManager.Current.IsEnabled = chkEnableGlobalHotkeys.Checked;
        }
    }
}
