using System;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Base user control that applies global font settings.
    /// </summary>
    public class BaseControl : UserControl
    {
        public BaseControl()
        {
            FontManager.Instance.FontChanged += OnFontChanged;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            FontManager.Instance.ApplyFontDeep(this);
        }

        private void OnFontChanged(object sender, EventArgs e)
        {
            FontManager.Instance.ApplyFontDeep(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                FontManager.Instance.FontChanged -= OnFontChanged;
            }
            base.Dispose(disposing);
        }
    }
}
