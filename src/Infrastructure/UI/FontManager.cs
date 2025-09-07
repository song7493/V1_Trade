using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace V1_Trade.Infrastructure.UI
{
    /// <summary>
    /// Manages global font preferences for the application.
    /// </summary>
    public static class FontManager
    {
        public static string CurrentFontName { get; private set; } = "Malgun Gothic";
        public static float CurrentFontSize { get; private set; } = 12f;
        public static bool Enabled { get; private set; } = true;

        /// <summary>
        /// Applies current font settings to the provided control and all of its children.
        /// </summary>
        public static void ApplyFontDeep(Control root)
        {
            if (!Enabled || root == null) return;

            var font = new Font(CurrentFontName, CurrentFontSize);
            ApplyFontRecursive(root, font);
        }

        private static void ApplyFontRecursive(Control control, Font font)
        {
            control.Font = font;
            foreach (Control child in control.Controls)
            {
                ApplyFontRecursive(child, font);
            }
        }

        /// <summary>
        /// Updates the current font and applies it to all open forms.
        /// </summary>
        public static void SetFont(string name, float size)
        {
            if (string.Equals(name, CurrentFontName, StringComparison.Ordinal) && Math.Abs(size - CurrentFontSize) < 0.1f)
                return;

            CurrentFontName = name;
            CurrentFontSize = size;
            SaveSettings();

            foreach (Form form in Application.OpenForms.Cast<Form>())
            {
                ApplyFontDeep(form);
            }
        }

        /// <summary>
        /// Loads font settings from configuration.
        /// </summary>
        public static void LoadSettings()
        {
            var enabledValue = ConfigurationManager.AppSettings["UI.Font.Enabled"];
            if (bool.TryParse(enabledValue, out var enabled))
                Enabled = enabled;

            var name = ConfigurationManager.AppSettings["UI.Font.Name"];
            var sizeValue = ConfigurationManager.AppSettings["UI.Font.Size"];
            if (!string.IsNullOrEmpty(name))
                CurrentFontName = name;
            if (float.TryParse(sizeValue, out var parsed))
                CurrentFontSize = parsed;
        }

        /// <summary>
        /// Saves font settings to configuration.
        /// </summary>
        public static void SaveSettings()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("UI.Font.Enabled");
            config.AppSettings.Settings.Remove("UI.Font.Name");
            config.AppSettings.Settings.Remove("UI.Font.Size");
            config.AppSettings.Settings.Add("UI.Font.Enabled", Enabled.ToString().ToLowerInvariant());
            config.AppSettings.Settings.Add("UI.Font.Name", CurrentFontName);
            config.AppSettings.Settings.Add("UI.Font.Size", CurrentFontSize.ToString());
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}

