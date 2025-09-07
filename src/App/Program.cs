using System;
using System.Windows.Forms;
using V1_Trade.Infrastructure.UI;

namespace V1_Trade.App
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Warm-up font configuration to ensure settings are loaded early.
            var _ = FontManager.GetConfiguredFontOrNull();

            Application.Run(new MainForm());
        }
    }
}
