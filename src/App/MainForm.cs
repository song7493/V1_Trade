using System;
using System.Drawing;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public sealed class MainForm : BaseForm
    {
        private MenuStrip _menu;
        private StatusStrip _status;
        private ToolStripStatusLabel _clock;
        private TabControl _workspaceHost;
        private Timer _timer;

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(480, 360);

            _menu = new MenuStrip();
            _menu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("Futures"),
                new ToolStripMenuItem("Options"),
                new ToolStripMenuItem("Accounts"),
                new ToolStripMenuItem("Analytics"),
                new ToolStripMenuItem("Test"),
                new ToolStripMenuItem("Settings")
            });
            MainMenuStrip = _menu;

            _status = new StatusStrip();
            _clock = new ToolStripStatusLabel();
            _status.Items.Add(_clock);

            _workspaceHost = new TabControl { Dock = DockStyle.Fill };

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += (_, __) =>
                _clock.Text = DateTime.Now.ToString("yyyy-MM-dd ddd tt h:mm:ss");
            _timer.Start();

            Controls.Add(_workspaceHost);
            Controls.Add(_status);
            Controls.Add(_menu);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
            base.OnFormClosed(e);
        }
    }
}
