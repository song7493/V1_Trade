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

            SuspendLayout();

            // MenuStrip
            _menuStrip = new MenuStrip();
            _menuStrip.Dock = DockStyle.Top;
            _menuStrip.Items.Add(new ToolStripMenuItem("Futures"));
            _menuStrip.Items.Add(new ToolStripMenuItem("Options"));
            _menuStrip.Items.Add(new ToolStripMenuItem("Accounts"));
            _menuStrip.Items.Add(new ToolStripMenuItem("Analytics"));
            _menuStrip.Items.Add(new ToolStripMenuItem("Test"));

            var settingsMenu = new ToolStripMenuItem("Settings");
            var fontMenu = new ToolStripMenuItem("Font Settings...");
            fontMenu.ShortcutKeys = Keys.Control | Keys.Shift | Keys.F;
            fontMenu.Click += FontSettingsClick;
            settingsMenu.DropDownItems.Add(fontMenu);
            _menuStrip.Items.Add(settingsMenu);

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
            _tabControl.TabPages.Add(new TabPage("Settings"));

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

        private void FontSettingsClick(object sender, EventArgs e)
        {
            using (var dlg = new FontSettingsForm())
                dlg.ShowDialog(this);
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
