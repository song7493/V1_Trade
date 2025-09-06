using System;
using System.Drawing;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
    {
        private readonly MenuStrip _menu;
        private readonly StatusStrip _status;
        private readonly ToolStripStatusLabel _clock;
        private readonly TabControl _workspaceHost;
        private readonly Timer _timer;

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(480, 360);

            _menu = new MenuStrip();
            _menu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("선물"),
                new ToolStripMenuItem("옵션"),
                new ToolStripMenuItem("계좌"),
                new ToolStripMenuItem("분석"),
                new ToolStripMenuItem("Test"),
                new ToolStripMenuItem("설정")
            });

            _clock = new ToolStripStatusLabel();
            _status = new StatusStrip();
            _status.Items.Add(_clock);

            _workspaceHost = new TabControl
            {
                Dock = DockStyle.Fill
            };

            _timer = new Timer
            {
                Interval = 1000
            };
            _timer.Tick += (sender, args) =>
                _clock.Text = DateTime.Now.ToString("yyyy-MM-dd dddd  tt h:mm:ss");
            _timer.Start();

            Controls.Add(_workspaceHost);
            Controls.Add(_status);
            Controls.Add(_menu);
            MainMenuStrip = _menu;
        }
    }
}
