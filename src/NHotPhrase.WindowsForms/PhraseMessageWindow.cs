using System.Windows.Forms;

namespace NHotPhrase.WindowsForms
{
    public class PhraseMessageWindow : ContainerControl
    {
        public readonly HotPhraseManager _hotPhraseManager;

        public PhraseMessageWindow(HotPhraseManager hotPhraseManager)
        {
            _hotPhraseManager = hotPhraseManager;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parameters = base.CreateParams;
                parameters.Parent = HotPhraseManagerBase.WindowMessage;
                return parameters;
            }
        }

        protected override void WndProc(ref Message m)
        {
            var handled = false;
            Hotkey hotkey;
            m.Result = _hotPhraseManager.HandleHotkeyMessage(Handle, m.Msg, m.WParam, m.LParam, ref handled, out hotkey);
            if (!handled)
                base.WndProc(ref m);
        }
    }
}