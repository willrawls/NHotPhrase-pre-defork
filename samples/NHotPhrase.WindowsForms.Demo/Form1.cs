using System.Windows.Forms;

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

            HotPhraseManager.Current.AddOrReplace("Increment", IncrementKeys, OnIncrement);
            HotPhraseManager.Current.AddOrReplace("Decrement", DecrementKeys, OnDecrement);

            chkEnableGlobalHotkeys.Checked = HotPhraseManager.Current.IsEnabled;
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
