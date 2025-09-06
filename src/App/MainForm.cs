using System;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
    {
        private readonly ToolStripStatusLabel _clockLabel;
        private readonly Timer _timer;
        private readonly TabControl _tabControl;

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";

            var menuStrip = new MenuStrip();
            menuStrip.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("선물"),
                new ToolStripMenuItem("옵션"),
                new ToolStripMenuItem("계좌"),
                new ToolStripMenuItem("분석"),
                new ToolStripMenuItem("Test"),
                new ToolStripMenuItem("설정")
            });
            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);

            _clockLabel = new ToolStripStatusLabel();
            var statusStrip = new StatusStrip();
            statusStrip.Items.Add(_clockLabel);
            Controls.Add(statusStrip);

            _tabControl = new TabControl { Dock = DockStyle.Fill };
            Controls.Add(_tabControl);

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += (sender, args) => UpdateClock();
            _timer.Start();
            UpdateClock();
        }

        private void UpdateClock()
        {
            _clockLabel.Text = DateTime.Now.ToString("yyyy-MM-dd dddd tt h:mm:ss");
        }
    }
}
