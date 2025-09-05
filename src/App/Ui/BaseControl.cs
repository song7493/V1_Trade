using System;
using System.Drawing;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App.Ui
{
    public class BaseControl : UserControl
    {
        protected readonly AppThemeService ThemeService = AppThemeService.Instance;

        public BaseControl()
        {
            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.Dpi;
            ThemeService.ThemeChanged += OnThemeChanged;
            ApplyFontRecursive(this, ThemeService.CurrentFont);
            ControlAdded += (s, e) => ApplyFontRecursive(e.Control, ThemeService.CurrentFont);
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            ApplyFontRecursive(this, ThemeService.CurrentFont);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeService.ThemeChanged -= OnThemeChanged;
            }
            base.Dispose(disposing);
        }

        protected void ApplyFontRecursive(Control c, Font f)
        {
            if (c.Font.Name != f.Name || Math.Abs(c.Font.Size - f.Size) > 0.1f)
            {
                c.Font = f;
                if (c is DataGridView dgv)
                {
                    dgv.DefaultCellStyle.Font = f;
                    dgv.ColumnHeadersDefaultCellStyle.Font = f;
                    dgv.RowTemplate.Height = Math.Max(20, (int)(f.GetHeight() + 8));
                }
                else if (c is ToolStrip ts)
                {
                    ts.Font = f;
                }
            }

            foreach (Control child in c.Controls)
            {
                ApplyFontRecursive(child, f);
            }
        }
    }
}
