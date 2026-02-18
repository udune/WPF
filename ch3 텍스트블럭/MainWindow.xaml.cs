using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ch3_텍스트블럭
{
    public partial class MainWindow : Window
    {
        private int _clickCount;

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<TextBlock>환영합니다</TextBlock>" },
            { "1_2", "<TextBlock Text=\"WPF 학습중\"/>" },
            { "1_3", "private void BtnCount_Click(object sender, RoutedEventArgs e)\n{\n    _count++;\n    tbCounter.Text = $\"클릭 횟수: {_count}회\";\n}" },
            { "1_4", "<TextBlock>첫째줄<LineBreak/>둘째줄<LineBreak/>셋째줄</TextBlock>" },

            // 탭 2: 인라인 요소
            { "2_1", "<TextBlock><Bold>굵게</Bold> <Italic>기울임</Italic></TextBlock>" },
            { "2_2", "<TextBlock><Run Text=\"빨강\" Foreground=\"Red\"/> <Run Text=\"파랑\" Foreground=\"Blue\"/></TextBlock>" },
            { "2_3", "<TextBlock><Span Background=\"Yellow\" FontWeight=\"Bold\">강조</Span></TextBlock>" },

            // 탭 3: 텍스트 레이아웃
            { "3_1", "<TextBlock TextWrapping=\"Wrap\">이것은 매우 긴 텍스트입니다</TextBlock>" },
            { "3_2", "<TextBlock TextTrimming=\"CharacterEllipsis\">이 텍스트는 말줄임 처리됩니다</TextBlock>" },
            { "3_3", "<TextBlock TextAlignment=\"Center\">가운데 정렬</TextBlock>" },

            // 탭 4: 폰트와 스타일
            { "4_1", "<TextBlock FontSize=\"24\">큰 텍스트</TextBlock>" },
            { "4_2", "<TextBlock FontWeight=\"Bold\">굵은 텍스트</TextBlock>" },
            { "4_3", "<TextBlock FontFamily=\"Consolas\">코드 폰트</TextBlock>" },
            { "4_4", "<TextBlock TextDecorations=\"Underline\">밑줄 텍스트</TextBlock>" },

            // 탭 5: 색상과 외관
            { "5_1", "<TextBlock Background=\"LightBlue\" Foreground=\"DarkBlue\">색상 테스트</TextBlock>" },
            { "5_2", "<TextBlock Background=\"Orange\" Opacity=\"0.5\">반투명 텍스트</TextBlock>" },
            { "5_3", "private void BtnApplyStyle_Click(object sender, RoutedEventArgs e)\n{\n    tbTarget.Background = Brushes.Green;\n    tbTarget.Foreground = Brushes.White;\n    tbTarget.FontWeight = FontWeights.Bold;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "1_3", new[] { "_count++", "tbCounter.Text", "클릭 횟수" } },
            { "5_3", new[] { "Brushes.Green", "Brushes.White", "FontWeights.Bold" } },
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnChangeText_Click(object sender, RoutedEventArgs e)
        {
            _clickCount++;
            tbDynamic.Text = $"코드에서 변경된 텍스트 (클릭 {_clickCount}회)";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }

        private void BtnRedStyle_Click(object sender, RoutedEventArgs e)
        {
            tbStyleDemo.Background = Brushes.Tomato;
            tbStyleDemo.Foreground = Brushes.White;
            tbStyleDemo.FontWeight = FontWeights.Bold;
            tbStyleDemo.Text = "빨간 스타일 적용됨";
        }

        private void BtnBlueStyle_Click(object sender, RoutedEventArgs e)
        {
            tbStyleDemo.Background = Brushes.SteelBlue;
            tbStyleDemo.Foreground = Brushes.White;
            tbStyleDemo.FontStyle = FontStyles.Italic;
            tbStyleDemo.Text = "파란 스타일 적용됨";
        }

        private void BtnResetStyle_Click(object sender, RoutedEventArgs e)
        {
            tbStyleDemo.Background = Brushes.Transparent;
            tbStyleDemo.Foreground = Brushes.Black;
            tbStyleDemo.FontWeight = FontWeights.Normal;
            tbStyleDemo.FontStyle = FontStyles.Normal;
            tbStyleDemo.Text = "스타일이 변경됩니다";
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
                    // 여러 루트 요소가 있을 수 있으므로 StackPanel로 감싸기
                    string trimmedCode = xamlCode.TrimStart();
                    if (!trimmedCode.StartsWith("<StackPanel") && 
                        !trimmedCode.StartsWith("<Grid") &&
                        !trimmedCode.StartsWith("<Border") &&
                        !trimmedCode.StartsWith("<Canvas") &&
                        !trimmedCode.StartsWith("<DockPanel") &&
                        !trimmedCode.StartsWith("<WrapPanel"))
                    {
                        fullXaml = $"<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{xamlCode}</StackPanel>";
                    }
                    else
                    {
                        fullXaml = xamlCode.Replace("<TextBlock",
                            "<TextBlock xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
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
    }
}
