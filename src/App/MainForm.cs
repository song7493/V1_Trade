using System;
using System.Windows.Forms;
using V1_Trade.App.Ui;
using V1_Trade.Screens.Settings;

namespace V1_Trade.App
{
    public class MainForm : BaseForm
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

            var settings = new ToolStripMenuItem("설정");
            settings.DropDownItems.Add(new ToolStripMenuItem("환경설정...", (s, e) => new AppearanceForm().ShowDialog(this)));
            _menuStrip.Items.Add(settings);

            Text = "V1 Trade";
            WindowState = FormWindowState.Maximized;
        }
    }
}
