using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch4_버튼
{
    public partial class MainWindow : Window
    {
        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<Button>클릭하세요</Button>" },
            { "1_2", "<Button Content=\"저장하기\"/>" },
            { "1_3", "<Button Content=\"도움말\" ToolTip=\"도움말을 표시합니다\"/>" },
            { "1_4", "<Button Content=\"수정 불가\" IsEnabled=\"False\"/>" },
            // 탭 2: 정렬
            { "2_1", "<Button Content=\"오른쪽 정렬\" HorizontalAlignment=\"Right\"/>" },
            { "2_2", "<Button Content=\"큰 버튼\" Width=\"180\" Height=\"60\"/>" },
            // 탭 3: 폰트와 스타일
            { "3_1", "<Button Content=\"큰 글자 버튼\" FontSize=\"24\"/>" },
            { "3_2", "<Button Content=\"강조 버튼\" FontWeight=\"Bold\" FontSize=\"18\"/>" },
            // 탭 4: 색상과 외관
            { "4_1", "<Button Content=\"경고\" Background=\"Red\" Foreground=\"White\"/>" },
            { "4_2", "<Button Content=\"넓은 버튼\" Padding=\"20\" Background=\"LightBlue\"/>" },
            // 탭 5: 이벤트와 상호작용 (코드 비교)
            { "5_1", "private void BtnCount_Click(object sender, RoutedEventArgs e)\n{\n    _count++;\n    lblStatus.Content = $\"클릭: {_count}회\";\n}" },
            { "5_2", "private void Btn_MouseEnter(object sender, MouseEventArgs e)\n{\n    btnTarget.Background = Brushes.Yellow;\n    btnTarget.Foreground = Brushes.Blue;\n}" },
            { "5_3", "private void BtnApplyStyle_Click(object sender, RoutedEventArgs e)\n{\n    btnTarget.Background = Brushes.Green;\n    btnTarget.Foreground = Brushes.White;\n    btnTarget.FontWeight = FontWeights.Bold;\n    btnTarget.Content = \"스타일 적용됨\";\n}" },
            { "5_4", "private void BtnToggle_Click(object sender, RoutedEventArgs e)\n{\n    if (btnTarget.Visibility == Visibility.Visible)\n    {\n        btnTarget.Visibility = Visibility.Collapsed;\n    }\n    else\n    {\n        btnTarget.Visibility = Visibility.Visible;\n    }\n}" }
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { "_count++", "lblStatus.Content", "클릭" } },
            { "5_2", new[] { "Brushes.Yellow", "Brushes.Blue", "btnTarget.Background", "btnTarget.Foreground" } },
            { "5_3", new[] { "Brushes.Green", "Brushes.White", "FontWeights.Bold", "btnTarget.Content", "스타일 적용됨" } },
            { "5_4", new[] { "Visibility.Visible", "Visibility.Collapsed", "if", "else" } }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        #region 직접 해보기 - XAML 실행 기능

        private void ExecuteXaml(string xamlCode, StackPanel resultPanel, Border resultBorder)
        {
            resultPanel.Children.Clear();
            resultBorder.Visibility = Visibility.Visible;

            try
            {
                string fullXaml = xamlCode;
                if (!xamlCode.Contains("xmlns="))
                {
                    fullXaml = xamlCode.Replace("<Button",
                        "<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
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

        #endregion

        // Click 이벤트
        private int _clickCount;
        private void BtnClick_Click(object sender, RoutedEventArgs e)
        {
            _clickCount++;
            txtClickResult.Text = $"버튼을 {_clickCount}번 클릭했습니다";
        }

        // MouseDoubleClick 이벤트
        private void BtnDouble_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtDoubleClick.Background = Brushes.Salmon;
            txtDoubleClick.Foreground = Brushes.White;
            txtDoubleClick.Text = "더블클릭 감지!";
        }

        // MouseEnter / MouseLeave 이벤트
        private void BtnHover_MouseEnter(object sender, MouseEventArgs e)
        {
            btnHover.Foreground = Brushes.Red;
            btnHover.Content = "마우스 진입!";
        }

        private void BtnHover_MouseLeave(object sender, MouseEventArgs e)
        {
            btnHover.Foreground = Brushes.Black;
            btnHover.Content = "마우스를 올려보세요";
        }

        // 동적 스타일 변경
        private void BtnRedStyle_Click(object sender, RoutedEventArgs e)
        {
            btnStyleTarget.Background = Brushes.Tomato;
            btnStyleTarget.Foreground = Brushes.White;
            btnStyleTarget.FontWeight = FontWeights.Bold;
            btnStyleTarget.Content = "빨간 스타일 적용됨";
        }

        private void BtnBlueStyle_Click(object sender, RoutedEventArgs e)
        {
            btnStyleTarget.Background = Brushes.SteelBlue;
            btnStyleTarget.Foreground = Brushes.White;
            btnStyleTarget.FontStyle = FontStyles.Italic;
            btnStyleTarget.Content = "파란 스타일 적용됨";
        }

        private void BtnResetStyle_Click(object sender, RoutedEventArgs e)
        {
            btnStyleTarget.ClearValue(Button.BackgroundProperty);
            btnStyleTarget.ClearValue(Button.ForegroundProperty);
            btnStyleTarget.FontWeight = FontWeights.Normal;
            btnStyleTarget.FontStyle = FontStyles.Normal;
            btnStyleTarget.Content = "스타일이 변경됩니다";
        }

        // Visibility 제어
        private void BtnVisible_Click(object sender, RoutedEventArgs e)
        {
            btnVisibility.Visibility = Visibility.Visible;
        }

        private void BtnHidden_Click(object sender, RoutedEventArgs e)
        {
            btnVisibility.Visibility = Visibility.Hidden;
        }

        private void BtnCollapsed_Click(object sender, RoutedEventArgs e)
        {
            btnVisibility.Visibility = Visibility.Collapsed;
        }
    }
}
