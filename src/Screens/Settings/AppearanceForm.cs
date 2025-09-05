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
            _fontCombo.SelectedItem = _theme.CurrentFont.Name;
            _sizeUpDown.Value = (decimal)_theme.CurrentFont.Size;
        }

        private void Apply()
        {
            var name = _fontCombo.SelectedItem?.ToString() ?? _theme.CurrentFont.Name;
            var size = (float)_sizeUpDown.Value;
            _theme.SetFont(name, size);
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
