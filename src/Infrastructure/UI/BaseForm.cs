using System;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Base form that applies global font settings.
    /// </summary>
    public class BaseForm : Form
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            FontManager.ApplyFontDeep(this);
        }
    }
}
