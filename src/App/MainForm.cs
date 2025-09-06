using System;
using System.Drawing;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public sealed class MainForm : BaseForm
    {
        private readonly MenuStrip _menu = new();
        private readonly ToolStripStatusLabel _status = new();
        private readonly Timer _timer = new();

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(480, 360);

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

            var statusStrip = new StatusStrip();
            statusStrip.Items.Add(_status);
            _status.Text = DateTime.Now.ToString("yyyy-MM-dd ddd tt h:mm:ss");

            Controls.Add(_menu);
            Controls.Add(statusStrip);

            _timer.Interval = 1000;
            _timer.Tick += (s, e) =>
            {
                _status.Text = DateTime.Now.ToString("yyyy-MM-dd ddd tt h:mm:ss");
            };
            _timer.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            base.OnFormClosed(e);
        }
    }
}
