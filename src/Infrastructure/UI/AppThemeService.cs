using System;
using System.Drawing;
using V1_Trade.Infrastructure.Configuration;

namespace V1_Trade.Infrastructure.UI
{
    public sealed class AppThemeService
    {
        public static AppThemeService Instance { get; } = new AppThemeService();

        public Font CurrentFont { get; private set; }

        public event EventHandler ThemeChanged;

        private AppThemeService()
        {
            var name = AppConfig.Get("Ui:FontName", "Malgun Gothic");
            var size = AppConfig.Get("Ui:FontSize", 12f);
            CurrentFont = new Font(name, size);
        }

        public void SetFont(string name, float size)
        {
            if (CurrentFont.Name == name && Math.Abs(CurrentFont.Size - size) < 0.1f)
                return;

            CurrentFont = new Font(name, size);
            AppConfig.Set("Ui:FontName", name);
            AppConfig.Set("Ui:FontSize", size);
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
