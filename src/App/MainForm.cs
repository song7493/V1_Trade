using System;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
    {
        private readonly MenuStrip _menu;
        private readonly TabControl _tabs;
        private readonly StatusStrip _statusStrip;
        private readonly ToolStripStatusLabel _springLabel;
        private readonly ToolStripStatusLabel _clockLabel;
        private readonly Timer _clockTimer;

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";

            _menu = new MenuStrip();
            ToolStripItem[] menuItems = new ToolStripItem[]
            {
                new ToolStripMenuItem("Futures"),
                new ToolStripMenuItem("Options"),
                new ToolStripMenuItem("Accounts"),
                new ToolStripMenuItem("Analytics"),
                new ToolStripMenuItem("Test"),
                new ToolStripMenuItem("Settings")
            };
            _menu.Items.AddRange(menuItems);
            MainMenuStrip = _menu;

            _tabs = new TabControl();
            _tabs.Dock = DockStyle.Fill;

            _statusStrip = new StatusStrip();

            _springLabel = new ToolStripStatusLabel();
            _springLabel.Spring = true;

            _clockLabel = new ToolStripStatusLabel();
            _clockLabel.Alignment = ToolStripItemAlignment.Right;

            _statusStrip.Items.Add(_springLabel);
            _statusStrip.Items.Add(_clockLabel);

            Controls.Add(_tabs);
            Controls.Add(_statusStrip);
            Controls.Add(_menu);

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
                _tabs.Dispose();
                _statusStrip.Dispose();
                _menu.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
