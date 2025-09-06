using System;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
    {
        // Controls
        private readonly MenuStrip _menu;
        private readonly StatusStrip _status;
        private readonly Timer _timer;
        private readonly ToolStripStatusLabel _clockLabel;
        private readonly TabControl _tabs;

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";

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

            _clockLabel = new ToolStripStatusLabel();
            _status = new StatusStrip();
            _status.Items.Add(_clockLabel);

            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += (s, e) =>
            {
                _clockLabel.Text = DateTime.Now.ToString("yyyy-MM-dd ddd tt h:mm:ss");
            };
            _timer.Start();

            _tabs = new TabControl();
            _tabs.Dock = DockStyle.Fill;

            Controls.Add(_tabs);
            Controls.Add(_status);
            Controls.Add(_menu);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            base.OnFormClosed(e);
        }
    }
}
