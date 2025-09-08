using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace V1_Trade.Screens.Settings
{
    public class FontSettingsForm : V1_Trade.App.FormBase
    {
        private readonly ComboBox _combo;
        private readonly NumericUpDown _num;
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
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 2
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            layout.Controls.Add(new Label { Text = "Font:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            _combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
            _combo.Items.AddRange(FontFamily.Families.Select(f => f.Name).OrderBy(n => n).Cast<object>().ToArray());
            layout.Controls.Add(_combo, 1, 0);

            layout.Controls.Add(new Label { Text = "Size:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            _num = new NumericUpDown { Minimum = 8, Maximum = 24, Value = 12, Width = 60 };
            layout.Controls.Add(_num, 1, 1);

            _combo.SelectedItem = Font.FontFamily.Name;
            if ((decimal)Font.Size >= _num.Minimum && (decimal)Font.Size <= _num.Maximum)
                _num.Value = (decimal)Font.Size;

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true
            };

            _cancel = new Button { Text = "Cancel" };
            _cancel.Click += (s, e) => Close();

            _save = new Button { Text = "Save" };
            _save.Click += (s, e) => { V1_Trade.Infrastructure.UI.FontManager.SetFont(_combo.Text, (float)_num.Value, true); Close(); };

            _apply = new Button { Text = "Apply" };
            _apply.Click += (s, e) => V1_Trade.Infrastructure.UI.FontManager.SetFont(_combo.Text, (float)_num.Value, false);

            buttons.Controls.Add(_cancel);
            buttons.Controls.Add(_save);
            buttons.Controls.Add(_apply);

            Controls.Add(buttons);
            Controls.Add(layout);
        }
    }
}
