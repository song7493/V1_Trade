using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Base user control that applies global font settings once during construction.
    /// </summary>
    public class BaseControl : UserControl
    {
        public BaseControl()
        {
            FontManager.ApplyFont(this);
        }
    }
}
