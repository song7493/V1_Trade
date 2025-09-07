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

            SuspendLayout();

            // MenuStrip
            _menuStrip = new MenuStrip();
            _menuStrip.Dock = DockStyle.Top;
            _menuStrip.Items.Add(CreateMenuItem("Futures"));
            _menuStrip.Items.Add(CreateMenuItem("Options"));
            _menuStrip.Items.Add(CreateMenuItem("Accounts"));
            _menuStrip.Items.Add(CreateMenuItem("Analytics"));
            _menuStrip.Items.Add(CreateMenuItem("Test"));
            _menuStrip.Items.Add(CreateMenuItem("Settings"));
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
            Controls.Add(_tabControl);
            Controls.Add(_statusStrip);
            Controls.Add(_menuStrip);

            ResumeLayout(false);
            PerformLayout();

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

        private static ToolStripMenuItem CreateMenuItem(string text)
        {
            var item = new ToolStripMenuItem(text);
            item.DropDownItems.Add("Placeholder");
            return item;
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
