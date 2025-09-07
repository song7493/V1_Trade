using System;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
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

            _menuStrip = new MenuStrip();
            _menuStrip.Dock = DockStyle.Top;

            var mnuFutures = new ToolStripMenuItem("Futures");
            mnuFutures.DropDownItems.Add(new ToolStripMenuItem("Placeholder…"));
            _menuStrip.Items.Add(mnuFutures);

            var mnuOptions = new ToolStripMenuItem("Options");
            mnuOptions.DropDownItems.Add(new ToolStripMenuItem("Placeholder…"));
            _menuStrip.Items.Add(mnuOptions);

            var mnuAccounts = new ToolStripMenuItem("Accounts");
            mnuAccounts.DropDownItems.Add(new ToolStripMenuItem("Placeholder…"));
            _menuStrip.Items.Add(mnuAccounts);

            var mnuAnalytics = new ToolStripMenuItem("Analytics");
            mnuAnalytics.DropDownItems.Add(new ToolStripMenuItem("Placeholder…"));
            _menuStrip.Items.Add(mnuAnalytics);

            var mnuTest = new ToolStripMenuItem("Test");
            mnuTest.DropDownItems.Add(new ToolStripMenuItem("Placeholder…"));
            _menuStrip.Items.Add(mnuTest);

            var mnuSettings = new ToolStripMenuItem("Settings");
            mnuSettings.DropDownItems.Add(new ToolStripMenuItem("Placeholder…"));
            _menuStrip.Items.Add(mnuSettings);

            MainMenuStrip = _menuStrip;

            _tabControl = new TabControl();
            _tabControl.Dock = DockStyle.Fill;
            _tabControl.TabPages.Add(new TabPage("Futures"));
            _tabControl.TabPages.Add(new TabPage("Options"));
            _tabControl.TabPages.Add(new TabPage("Accounts"));
            _tabControl.TabPages.Add(new TabPage("Analytics"));
            _tabControl.TabPages.Add(new TabPage("Test"));
            _tabControl.TabPages.Add(new TabPage("Settings"));

            _statusStrip = new StatusStrip();

            _springLabel = new ToolStripStatusLabel();
            _springLabel.Spring = true;

            _clockLabel = new ToolStripStatusLabel();
            _clockLabel.Alignment = ToolStripItemAlignment.Right;

            _statusStrip.Items.Add(_springLabel);
            _statusStrip.Items.Add(_clockLabel);

            Controls.Add(_menuStrip);
            Controls.Add(_statusStrip);
            Controls.Add(_tabControl);

            _clockTimer = new Timer();
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += TimerTick;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

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
