using System;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
    {
        private readonly MenuStrip _menu;
        private readonly StatusStrip _status;
        private readonly Timer _timer;
        private readonly TabControl _tabs;
        private readonly ToolStripStatusLabel _clock;

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

            _status = new StatusStrip();
            _clock = new ToolStripStatusLabel();
            _status.Items.Add(_clock);

            _tabs = new TabControl();
            _tabs.Dock = DockStyle.Fill;

            Controls.Add(_tabs);
            Controls.Add(_status);
            Controls.Add(_menu);

            MainMenuStrip = _menu;

            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += (sender, args) =>
            {
                _clock.Text = DateTime.Now.ToString("yyyy-MM-dd ddd tt h:mm:ss");
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

