using System;
using System.Windows.Forms;

namespace V1_Trade.App
{
    public class MainForm : Form
    {
        private readonly MenuStrip _menuStrip;
        private readonly StatusStrip _statusStrip;
        private readonly ToolStripStatusLabel _clockLabel;
        private readonly TabControl _workspaceHost;
        private readonly Timer _clockTimer;

        public MainForm()
        {
            _menuStrip = new MenuStrip();
            _statusStrip = new StatusStrip();
            _clockLabel = new ToolStripStatusLabel();
            _workspaceHost = new TabControl { Dock = DockStyle.Fill };
            _clockTimer = new Timer { Interval = 1000 };

            _statusStrip.Items.Add(_clockLabel);
            _clockTimer.Tick += (s, e) => _clockLabel.Text = DateTime.Now.ToString("HH:mm:ss");
            _clockTimer.Start();

            Controls.Add(_workspaceHost);
            Controls.Add(_statusStrip);
            Controls.Add(_menuStrip);

            MainMenuStrip = _menuStrip;
            Text = "V1 Trade";
            WindowState = FormWindowState.Maximized;
        }
    }
}
