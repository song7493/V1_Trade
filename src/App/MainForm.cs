using System;
using System.Windows.Forms;
using V1_Trade.Infrastructure.Telemetry;

namespace V1_Trade.App
{
    public class MainForm : Form
    {
        private readonly MenuStrip _menuStrip;
        private readonly StatusStrip _statusStrip;
        private readonly ToolStripStatusLabel _connectionLabel;
        private readonly ToolStripStatusLabel _eventsLabel;
        private readonly ToolStripStatusLabel _uiLabel;
        private readonly ToolStripStatusLabel _clockLabel;
        private readonly TabControl _workspaceHost;
        private readonly Timer _clockTimer;

        public MainForm()
        {
            _menuStrip = new MenuStrip();
            _statusStrip = new StatusStrip();
            _connectionLabel = new ToolStripStatusLabel();
            _eventsLabel = new ToolStripStatusLabel();
            _uiLabel = new ToolStripStatusLabel();
            _clockLabel = new ToolStripStatusLabel();
            _workspaceHost = new TabControl { Dock = DockStyle.Fill };
            _clockTimer = new Timer { Interval = 1000 };

            _statusStrip.Items.AddRange(new ToolStripItem[] { _connectionLabel, _eventsLabel, _uiLabel, _clockLabel });
            _clockTimer.Tick += (s, e) =>
            {
                _clockLabel.Text = DateTime.Now.ToString("HH:mm:ss");
                _connectionLabel.Text = "Connected";
                var tel = TelemetryClient.Instance;
                _eventsLabel.Text = $"{tel.EventsPerSecond:F1} evt/s";
                _uiLabel.Text = $"UI p95: {tel.UiP95:F1} ms";
            };
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
