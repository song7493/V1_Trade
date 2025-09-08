using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.Screens.Settings
{
    public class FontSettingsForm : V1_Trade.App.FormBase
    {
        private readonly ComboBox _fontCombo;
        private readonly NumericUpDown _fontSize;
        private readonly Button _apply;
        private readonly Button _save;
        private readonly Button _cancel;

        public FontSettingsForm()
        {
            Text = "Font Settings";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 200);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(10)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            layout.Controls.Add(new Label { Text = "Font", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 0);
            _fontCombo = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _fontCombo.Items.AddRange(FontFamily.Families.Select(f => f.Name).ToArray());
            layout.Controls.Add(_fontCombo, 1, 0);

            layout.Controls.Add(new Label { Text = "Size", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, 1);
            _fontSize = new NumericUpDown { Minimum = 8, Maximum = 32, Increment = 1, Width = 60, Dock = DockStyle.Left };
            layout.Controls.Add(_fontSize, 1, 1);

            var buttons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill, AutoSize = true };
            _apply = new Button { Text = "Apply" };
            _save = new Button { Text = "Save" };
            _cancel = new Button { Text = "Cancel" };
            buttons.Controls.Add(_cancel);
            buttons.Controls.Add(_save);
            buttons.Controls.Add(_apply);
            layout.Controls.Add(buttons, 0, 2);
            layout.SetColumnSpan(buttons, 2);

            Controls.Add(layout);

            AcceptButton = _save;
            CancelButton = _cancel;

            _apply.Click += (s, e) => FontManager.SetFont(_fontCombo.Text, (float)_fontSize.Value, false);
            _save.Click += (s, e) => { FontManager.SetFont(_fontCombo.Text, (float)_fontSize.Value, true); Close(); };
            _cancel.Click += (s, e) => Close();

            var currentFont = Font;
            _fontCombo.SelectedItem = currentFont.FontFamily.Name;
            _fontSize.Value = (decimal)currentFont.Size;
        }
    }
}
