using System;
using System.Drawing;
using System.Windows.Forms;
using V1_Trade.App;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.Screens.Settings
{
    /// <summary>
    /// Allows the user to change the global UI font.
    /// </summary>
    public class FontSettingsForm : FormBase
    {
        private readonly ComboBox _fontCombo;
        private readonly NumericUpDown _fontSize;
        private readonly Button _applyButton;
        private readonly Button _saveButton;
        private readonly Button _cancelButton;

        public FontSettingsForm()
        {
            Text = "Font Settings";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(300, 120);

            var fontLabel = new Label();
            fontLabel.AutoSize = true;
            fontLabel.Text = "Font:";
            fontLabel.Location = new Point(12, 15);

            _fontCombo = new ComboBox();
            _fontCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _fontCombo.Location = new Point(60, 12);
            _fontCombo.Width = 200;
            foreach (var family in FontFamily.Families)
                _fontCombo.Items.Add(family.Name);
            _fontCombo.SelectedItem = FontManager.CurrentFontName;

            var sizeLabel = new Label();
            sizeLabel.AutoSize = true;
            sizeLabel.Text = "Size:";
            sizeLabel.Location = new Point(12, 45);

            _fontSize = new NumericUpDown();
            _fontSize.Minimum = 8;
            _fontSize.Maximum = 24;
            _fontSize.Location = new Point(60, 42);
            _fontSize.Width = 60;
            var size = FontManager.CurrentFontSize;
            if (size < 8) size = 8;
            if (size > 24) size = 24;
            _fontSize.Value = (decimal)size;

            _applyButton = new Button();
            _applyButton.Text = "Apply";
            _applyButton.Location = new Point(40, 80);
            _applyButton.Click += (s, e) => FontManager.SetFont(_fontCombo.Text, (float)_fontSize.Value, false);

            _saveButton = new Button();
            _saveButton.Text = "Save";
            _saveButton.Location = new Point(120, 80);
            _saveButton.Click += (s, e) => { FontManager.SetFont(_fontCombo.Text, (float)_fontSize.Value, true); Close(); };

            _cancelButton = new Button();
            _cancelButton.Text = "Cancel";
            _cancelButton.Location = new Point(200, 80);
            _cancelButton.Click += (s, e) => Close();

            Controls.Add(fontLabel);
            Controls.Add(_fontCombo);
            Controls.Add(sizeLabel);
            Controls.Add(_fontSize);
            Controls.Add(_applyButton);
            Controls.Add(_saveButton);
            Controls.Add(_cancelButton);

            AcceptButton = _saveButton;
            CancelButton = _cancelButton;
        }
    }
}
