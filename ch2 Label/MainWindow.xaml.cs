using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch2_Label
{
    public partial class MainWindow : Window
    {
        private int _clickCount;

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_1", "<Label>반갑습니다</Label>" },
            { "1_2", "<Label Content=\"WPF 학습중\"/>" },
            { "1_3", "private void BtnCount_Click(object sender, RoutedEventArgs e)\n{\n    _count++;\n    lblCounter.Content = $\"클릭 횟수: {_count}회\";\n}" },
            { "1_4", "<Label Content=\"도움말\" ToolTip=\"클릭하면 도움말이 열립니다\"/>" },
            { "2_1", "<Label HorizontalContentAlignment=\"Center\" Background=\"LightBlue\">가운데 정렬</Label>" },
            { "2_2", "<Label HorizontalAlignment=\"Right\" Background=\"Gold\">오른쪽 정렬</Label>" },
            { "2_3", "<Label VerticalContentAlignment=\"Bottom\" Height=\"80\" Width=\"120\" Background=\"LightPink\">아래 정렬</Label>" },
            { "3_1", "<Label FontSize=\"30\">큰 글자</Label>" },
            { "3_2", "<Label FontWeight=\"Bold\" FontSize=\"20\">굵은 텍스트</Label>" },
            { "3_3", "<Label FontStyle=\"Italic\">기울임 텍스트</Label>" },
            { "3_4", "<Label FontFamily=\"Consolas\" FontSize=\"18\">Code Style</Label>" },
            { "4_1", "<Label Content=\"경고 메시지\" Background=\"Red\" Foreground=\"White\"/>" },
            { "4_2", "<Label Content=\"테두리 연습\" BorderBrush=\"Green\" BorderThickness=\"2\" Padding=\"10\"/>" },
            { "4_3", "<Label Content=\"반투명\" Background=\"Orange\" Opacity=\"0.5\"/>" },
            { "4_4", "<Label Content=\"여백 테스트\" Background=\"LightGreen\" Padding=\"15\"/>" },
            { "4_5", "<Label Content=\"마진 연습\" Background=\"Yellow\" Margin=\"15\"/>" },
            // 탭 5: 코드 비교 방식
            { "5_1", "<Label Content=\"_Password\" Target=\"{Binding ElementName=txtPassword}\"/>\n<TextBox x:Name=\"txtPassword\" Width=\"200\"/>" },
            { "5_2", "private void BtnGreenStyle_Click(object sender, RoutedEventArgs e)\n{\n    lblStyleDemo.Background = Brushes.Green;\n    lblStyleDemo.Foreground = Brushes.Yellow;\n    lblStyleDemo.FontWeight = FontWeights.Bold;\n    lblStyleDemo.Content = \"스타일 변경됨\";\n}" },
            { "5_3", "private void BtnToggle_Click(object sender, RoutedEventArgs e)\n{\n    if (lblMessage.Visibility == Visibility.Visible)\n    {\n        lblMessage.Visibility = Visibility.Collapsed;\n    }\n    else\n    {\n        lblMessage.Visibility = Visibility.Visible;\n    }\n}" }
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            // 탭 1: 코드 비하인드 동적 설정
            { "1_3", new[] { "_count++", "lblCounter.Content", "클릭 횟수" } },
            // 탭 5: Target과 상호작용
            { "5_1", new[] { "_Password", "txtPassword", "ElementName", "Target" } },
            { "5_2", new[] { "Brushes.Green", "Brushes.Yellow", "FontWeights.Bold", "lblStyleDemo.Content", "스타일 변경됨" } },
            { "5_3", new[] { "Visibility.Visible", "Visibility.Collapsed", "if", "else" } }
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
                // XAML 네임스페이스 추가
                string fullXaml = xamlCode;
                if (!xamlCode.Contains("xmlns="))
                {
                    fullXaml = xamlCode.Replace("<Label",
                        "<Label xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
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

        #endregion

        #region 기본 사용법 탭 - 코드 비하인드 예제

        private void BtnChangeText_Click(object sender, RoutedEventArgs e)
        {
            _clickCount++;
            lblDynamic.Content = $"코드에서 변경된 텍스트 (클릭 {_clickCount}회)";
        }

        #endregion

        #region Target과 상호작용 탭 - 스타일 변경 예제

        private void BtnRedStyle_Click(object sender, RoutedEventArgs e)
        {
            lblStyleDemo.Background = Brushes.Tomato;
            lblStyleDemo.Foreground = Brushes.White;
            lblStyleDemo.FontWeight = FontWeights.Bold;
            lblStyleDemo.FontStyle = FontStyles.Normal;
            lblStyleDemo.Content = "빨간 스타일 적용됨";
        }

        private void BtnBlueStyle_Click(object sender, RoutedEventArgs e)
        {
            lblStyleDemo.Background = Brushes.SteelBlue;
            lblStyleDemo.Foreground = Brushes.White;
            lblStyleDemo.FontWeight = FontWeights.Normal;
            lblStyleDemo.FontStyle = FontStyles.Italic;
            lblStyleDemo.Content = "파란 스타일 적용됨";
        }

        private void BtnResetStyle_Click(object sender, RoutedEventArgs e)
        {
            lblStyleDemo.Background = Brushes.Transparent;
            lblStyleDemo.Foreground = Brushes.Black;
            lblStyleDemo.FontWeight = FontWeights.Normal;
            lblStyleDemo.FontStyle = FontStyles.Normal;
            lblStyleDemo.Content = "스타일이 변경됩니다";
        }

        #endregion

        #region Target과 상호작용 탭 - Visibility 예제

        private void BtnVisible_Click(object sender, RoutedEventArgs e)
        {
            lblVisibility.Visibility = Visibility.Visible;
        }

        private void BtnHidden_Click(object sender, RoutedEventArgs e)
        {
            lblVisibility.Visibility = Visibility.Hidden;
        }

        private void BtnCollapsed_Click(object sender, RoutedEventArgs e)
        {
            lblVisibility.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}
