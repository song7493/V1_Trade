using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Provides helpers for applying the globally configured font to controls.
    /// </summary>
    public static class FontManager
    {
        private static Font _cachedFont;
        private static bool _fontLoaded;

        /// <summary>
        /// Reads the configured font from configuration files.
        /// Returns <c>null</c> if parsing fails or the font cannot be created.
        /// </summary>
        public static Font GetConfiguredFontOrNull()
        {
            if (_fontLoaded)
            {
                return _cachedFont;
            }

            _fontLoaded = true;

            try
            {
                string name = null;
                string sizeValue = null;

                // 1) Try external config file.
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var externalPath = Path.Combine(baseDir, "환경 설정.config");
                if (File.Exists(externalPath))
                {
                    foreach (var line in File.ReadAllLines(externalPath))
                    {
                        var parts = line.Split(new[] { '=' }, 2);
                        if (parts.Length != 2)
                            continue;
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        if (key == "UI.Font.Name")
                            name = value;
                        else if (key == "UI.Font.Size")
                            sizeValue = value;
                    }
                }

                // 2) Fallback to App.config.
                if (name == null)
                    name = ConfigurationManager.AppSettings["UI.Font.Name"];
                if (sizeValue == null)
                    sizeValue = ConfigurationManager.AppSettings["UI.Font.Size"];

                if (string.IsNullOrWhiteSpace(name))
                    return null;

                float size;
                if (!TryParseSize(sizeValue, out size))
                    return null;

                var font = new Font(name, size);
                _cachedFont = font;
                return font;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Applies the configured font to a control and all of its child controls.
        /// </summary>
        public static void ApplyFont(Control root)
        {
            try
            {
                var font = GetConfiguredFontOrNull();
                if (font == null || root == null)
                    return;

                ApplyToControl(root, font);
            }
            catch
            {
                // Swallow silently as requested.
            }
        }

        /// <summary>
        /// Applies the configured font to a ToolStrip and its items.
        /// </summary>
        public static void ApplyFont(ToolStrip strip)
        {
            try
            {
                var font = GetConfiguredFontOrNull();
                if (font == null || strip == null)
                    return;

                if (!IsSameFont(strip.Font, font))
                    strip.Font = font;

                foreach (ToolStripItem item in strip.Items)
                {
                    ApplyToItem(item, font);
                }
            }
            catch
            {
                // Ignore.
            }
        }

        private static void ApplyToControl(Control control, Font font)
        {
            if (control is ToolStrip toolStrip)
            {
                ApplyFont(toolStrip);
                return;
            }

            if (!IsSameFont(control.Font, font))
                control.Font = font;

            foreach (Control child in control.Controls)
            {
                ApplyToControl(child, font);
            }
        }

        private static void ApplyToItem(ToolStripItem item, Font font)
        {
            if (item == null)
                return;

            if (!IsSameFont(item.Font, font))
                item.Font = font;

            var dropDownItem = item as ToolStripDropDownItem;
            if (dropDownItem != null)
            {
                foreach (ToolStripItem child in dropDownItem.DropDownItems)
                {
                    ApplyToItem(child, font);
                }
            }

            var host = item as ToolStripControlHost;
            if (host != null && host.Control != null)
            {
                ApplyToControl(host.Control, font);
            }
        }

        private static bool IsSameFont(Font a, Font b)
        {
            if (a == null || b == null)
                return false;

            return string.Equals(a.Name, b.Name, StringComparison.Ordinal) && Math.Abs(a.Size - b.Size) < 0.1f;
        }

        private static bool TryParseSize(string value, out float size)
        {
            size = 0f;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var trimmed = value.Trim();
            if (trimmed.EndsWith("pt", StringComparison.OrdinalIgnoreCase))
            {
                trimmed = trimmed.Substring(0, trimmed.Length - 2);
            }

            return float.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out size);
        }
    }
}
