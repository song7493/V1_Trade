using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Provides helpers to apply a global font across the UI.
    /// </summary>
    public static class FontManager
    {
        public static bool Enabled { get; private set; } = true;
        public static string CurrentFontName { get; private set; } = "Malgun Gothic";
        public static float CurrentFontSize { get; private set; } = 12f;

        public static void LoadSettings()
        {
            var e = ConfigurationManager.AppSettings["UI.Font.Enabled"];
            if (bool.TryParse(e, out var enabled)) Enabled = enabled;
            var n = ConfigurationManager.AppSettings["UI.Font.Name"];
            if (!string.IsNullOrEmpty(n)) CurrentFontName = n;
            var s = ConfigurationManager.AppSettings["UI.Font.Size"];
            if (float.TryParse(s, out var size)) CurrentFontSize = size;
        }

        public static void ApplyFontDeep(Control root)
        {
            if (root == null || !Enabled) return;
            var font = new Font(CurrentFontName, CurrentFontSize);
            ApplyToControl(root, font);
        }

        private static void ApplyToControl(Control c, Font font)
        {
            try
            {
                if (c.Font == null || c.Font.Name != font.Name || Math.Abs(c.Font.Size - font.Size) > 0.01f)
                    c.Font = font;
            }
            catch { }

            foreach (Control child in c.Controls)
                ApplyToControl(child, font);

            if (c.ContextMenuStrip != null)
                ApplyToToolStrip(c.ContextMenuStrip, font);
            if (c is MenuStrip ms)
                ApplyToToolStrip(ms, font);
            if (c is ToolStrip ts)
                ApplyToToolStrip(ts, font);
        }

        private static void ApplyToToolStrip(ToolStrip strip, Font font)
        {
            try { strip.Font = font; } catch { }
            foreach (ToolStripItem item in strip.Items)
                ApplyToToolStripItem(item, font);
        }

        private static void ApplyToToolStripItem(ToolStripItem item, Font font)
        {
            try { item.Font = font; } catch { }
            if (item is ToolStripDropDownItem dd)
                foreach (ToolStripItem sub in dd.DropDownItems)
                    ApplyToToolStripItem(sub, font);
            if (item is ToolStripControlHost host && host.Control != null)
                ApplyToControl(host.Control, font);
        }
    }
}

