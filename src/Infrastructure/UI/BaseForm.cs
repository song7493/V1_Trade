using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Base form that applies global font settings once during construction.
    /// </summary>
    public class BaseForm : Form
    {
        public BaseForm()
        {
            FontManager.ApplyFont(this);
        }
    }
}
