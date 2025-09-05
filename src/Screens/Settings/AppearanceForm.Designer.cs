using System.Windows.Forms;
using V1_Trade.App.Ui;

namespace V1_Trade.Screens.Settings
{
    public partial class AppearanceForm : BaseForm
    {
        private ComboBox _fontCombo;
        private NumericUpDown _sizeUpDown;
        private Button _applyButton;
        private Button _okButton;

        private void InitializeComponent()
        {
            _fontCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Left = 15, Top = 15, Width = 200 };
            _sizeUpDown = new NumericUpDown { Left = 225, Top = 15, Width = 60, Minimum = 10, Maximum = 16, Value = 12 };
            _applyButton = new Button { Text = "적용", Left = 15, Top = 50, Width = 80 };
            _okButton = new Button { Text = "확인", Left = 105, Top = 50, Width = 80 };

            _applyButton.Click += ApplyButton_Click;
            _okButton.Click += OkButton_Click;

            Controls.Add(_fontCombo);
            Controls.Add(_sizeUpDown);
            Controls.Add(_applyButton);
            Controls.Add(_okButton);

            Text = "환경설정";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new System.Drawing.Size(300, 90);
        }
    }
}
