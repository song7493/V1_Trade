using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Manages global font preferences for the application.
    /// </summary>
    public static class FontManager
    {
        public static bool Enabled { get; private set; } = true;
        public static string CurrentFontName { get; private set; } = "Malgun Gothic";
        public static float CurrentFontSize { get; private set; } = 12f;

        /// <summary>
        /// Loads font settings from configuration.
        /// </summary>
        public static void LoadSettings()
        {
            var enabledValue = ConfigurationManager.AppSettings["UI.Font.Enabled"];
            if (bool.TryParse(enabledValue, out var enabled))
            {
                Enabled = enabled;
            }

            var name = ConfigurationManager.AppSettings["UI.Font.Name"];
            if (!string.IsNullOrEmpty(name))
            {
                CurrentFontName = name;
            }

            var sizeValue = ConfigurationManager.AppSettings["UI.Font.Size"];
            if (float.TryParse(sizeValue, out var size))
            {
                CurrentFontSize = size;
            }
        }

        /// <summary>
        /// Applies current font settings to the provided control and all of its children.
        /// </summary>
        public static void ApplyFontDeep(Control root)
        {
            if (root == null || !Enabled) return;

            var font = new Font(CurrentFontName, CurrentFontSize);
            ApplyFontRecursive(root, font);
        }

        private static void ApplyFontRecursive(Control control, Font font)
        {
            control.Font = font;

            if (control.ContextMenuStrip != null)
            {
                ApplyFont(control.ContextMenuStrip.Items, font);
            }

            if (control is MenuStrip menuStrip)
            {
                ApplyFont(menuStrip.Items, font);
            }

            if (control is ToolStrip toolStrip)
            {
                ApplyFont(toolStrip.Items, font);
            }

            foreach (Control child in control.Controls)
            {
                ApplyFontRecursive(child, font);
            }
        }

        private static void ApplyFont(ToolStripItemCollection items, Font font)
        {
            foreach (ToolStripItem item in items)
            {
                item.Font = font;

                if (item is ToolStripControlHost host && host.Control != null)
                {
                    ApplyFontRecursive(host.Control, font);
                }

                if (item is ToolStripDropDownItem dropDownItem && dropDownItem.HasDropDownItems)
                {
                    ApplyFont(dropDownItem.DropDownItems, font);
                }
            }
        }
    }
}

