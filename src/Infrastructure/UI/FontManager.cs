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
    public sealed class FontManager
    {
        private static readonly Lazy<FontManager> _instance = new Lazy<FontManager>(() => new FontManager());

        public static FontManager Instance => _instance.Value;

        private FontManager()
        {
            LoadSettings();
        }

        public string CurrentFontName { get; private set; } = "Malgun Gothic";
        public float CurrentFontSize { get; private set; } = 12f;

        public event EventHandler FontChanged;

        /// <summary>
        /// Applies current font settings to the provided control and all of its children.
        /// </summary>
        public void ApplyFontDeep(Control root)
        {
            if (root == null) return;

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
        /// Updates the current font and notifies subscribers.
        /// </summary>
        public void SetFont(string name, float size)
        {
            if (string.Equals(name, CurrentFontName, StringComparison.Ordinal) && Math.Abs(size - CurrentFontSize) < 0.1f)
                return;

            CurrentFontName = name;
            CurrentFontSize = size;
            SaveSettings();
            FontChanged?.Invoke(this, EventArgs.Empty);
            // Apply to all open forms.
            foreach (Form form in Application.OpenForms.Cast<Form>())
            {
                ApplyFontDeep(form);
            }
        }

        /// <summary>
        /// Loads font settings from configuration.
        /// </summary>
        public void LoadSettings()
        {
            var name = ConfigurationManager.AppSettings["Ui.Font.Name"];
            var sizeValue = ConfigurationManager.AppSettings["Ui.Font.Size"];
            if (!string.IsNullOrEmpty(name))
                CurrentFontName = name;
            if (float.TryParse(sizeValue, out var parsed))
                CurrentFontSize = parsed;
        }

        /// <summary>
        /// Saves font settings to configuration.
        /// </summary>
        public void SaveSettings()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("Ui.Font.Name");
            config.AppSettings.Settings.Remove("Ui.Font.Size");
            config.AppSettings.Settings.Add("Ui.Font.Name", CurrentFontName);
            config.AppSettings.Settings.Add("Ui.Font.Size", CurrentFontSize.ToString());
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}

