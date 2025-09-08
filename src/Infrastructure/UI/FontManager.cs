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

        /// <summary>
        /// The name of the font currently applied to the UI.
        /// </summary>
        public static string CurrentFontName { get; private set; } = DefaultFontName;

        /// <summary>
        /// The size of the font currently applied to the UI.
        /// </summary>
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

        /// <summary>
        /// Sets the global font and optionally persists it to configuration.
        /// </summary>
        public static void SetFont(string name, float size, bool save)
        {
            if (string.IsNullOrEmpty(name))
                return;

            CurrentFontName = name;
            CurrentFontSize = size;

            if (save)
            {
                try
                {
                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    var settings = config.AppSettings.Settings;
                    if (settings["UI.Font.Name"] == null)
                        settings.Add("UI.Font.Name", name);
                    else
                        settings["UI.Font.Name"].Value = name;

                    if (settings["UI.Font.Size"] == null)
                        settings.Add("UI.Font.Size", size.ToString());
                    else
                        settings["UI.Font.Size"].Value = size.ToString();

                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                catch
                {
                }
            }

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
