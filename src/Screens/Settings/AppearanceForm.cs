using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using V1_Trade.App.Ui;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.Screens.Settings
{
    public partial class AppearanceForm : BaseForm
    {
        private readonly AppThemeService _theme = AppThemeService.Instance;

        public AppearanceForm()
        {
            InitializeComponent();
            Load += AppearanceForm_Load;
        }

        private void AppearanceForm_Load(object sender, EventArgs e)
        {
            var fonts = FontFamily.Families.Select(f => f.Name).OrderBy(n => n).ToArray();
            _fontCombo.Items.AddRange(fonts);
            var current = _theme.CurrentFont ?? Control.DefaultFont;
            _fontCombo.SelectedItem = current.Name;
            _sizeUpDown.Value = (decimal)current.Size;
        }

        private void Apply()
        {
            var selectedName = _fontCombo.SelectedItem?.ToString();
            var size = (float)_sizeUpDown.Value;
            var current = _theme.CurrentFont ?? Control.DefaultFont;
            if (selectedName != null &&
                (current.Name != selectedName || Math.Abs(current.Size - size) > 0.1f))
            {
                _theme.SetFont(selectedName, size);
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Apply();
            Close();
        }
    }
}
