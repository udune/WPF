using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ch6_패스워드박스
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // PasswordChanged 이벤트
        private void PwdInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPwdLength != null && pwdInput != null)
                txtPwdLength.Text = $"입력된 글자 수: {pwdInput.Password.Length}";
        }

        // 비밀번호 강도 체크
        private void PwdStrength_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtStrength == null || pwdStrength == null) return;

            string pw = pwdStrength.Password;
            if (pw.Length == 0)
                txtStrength.Text = "비밀번호 강도: -";
            else if (pw.Length < 4)
                txtStrength.Text = "비밀번호 강도: 약함";
            else if (pw.Length < 8)
                txtStrength.Text = "비밀번호 강도: 보통";
            else
                txtStrength.Text = "비밀번호 강도: 강함";
        }

        // GotFocus / LostFocus 이벤트
        private void PwdFocus_GotFocus(object sender, RoutedEventArgs e)
        {
            pwdFocus.Background = Brushes.LightYellow;
            pwdFocus.BorderBrush = Brushes.Orange;
            txtPwdFocusStatus.Text = "포커스 상태: 획득 (GotFocus)";
        }

        private void PwdFocus_LostFocus(object sender, RoutedEventArgs e)
        {
            pwdFocus.ClearValue(PasswordBox.BackgroundProperty);
            pwdFocus.ClearValue(PasswordBox.BorderBrushProperty);
            txtPwdFocusStatus.Text = "포커스 상태: 해제 (LostFocus)";
        }

        // KeyDown 이벤트
        private void PwdKeyDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtPwdKeyResult.Text = $"입력된 비밀번호: {pwdKeyDown.Password}";
            }
        }

        // 비밀번호 일치 검사
        private void BtnCheckMatch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwdFirst.Password))
                txtMatchResult.Text = "비밀번호를 입력해주세요.";
            else if (pwdFirst.Password == pwdSecond.Password)
                txtMatchResult.Text = "비밀번호가 일치합니다!";
            else
                txtMatchResult.Text = "비밀번호가 일치하지 않습니다.";
        }
    }
}
