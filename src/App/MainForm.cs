using System;
using System.Windows.Forms;
using V1_Trade.Screens.Settings;

namespace V1_Trade.App
{
    public class MainForm : FormBase
    {
        private readonly StatusStrip _statusStrip;
        private readonly ToolStripStatusLabel _springLabel;
        private readonly ToolStripStatusLabel _clockLabel;
        private readonly MenuStrip _menuStrip;
        private readonly TabControl _tabControl;
        private readonly Timer _clockTimer;

        public MainForm()
        {
            Text = "V1 Trade (Baseline)";
            KeyPreview = true;

            SuspendLayout();

            // MenuStrip
            _menuStrip = new MenuStrip { Dock = DockStyle.Top };
            _menuStrip.Items.Add(CreateTabMenuItem("Futures", 0));
            _menuStrip.Items.Add(CreateTabMenuItem("Options", 1));
            _menuStrip.Items.Add(CreateTabMenuItem("Accounts", 2));
            _menuStrip.Items.Add(CreateTabMenuItem("Analytics", 3));
            _menuStrip.Items.Add(CreateTabMenuItem("Test", 4));

            var settingsMenu = new ToolStripMenuItem("Settings");
            var fontItem = new ToolStripMenuItem("Font Settings...");
            fontItem.Click += (s, e) => new FontSettingsForm().ShowDialog(this);
            settingsMenu.DropDownItems.Add(fontItem);
            _menuStrip.Items.Add(settingsMenu);
            MainMenuStrip = _menuStrip;

            // TabControl
            _tabControl = new TabControl { Dock = DockStyle.Fill };
            _tabControl.TabPages.Add(new TabPage("Futures"));
            _tabControl.TabPages.Add(new TabPage("Options"));
            _tabControl.TabPages.Add(new TabPage("Accounts"));
            _tabControl.TabPages.Add(new TabPage("Analytics"));
            _tabControl.TabPages.Add(new TabPage("Test"));
            _tabControl.TabPages.Add(new TabPage("Settings"));

            // StatusStrip
            _statusStrip = new StatusStrip { Dock = DockStyle.Bottom };

            _springLabel = new ToolStripStatusLabel { Spring = true };

            _clockLabel = new ToolStripStatusLabel { Alignment = ToolStripItemAlignment.Right };

            _statusStrip.Items.Add(_springLabel);
            _statusStrip.Items.Add(_clockLabel);

            Controls.Add(_menuStrip);
            Controls.Add(_tabControl);
            Controls.Add(_statusStrip);

            ResumeLayout(false);
            PerformLayout();

            _clockTimer = new Timer { Interval = 1000 };
            _clockTimer.Tick += TimerTick;

            UpdateClock();
            _clockTimer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        private void UpdateClock()
        {
            _clockLabel.Text = DateTime.Now.ToString("yyyy-MM-dd dddd tt h:mm:ss");
        }

        private ToolStripMenuItem CreateTabMenuItem(string text, int index)
        {
            var item = new ToolStripMenuItem(text);
            item.Click += (s, e) => _tabControl.SelectedIndex = index;
            return item;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.F))
            {
                new FontSettingsForm().ShowDialog(this);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _clockTimer.Tick -= TimerTick;
                _clockTimer.Dispose();
                _statusStrip.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
