using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Provides helpers for applying configured fonts across controls.
    /// </summary>
    public static class FontManager
    {
        private static Font _configuredFont;
        private const string EnabledKey = "UI.Font.Enabled";
        private const string NameKey = "UI.Font.Name";
        private const string SizeKey = "UI.Font.Size";

        /// <summary>
        /// Retrieves the font configured in app settings or returns null if disabled.
        /// </summary>
        public static Font GetConfiguredFontOrNull()
        {
            if (_configuredFont != null)
                return _configuredFont;

            if (!bool.TryParse(ConfigurationManager.AppSettings[EnabledKey], out var enabled) || !enabled)
                return null;

            var name = ConfigurationManager.AppSettings[NameKey];
            var sizeValue = ConfigurationManager.AppSettings[SizeKey];
            if (string.IsNullOrEmpty(name) || !float.TryParse(sizeValue, out var size))
                return null;

            _configuredFont = new Font(name, size);
            return _configuredFont;
        }

        /// <summary>
        /// Applies the configured font to the provided control and all descendants.
        /// </summary>
        public static void ApplyFontDeep(Control root)
        {
            var font = GetConfiguredFontOrNull();
            if (font == null || root == null)
                return;

            ApplyToControl(root, font);
        }

        private static void ApplyToControl(Control control, Font font)
        {
            control.Font = font;

            if (control is MenuStrip menuStrip)
            {
                foreach (ToolStripItem item in menuStrip.Items)
                    ApplyToToolStripItem(item, font);
            }

            foreach (Control child in control.Controls)
            {
                if (child is ToolStrip strip)
                {
                    ApplyToToolStrip(strip, font);
                }
                else
                {
                    ApplyToControl(child, font);
                }
            }
        }

        private static void ApplyToToolStrip(ToolStrip strip, Font font)
        {
            strip.Font = font;
            foreach (ToolStripItem item in strip.Items)
                ApplyToToolStripItem(item, font);
        }

        private static void ApplyToToolStripItem(ToolStripItem item, Font font)
        {
            item.Font = font;

            if (item is ToolStripDropDownItem dropDown)
            {
                foreach (ToolStripItem sub in dropDown.DropDownItems)
                    ApplyToToolStripItem(sub, font);
            }
            else if (item is ToolStripControlHost host && host.Control != null)
            {
                ApplyToControl(host.Control, font);
            }
        }
    }
}

