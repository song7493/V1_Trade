using System;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    /// <summary>
    /// Base form that applies the global font settings automatically.
    /// </summary>
    public class FormBase : Form
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            FontManager.Apply(this);
        }
    }
}
