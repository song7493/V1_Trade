using System;
using System.Linq;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    /// <summary>
    /// Dialog for configuring the application's global font.
    /// </summary>
    public class FontSettingsForm : FormBase
    {
        private readonly TextBox _nameBox;
        private readonly TextBox _sizeBox;
        private readonly Button _applyButton;
        private readonly Button _saveButton;
        private readonly Button _cancelButton;

        public FontSettingsForm()
        {
            Text = "Font Settings";

            var nameLabel = new Label { Text = "Font Name:", AutoSize = true };
            _nameBox = new TextBox { Width = 200 };

            var sizeLabel = new Label { Text = "Font Size:", AutoSize = true };
            _sizeBox = new TextBox { Width = 60 };

            _applyButton = new Button { Text = "Apply" };
            _saveButton = new Button { Text = "Save" };
            _cancelButton = new Button { Text = "Cancel" };

            _applyButton.Click += ApplyClick;
            _saveButton.Click += SaveClick;
            _cancelButton.Click += (s, e) => Close();

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.AutoSize = true;
            layout.ColumnCount = 2;
            layout.RowCount = 3;

            layout.Controls.Add(nameLabel, 0, 0);
            layout.Controls.Add(_nameBox, 1, 0);
            layout.Controls.Add(sizeLabel, 0, 1);
            layout.Controls.Add(_sizeBox, 1, 1);

            var buttons = new FlowLayoutPanel();
            buttons.FlowDirection = FlowDirection.RightToLeft;
            buttons.Dock = DockStyle.Fill;
            buttons.Controls.Add(_cancelButton);
            buttons.Controls.Add(_saveButton);
            buttons.Controls.Add(_applyButton);

            layout.Controls.Add(buttons, 0, 2);
            layout.SetColumnSpan(buttons, 2);

            Controls.Add(layout);

            AcceptButton = _saveButton;
            CancelButton = _cancelButton;
            StartPosition = FormStartPosition.CenterParent;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _nameBox.Text = FontManager.CurrentFontName;
            _sizeBox.Text = FontManager.CurrentFontSize.ToString();
        }

        private void ApplyClick(object sender, EventArgs e)
        {
            ApplyFont();
        }

        private void SaveClick(object sender, EventArgs e)
        {
            ApplyFont();
            FontManager.Save();
            Close();
        }

        private void ApplyFont()
        {
            var name = _nameBox.Text.Trim();
            if (!TryParseSize(_sizeBox.Text, out var size))
                size = FontManager.CurrentFontSize;

            FontManager.SetFont(name, size);

            foreach (Form form in Application.OpenForms.Cast<Form>())
                FontManager.Apply(form);
        }

        private static bool TryParseSize(string text, out float size)
        {
            size = 0f;
            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim();
            if (text.EndsWith("pt", StringComparison.OrdinalIgnoreCase))
                text = text.Substring(0, text.Length - 2);

            return float.TryParse(text, out size);
        }
    }
}

