using System;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
    {
        private readonly MenuStrip _menu;
        private readonly StatusStrip _status;
        private readonly TabControl _tabs;
        private readonly Timer _timer;
        private readonly ToolStripStatusLabel _clock;

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";

            _menu = new MenuStrip();
            _status = new StatusStrip();
            _tabs = new TabControl();
            _timer = new Timer();
            _clock = new ToolStripStatusLabel();

            Controls.Add(_tabs);
            Controls.Add(_status);
            Controls.Add(_menu);

            InitializeMenu();
            InitializeControls();
            InitializeStatus();
        }

        private void InitializeMenu()
        {
            var futuresItem = new ToolStripMenuItem("Futures");
            var optionsItem = new ToolStripMenuItem("Options");
            var accountsItem = new ToolStripMenuItem("Accounts");
            var analyticsItem = new ToolStripMenuItem("Analytics");
            var testItem = new ToolStripMenuItem("Test");
            var settingsItem = new ToolStripMenuItem("Settings");

            _menu.Items.AddRange(new ToolStripItem[]
            {
                futuresItem,
                optionsItem,
                accountsItem,
                analyticsItem,
                testItem,
                settingsItem
            });

            MainMenuStrip = _menu;
        }

        private void InitializeControls()
        {
            // Controls
            _tabs.Dock = DockStyle.Fill;
        }

        private void InitializeStatus()
        {
            _status.Items.Add(_clock);

            _timer.Interval = 1000;
            _timer.Tick += TimerOnTick;
            _timer.Start();
            TimerOnTick(null, EventArgs.Empty);
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            _clock.Text = DateTime.Now.ToString("yyyy-MM-dd ddd tt h:mm:ss");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            base.OnFormClosed(e);
        }
    }
}
