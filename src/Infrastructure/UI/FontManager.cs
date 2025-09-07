using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Provides helpers for applying a globally configured font.
    /// </summary>
    public static class FontManager
    {
        /// <summary>
        /// Reads configuration and returns the desired font or null when disabled/invalid.
        /// </summary>
        public static Font GetConfiguredFontOrNull()
        {
            try
            {
                var enabledValue = ConfigurationManager.AppSettings["UI.Font.Enabled"];
                bool enabled;
                if (!bool.TryParse(enabledValue, out enabled) || !enabled)
                    return null;

                var name = ConfigurationManager.AppSettings["UI.Font.Name"];
                var sizeValue = ConfigurationManager.AppSettings["UI.Font.Size"];

                float size;
                if (string.IsNullOrEmpty(name) || !TryParseSize(sizeValue, out size))
                    return null;

                return new Font(name, size);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Recursively applies the configured font to the control tree.
        /// </summary>
        public static void ApplyFontDeep(Control root)
        {
            if (root == null)
                return;

            try
            {
                Font font = GetConfiguredFontOrNull();
                if (font == null)
                    return;

                ApplyFontRecursive(root, font);
            }
            catch
            {
                // ignore
            }
        }

        private static void ApplyFontRecursive(Control control, Font font)
        {
            try
            {
                if (!IsSameFont(control.Font, font))
                    control.Font = font;
            }
            catch
            {
            }

            foreach (Control child in control.Controls)
            {
                try
                {
                    ApplyFontRecursive(child, font);
                }
                catch
                {
                }
            }

            ToolStrip toolStrip = control as ToolStrip;
            if (toolStrip != null)
            {
                foreach (ToolStripItem item in toolStrip.Items)
                {
                    try
                    {
                        ApplyFontRecursive(item, font);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static void ApplyFontRecursive(ToolStripItem item, Font font)
        {
            try
            {
                if (!IsSameFont(item.Font, font))
                    item.Font = font;
            }
            catch
            {
            }

            ToolStripDropDownItem dropDown = item as ToolStripDropDownItem;
            if (dropDown != null)
            {
                foreach (ToolStripItem child in dropDown.DropDownItems)
                {
                    try
                    {
                        ApplyFontRecursive(child, font);
                    }
                    catch
                    {
                    }
                }
            }

            ToolStripControlHost host = item as ToolStripControlHost;
            if (host != null && host.Control != null)
            {
                try
                {
                    ApplyFontRecursive(host.Control, font);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Compares two fonts by name and size.
        /// </summary>
        public static bool IsSameFont(Font a, Font b)
        {
            if (a == null || b == null)
                return false;

            return string.Equals(a.Name, b.Name, StringComparison.Ordinal) &&
                   Math.Abs(a.Size - b.Size) < 0.1f;
        }

        private static bool TryParseSize(string value, out float size)
        {
            size = 0f;
            if (string.IsNullOrEmpty(value))
                return false;

            string trimmed = value.Trim();
            if (trimmed.EndsWith("pt", StringComparison.OrdinalIgnoreCase))
                trimmed = trimmed.Substring(0, trimmed.Length - 2).Trim();

            return float.TryParse(trimmed, out size);
        }
    }
}

