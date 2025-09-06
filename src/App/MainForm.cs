using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
    {
        private readonly StatusStrip _status = new StatusStrip();
        private readonly ToolStripStatusLabel _spring = new ToolStripStatusLabel() { Spring = true };
        private readonly ToolStripStatusLabel _clockLabel = new ToolStripStatusLabel();
        private readonly Timer _timer = new Timer();

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";
        }
    }
}
