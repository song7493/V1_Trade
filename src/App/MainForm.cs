using System;
using System.Windows.Forms;

namespace V1_Trade.App
{
    public class MainForm : FormBase
    {
        private readonly StatusStrip _statusStrip;
        private readonly ToolStripStatusLabel _springLabel;
        private readonly ToolStripStatusLabel _clockLabel;
        private readonly MenuStrip _menuStrip;
        private readonly TabControl _tabControl;
        private readonly Timer _clockTimer;

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";
            KeyPreview = true;

            SuspendLayout();

            // MenuStrip
            _menuStrip = new MenuStrip();
            _menuStrip.Dock = DockStyle.Top;
            _menuStrip.Items.Add(CreateMenuItem("Futures"));
            _menuStrip.Items.Add(CreateMenuItem("Options"));
            _menuStrip.Items.Add(CreateMenuItem("Accounts"));
            _menuStrip.Items.Add(CreateMenuItem("Analytics"));
            _menuStrip.Items.Add(CreateMenuItem("Test"));

            for (int i = _menuStrip.Items.Count - 1; i >= 0; i--)
                if (_menuStrip.Items[i] is ToolStripMenuItem mi && mi.Text == "Settings")
                    _menuStrip.Items.RemoveAt(i);

            var settingsItem = new ToolStripMenuItem("Settings");
            var fontSettingsItem = new ToolStripMenuItem("Font Settings…");
            fontSettingsItem.Click += (s, e) => new V1_Trade.Screens.Settings.FontSettingsForm().ShowDialog(this);
            fontSettingsItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F;
            fontSettingsItem.ShowShortcutKeys = true;
            settingsItem.DropDownItems.Add(fontSettingsItem);
            _menuStrip.Items.Add(settingsItem);
            MainMenuStrip = _menuStrip;

            // TabControl
            _tabControl = new TabControl();
            _tabControl.Dock = DockStyle.Fill;
            _tabControl.TabPages.Clear();
            _tabControl.TabPages.Add(new TabPage("Futures"));
            _tabControl.TabPages.Add(new TabPage("Options"));
            _tabControl.TabPages.Add(new TabPage("Accounts"));
            _tabControl.TabPages.Add(new TabPage("Analytics"));
            _tabControl.TabPages.Add(new TabPage("Test"));

            // StatusStrip
            _statusStrip = new StatusStrip();
            _statusStrip.Dock = DockStyle.Bottom;

            _springLabel = new ToolStripStatusLabel();
            _springLabel.Spring = true;

            _clockLabel = new ToolStripStatusLabel();
            _clockLabel.Alignment = ToolStripItemAlignment.Right;

            _statusStrip.Items.Add(_springLabel);
            _statusStrip.Items.Add(_clockLabel);

            // Add controls in specific order
            Controls.Add(_menuStrip);
            Controls.Add(_tabControl);
            Controls.Add(_statusStrip);

            ResumeLayout(false);
            PerformLayout();

            _clockTimer = new Timer();
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += TimerTick;

            UpdateClock();
            _clockTimer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        private void UpdateClock()
        {
            _clockLabel.Text = DateTime.Now.ToString("yyyy-MM-dd dddd tt h:mm:ss");
        }

        private static ToolStripMenuItem CreateMenuItem(string text)
        {
            var item = new ToolStripMenuItem(text);
            return item;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.F)
            {
                new V1_Trade.Screens.Settings.FontSettingsForm().ShowDialog(this);
                e.Handled = true;
                return;
            }
            base.OnKeyDown(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _clockTimer.Tick -= TimerTick;
                _clockTimer.Dispose();
                _statusStrip.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
