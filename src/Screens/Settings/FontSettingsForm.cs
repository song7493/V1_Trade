using System;
using System.Windows.Forms;
using V1_Trade.App;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.Screens.Settings
{
    /// <summary>
    /// Form to configure global font settings.
    /// </summary>
    public class FontSettingsForm : FormBase
    {
        private readonly ComboBox _fontCombo = new ComboBox();
        private readonly NumericUpDown _fontSize = new NumericUpDown();
        private readonly Button _applyButton = new Button();
        private readonly Button _saveButton = new Button();

        public FontSettingsForm()
        {
            _applyButton.Text = "Apply";
            _saveButton.Text = "Save";

            _applyButton.Click += OnApply;
            _saveButton.Click += OnSave;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var name = FontManager.CurrentFontName;
            var size = FontManager.CurrentFontSize;

            int idx = _fontCombo.Items.IndexOf(name);
            _fontCombo.SelectedIndex = (idx >= 0 ? idx : (_fontCombo.Items.Count > 0 ? 0 : -1));

            if (size < 8) size = 8;
            if (size > 32) size = 32;
            _fontSize.Value = (decimal)size;

            _applyButton.Enabled = _saveButton.Enabled = (_fontCombo.Items.Count > 0);
        }

        private void OnApply(object? sender, EventArgs e)
        {
            var name = _fontCombo.SelectedItem as string ?? string.Empty;
            var size = (float)_fontSize.Value;
            FontManager.SetFont(name, size, false);
        }

        private void OnSave(object? sender, EventArgs e)
        {
            var name = _fontCombo.SelectedItem as string ?? string.Empty;
            var size = (float)_fontSize.Value;
            FontManager.SetFont(name, size, true);
        }
    }
}

