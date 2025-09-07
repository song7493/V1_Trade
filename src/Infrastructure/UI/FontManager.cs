using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Provides helpers to apply a configured font across WinForms controls.
    /// </summary>
    public static class FontManager
    {
        private static Font _configuredFont;

        /// <summary>
        /// Gets the font configured in App.config or <c>null</c> if disabled.
        /// </summary>
        public static Font GetConfiguredFontOrNull()
        {
            if (_configuredFont != null)
                return _configuredFont;

            var enabled = ConfigurationManager.AppSettings["UI.Font.Enabled"];
            if (!string.Equals(enabled, "true", StringComparison.OrdinalIgnoreCase))
                return null;

            var name = ConfigurationManager.AppSettings["UI.Font.Name"] ?? "Malgun Gothic";
            var sizeSetting = ConfigurationManager.AppSettings["UI.Font.Size"];
            float size;
            if (!float.TryParse(sizeSetting, out size))
                size = 12f;

            _configuredFont = new Font(name, size);
            return _configuredFont;
        }

        /// <summary>
        /// Applies the configured font to the provided control and its children.
        /// </summary>
        public static void ApplyFontDeep(Control root)
        {
            var font = GetConfiguredFontOrNull();
            if (font == null || root == null)
                return;

            ApplyControlRecursive(root, font);
        }

        private static void ApplyControlRecursive(Control control, Font font)
        {
            control.Font = font;
            foreach (Control child in control.Controls)
            {
                ApplyControlRecursive(child, font);
            }

            var toolStrip = control as ToolStrip;
            if (toolStrip != null)
            {
                ApplyToolStripItems(toolStrip.Items, font);
            }

            if (control.ContextMenuStrip != null)
            {
                ApplyToolStripItems(control.ContextMenuStrip.Items, font);
            }
        }

        private static void ApplyToolStripItems(ToolStripItemCollection items, Font font)
        {
            foreach (ToolStripItem item in items)
            {
                item.Font = font;

                var dropDownItem = item as ToolStripDropDownItem;
                if (dropDownItem != null)
                {
                    ApplyToolStripItems(dropDownItem.DropDownItems, font);
                }

                var host = item as ToolStripControlHost;
                if (host != null && host.Control != null)
                {
                    ApplyControlRecursive(host.Control, font);
                }
            }
        }
    }
}
