using System;
using System.Globalization;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
    {
        private readonly StatusStrip _status = new();
        private readonly ToolStripStatusLabel _spring = new() { Spring = true };
        private readonly ToolStripStatusLabel _clockLabel = new();
        private readonly Timer _timer = new();

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";

            _status.Items.Add(_spring);
            _status.Items.Add(_clockLabel);
            Controls.Add(_status);

            _timer.Interval = 1000;
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _clockLabel.Text = DateTime.Now.ToString("yyyy-MM-dd dddd tt h:mm:ss");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Tick -= TimerTick;
                _timer.Dispose();
                _status.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
