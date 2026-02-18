using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch6_패스워드박스
{
    public partial class MainWindow : Window
    {
        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<PasswordBox PasswordChar=\"*\"/>" },
            { "1_2", "<PasswordBox MaxLength=\"6\"/>" },
            { "1_3", "<PasswordBox Password=\"test1234\" PasswordChar=\"#\"/>" },

            // 탭 2: 정렬
            { "2_1", "<PasswordBox HorizontalAlignment=\"Center\" Width=\"250\"/>" },
            { "2_2", "<PasswordBox HorizontalContentAlignment=\"Right\"/>" },

            // 탭 3: 폰트와 스타일
            { "3_1", "<PasswordBox FontSize=\"20\"/>" },
            { "3_2", "<PasswordBox FontWeight=\"Bold\" FontSize=\"16\"/>" },
            { "3_3", "<PasswordBox FontFamily=\"Consolas\"/>" },

            // 탭 4: 색상과 외관
            { "4_1", "<PasswordBox Background=\"LightBlue\" Foreground=\"DarkBlue\"/>" },
            { "4_2", "<PasswordBox BorderBrush=\"Red\" BorderThickness=\"2\"/>" },
            { "4_3", "<PasswordBox Opacity=\"0.5\" Background=\"Orange\"/>" },

            // 탭 5: 이벤트와 상호작용 (코드 비교)
            { "5_1", "private void Pwd_PasswordChanged(object sender, RoutedEventArgs e)\n{\n    txtLength.Text = pwdBox.Password.Length.ToString();\n}" },
            { "5_2", "private void CheckStrength(string password)\n{\n    if (password.Length < 4)\n        txtStrengthResult.Text = \"약함\";\n    else if (password.Length < 8)\n        txtStrengthResult.Text = \"보통\";\n    else\n        txtStrengthResult.Text = \"강함\";\n}" },
            { "5_3", "private bool CheckMatch(string pwd1, string pwd2)\n{\n    return pwd1 == pwd2;\n}" }
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { ".Password", ".Length", "txtLength" } },
            { "5_2", new[] { "Length", "약함", "보통", "강함", "if", "else" } },
            { "5_3", new[] { "return", "==", "pwd1", "pwd2" } }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        #region 직접 해보기 이벤트 핸들러

        // XAML 실행 버튼
        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var txtPractice = FindName($"txtPractice{tag}") as TextBox;
                var resultPanel = FindName($"resultPanel{tag}") as StackPanel;
                var resultBorder = FindName($"resultBorder{tag}") as Border;

                if (txtPractice != null && resultPanel != null && resultBorder != null)
                {
                    ExecuteXaml(txtPractice.Text, resultPanel, resultBorder);
                }
            }
        }

        // 힌트 토글 버튼
        private void BtnHint_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var txtHint = FindName($"txtHint{tag}") as TextBlock;
                if (txtHint != null)
                {
                    txtHint.Visibility = txtHint.Visibility == Visibility.Visible
                        ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        // 정답 보기 버튼
        private void BtnAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var txtPractice = FindName($"txtPractice{tag}") as TextBox;
                if (txtPractice != null && _answers.TryGetValue(tag, out var answer))
                {
                    txtPractice.Text = answer;
                }
            }
        }

        // 코드 비교 확인 버튼
        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var txtPractice = FindName($"txtPractice{tag}") as TextBox;
                var txtResult = FindName($"txtResult{tag}") as TextBlock;

                if (txtPractice != null && txtResult != null && _requiredKeywords.TryGetValue(tag, out var keywords))
                {
                    txtResult.Visibility = Visibility.Visible;
                    string userCode = txtPractice.Text;

                    // 모든 필수 키워드가 포함되어 있는지 확인
                    var missingKeywords = keywords.Where(k => !userCode.Contains(k)).ToList();

                    if (missingKeywords.Count == 0)
                    {
                        txtResult.Text = "정답입니다! 모든 필수 요소가 포함되어 있습니다.";
                        txtResult.Foreground = Brushes.Green;
                    }
                    else
                    {
                        txtResult.Text = $"다시 확인해보세요. 누락된 요소: {string.Join(", ", missingKeywords)}";
                        txtResult.Foreground = Brushes.Red;
                    }
                }
            }
        }

        // XAML 실행 메서드
        private void ExecuteXaml(string xamlCode, StackPanel resultPanel, Border resultBorder)
        {
            resultPanel.Children.Clear();
            resultBorder.Visibility = Visibility.Visible;

            try
            {
                string fullXaml = xamlCode;
                if (!xamlCode.Contains("xmlns="))
                {
                    // PasswordBox에 네임스페이스 추가
                    fullXaml = xamlCode.Replace("<PasswordBox",
                        "<PasswordBox xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                }

                var element = XamlReader.Parse(fullXaml) as UIElement;
                if (element != null)
                {
                    resultPanel.Children.Add(element);
                    resultPanel.Children.Add(new TextBlock
                    {
                        Text = "성공적으로 실행되었습니다!",
                        Foreground = Brushes.Green,
                        Margin = new Thickness(0, 10, 0, 0)
                    });
                }
            }
            catch (Exception ex)
            {
                resultPanel.Children.Add(new TextBlock
                {
                    Text = $"오류: {ex.Message}",
                    Foreground = Brushes.Red,
                    TextWrapping = TextWrapping.Wrap
                });
            }
        }

        #endregion

        #region 기존 이벤트 핸들러

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

        #endregion
    }
}
