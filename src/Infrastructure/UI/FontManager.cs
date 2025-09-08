using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Applies a global font to a control tree.
    /// </summary>
    public static class FontManager
    {
        private const string DefaultFontName = "맑은 고딕";
        private const float DefaultFontSize = 12f;

        public static string CurrentFontName { get; private set; } = DefaultFontName;
        public static float CurrentFontSize { get; private set; } = DefaultFontSize;

        static FontManager()
        {
            try
            {
                var name = ConfigurationManager.AppSettings["UI.Font.Name"];
                if (!string.IsNullOrEmpty(name))
                    CurrentFontName = name;

                var size = ConfigurationManager.AppSettings["UI.Font.Size"];
                if (float.TryParse(size, out var parsedSize))
                    CurrentFontSize = parsedSize;
            }
            catch
            {
                CurrentFontName = DefaultFontName;
                CurrentFontSize = DefaultFontSize;
            }
        }

        /// <summary>
        /// Applies the configured font to the given control and all of its descendants.
        /// </summary>
        public static void Apply(Control root)
        {
            if (root == null)
                return;

            ApplyToControl(root, CurrentFontName, CurrentFontSize);
        }

        public static void SetFont(string fontName, float fontSize)
        {
            CurrentFontName = fontName;
            CurrentFontSize = fontSize;

            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("UI.Font.Name");
                config.AppSettings.Settings.Remove("UI.Font.Size");
                config.AppSettings.Settings.Add("UI.Font.Name", fontName);
                config.AppSettings.Settings.Add("UI.Font.Size", fontSize.ToString());
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch { }

            foreach (Form form in Application.OpenForms)
                Apply(form);
        }

        private static void ApplyToControl(Control control, string fontName, float fontSize)
        {
            if (control == null)
                return;

            try
            {
                var style = control.Font?.Style ?? FontStyle.Regular;
                control.Font = new Font(fontName, fontSize, style);
            }
            catch { }

            foreach (Control child in control.Controls)
                ApplyToControl(child, fontName, fontSize);

            if (control.ContextMenuStrip != null)
                ApplyToToolStrip(control.ContextMenuStrip, fontName, fontSize);
            if (control is MenuStrip ms)
                ApplyToToolStrip(ms, fontName, fontSize);
            if (control is ToolStrip ts)
                ApplyToToolStrip(ts, fontName, fontSize);
        }

        private static void ApplyToToolStrip(ToolStrip strip, string fontName, float fontSize)
        {
            try
            {
                var style = strip.Font?.Style ?? FontStyle.Regular;
                strip.Font = new Font(fontName, fontSize, style);
            }
            catch { }

            foreach (ToolStripItem item in strip.Items)
                ApplyToToolStripItem(item, fontName, fontSize);
        }

        private static void ApplyToToolStripItem(ToolStripItem item, string fontName, float fontSize)
        {
            try
            {
                var style = item.Font?.Style ?? FontStyle.Regular;
                item.Font = new Font(fontName, fontSize, style);
            }
            catch { }

            if (item is ToolStripDropDownItem dd)
                foreach (ToolStripItem sub in dd.DropDownItems)
                    ApplyToToolStripItem(sub, fontName, fontSize);
            if (item is ToolStripControlHost host && host.Control != null)
                ApplyToControl(host.Control, fontName, fontSize);
        }
    }
}
