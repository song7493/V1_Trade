using System;
using System.Drawing;
using V1_Trade.Infrastructure.Config;

namespace V1_Trade.Infrastructure.UI
{
    public sealed class AppThemeService
    {
        private readonly Config _config = new Config();

        public static AppThemeService Instance { get; } = new AppThemeService();

        public Font CurrentFont { get; private set; }

        public event EventHandler ThemeChanged;

        private AppThemeService()
        {
            var name = _config.Get("Ui:FontName", "Malgun Gothic");
            var size = _config.Get("Ui:FontSize", 12f);
            CurrentFont = new Font(name, size);
        }

        public void SetFont(string name, float size)
        {
            if (CurrentFont.Name == name && Math.Abs(CurrentFont.Size - size) < 0.1f)
                return;

            CurrentFont = new Font(name, size);
            _config.Set("Ui:FontName", name);
            _config.Set("Ui:FontSize", size);
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
