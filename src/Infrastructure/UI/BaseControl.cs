using System;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Base user control that applies global font settings.
    /// </summary>
    public class BaseControl : UserControl
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            FontManager.Apply(this);
        }
    }
}
