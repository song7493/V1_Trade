using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using V1_Trade.App;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.Screens.Settings
{
    public class FontSettingsForm : FormBase
    {
        private readonly ComboBox _fontCombo;
        private readonly NumericUpDown _fontSize;
        private readonly Button _applyButton;
        private readonly Button _saveButton;
        private readonly Button _cancelButton;

        public FontSettingsForm()
        {
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 200);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var layout = new TableLayoutPanel();
            layout.ColumnCount = 2;
            layout.RowCount = 3;
            layout.Padding = new Padding(12);
            layout.Dock = DockStyle.Fill;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var fontLabel = new Label();
            fontLabel.Text = "Font:";
            fontLabel.TextAlign = ContentAlignment.MiddleLeft;
            fontLabel.Anchor = AnchorStyles.Left;
            fontLabel.Margin = new Padding(6);
            fontLabel.AutoSize = true;

            _fontCombo = new ComboBox();
            _fontCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _fontCombo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _fontCombo.Margin = new Padding(6);
            _fontCombo.TabIndex = 0;

            var sizeLabel = new Label();
            sizeLabel.Text = "Size:";
            sizeLabel.TextAlign = ContentAlignment.MiddleLeft;
            sizeLabel.Anchor = AnchorStyles.Left;
            sizeLabel.Margin = new Padding(6);
            sizeLabel.AutoSize = true;

            _fontSize = new NumericUpDown();
            _fontSize.Minimum = 8;
            _fontSize.Maximum = 32;
            _fontSize.DecimalPlaces = 0;
            _fontSize.Increment = 1;
            _fontSize.Anchor = AnchorStyles.Left;
            _fontSize.Margin = new Padding(6);
            _fontSize.TabIndex = 1;

            var buttonsPanel = new FlowLayoutPanel();
            buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonsPanel.Dock = DockStyle.Fill;
            buttonsPanel.Margin = new Padding(6);

            _applyButton = new Button();
            _applyButton.Text = "Apply";
            _applyButton.Margin = new Padding(6);
            _applyButton.TabIndex = 2;
            _applyButton.Click += (s, e) => ApplyFont(false);

            _saveButton = new Button();
            _saveButton.Text = "Save";
            _saveButton.Margin = new Padding(6);
            _saveButton.TabIndex = 3;
            _saveButton.Click += (s, e) => { ApplyFont(true); Close(); };

            _cancelButton = new Button();
            _cancelButton.Text = "Cancel";
            _cancelButton.Margin = new Padding(6);
            _cancelButton.TabIndex = 4;
            _cancelButton.Click += (s, e) => Close();

            buttonsPanel.Controls.Add(_applyButton);
            buttonsPanel.Controls.Add(_saveButton);
            buttonsPanel.Controls.Add(_cancelButton);

            layout.Controls.Add(fontLabel, 0, 0);
            layout.Controls.Add(_fontCombo, 1, 0);
            layout.Controls.Add(sizeLabel, 0, 1);
            layout.Controls.Add(_fontSize, 1, 1);
            layout.Controls.Add(buttonsPanel, 0, 2);
            layout.SetColumnSpan(buttonsPanel, 2);

            Controls.Add(layout);

            AcceptButton = _saveButton;
            CancelButton = _cancelButton;

            _fontCombo.SelectedIndexChanged += (s, e) => UpdateButtons();
            _fontSize.ValueChanged += (s, e) => UpdateButtons();

            LoadFonts();
            UpdateButtons();
        }

        private void LoadFonts()
        {
            var fonts = FontFamily.Families.Select(f => f.Name).ToArray();
            _fontCombo.Items.AddRange(fonts);

            string currentName = GetCurrentFontName();
            float currentSize = GetCurrentFontSize();

            if (_fontCombo.Items.Count > 0)
            {
                int idx = _fontCombo.Items.IndexOf(currentName);
                _fontCombo.SelectedIndex = idx >= 0 ? idx : 0;
            }

            if (currentSize < _fontSize.Minimum) currentSize = (float)_fontSize.Minimum;
            if (currentSize > _fontSize.Maximum) currentSize = (float)_fontSize.Maximum;
            _fontSize.Value = (decimal)currentSize;
        }

        private void UpdateButtons()
        {
            bool enable = _fontCombo.Items.Count > 0;
            _applyButton.Enabled = enable;
            _saveButton.Enabled = enable;
        }

        private void ApplyFont(bool save)
        {
            var name = _fontCombo.Text;
            var size = (float)_fontSize.Value;
            var type = typeof(FontManager);
            var method = type.GetMethod("SetFont", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (method != null)
            {
                try
                {
                    method.Invoke(null, new object[] { name, size, save });
                    return;
                }
                catch { }
            }

            // Fallback if SetFont is not available
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings["UI.Font.Name"] != null)
                    config.AppSettings.Settings["UI.Font.Name"].Value = name;
                else
                    config.AppSettings.Settings.Add("UI.Font.Name", name);
                if (config.AppSettings.Settings["UI.Font.Size"] != null)
                    config.AppSettings.Settings["UI.Font.Size"].Value = size.ToString();
                else
                    config.AppSettings.Settings.Add("UI.Font.Size", size.ToString());
                if (save)
                {
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch { }

            foreach (Form form in Application.OpenForms)
            {
                try { FontManager.Apply(form); } catch { }
            }
        }

        private static string GetCurrentFontName()
        {
            var prop = typeof(FontManager).GetProperty("CurrentFontName", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (prop != null)
            {
                var val = prop.GetValue(null) as string;
                if (!string.IsNullOrEmpty(val)) return val;
            }
            return SystemFonts.DefaultFont.FontFamily.Name;
        }

        private static float GetCurrentFontSize()
        {
            var prop = typeof(FontManager).GetProperty("CurrentFontSize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (prop != null)
            {
                try
                {
                    var val = Convert.ToSingle(prop.GetValue(null));
                    return val;
                }
                catch { }
            }
            return SystemFonts.DefaultFont.Size;
        }
    }
}
