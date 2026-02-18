using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch5_텍스트박스
{
    public partial class MainWindow : Window
    {
        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<TextBox Text=\"안녕하세요\"/>" },
            { "1_2", "<TextBox Text=\"여러 줄 입력\" TextWrapping=\"Wrap\" AcceptsReturn=\"True\"/>" },
            { "1_3", "<TextBox MaxLength=\"5\"/>" },
            { "1_4", "<TextBox Text=\"읽기 전용\" IsReadOnly=\"True\"/>" },

            // 탭 2: 정렬
            { "2_1", "<TextBox Text=\"가운데\" TextAlignment=\"Center\"/>" },
            { "2_2", "<TextBox Text=\"세로 가운데\" Height=\"50\" VerticalContentAlignment=\"Center\"/>" },
            { "2_3", "<TextBox Text=\"고정 크기\" Width=\"300\" Height=\"40\"/>" },

            // 탭 3: 폰트와 스타일
            { "3_1", "<TextBox Text=\"큰 글씨\" FontSize=\"20\"/>" },
            { "3_2", "<TextBox Text=\"굵은 기울임\" FontWeight=\"Bold\" FontStyle=\"Italic\"/>" },
            { "3_3", "<TextBox Text=\"Console.WriteLine()\" FontFamily=\"Consolas\"/>" },

            // 탭 4: 색상과 외관
            { "4_1", "<TextBox Text=\"색상 테스트\" Background=\"LightYellow\" Foreground=\"DarkOrange\"/>" },
            { "4_2", "<TextBox Text=\"테두리\" BorderBrush=\"Blue\" BorderThickness=\"2\"/>" },
            { "4_3", "<TextBox Text=\"패딩과 투명도\" Background=\"SkyBlue\" Padding=\"15\" Opacity=\"0.7\"/>" },

            // 탭 5: 이벤트와 상호작용
            { "5_1", "private void TxtInput_TextChanged(object sender, TextChangedEventArgs e)\n{\n    txtCount.Text = $\"글자 수: {txtInput.Text.Length}\";\n}" },
            { "5_2", "private void TxtBox_GotFocus(object sender, RoutedEventArgs e)\n{\n    txtBox.Background = Brushes.LightYellow;\n}" },
            { "5_3", "private void TxtBox_KeyDown(object sender, KeyEventArgs e)\n{\n    if (e.Key == Key.Enter)\n    {\n        txtResult.Text = txtBox.Text;\n    }\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { ".Text.Length", "txtCount.Text" } },
            { "5_2", new[] { "Brushes.LightYellow", "Background" } },
            { "5_3", new[] { "Key.Enter", "if", "e.Key" } },
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        // TextChanged 이벤트
        private void TxtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCharCount != null && txtInput != null)
                txtCharCount.Text = $"글자 수: {txtInput.Text.Length}";
        }

        // SelectionChanged 이벤트
        private void TxtSelection_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string selected = txtSelection.SelectedText;
            if (txtSelectedText != null && txtInput != null)
            {
                txtSelectedText.Text = string.IsNullOrEmpty(selected)
                ? "선택된 텍스트: (없음)"
                : $"선택된 텍스트: {selected}";
            }
        }

        // GotFocus / LostFocus 이벤트
        private void TxtFocus_GotFocus(object sender, RoutedEventArgs e)
        {
            txtFocus.Background = Brushes.LightYellow;
            txtFocus.BorderBrush = Brushes.Orange;
            txtFocusStatus.Text = "포커스 상태: 획득 (GotFocus)";
        }

        private void TxtFocus_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFocus.ClearValue(TextBox.BackgroundProperty);
            txtFocus.ClearValue(TextBox.BorderBrushProperty);
            txtFocusStatus.Text = "포커스 상태: 해제 (LostFocus)";
        }

        // KeyDown 이벤트
        private void TxtKeyDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtKeyResult.Text = $"입력 확정: {txtKeyDown.Text}";
            }
        }

        // 동적 텍스트 조작
        private void BtnAppend_Click(object sender, RoutedEventArgs e)
        {
            txtDynamic.Text += " [추가됨]";
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtDynamic.Clear();
        }

        private void BtnUpperCase_Click(object sender, RoutedEventArgs e)
        {
            txtDynamic.Text = txtDynamic.Text.ToUpper();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtDynamic.Text = "동적으로 변경됩니다";
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
                        fullXaml = xamlCode.Replace("<TextBox",
                            "<TextBox xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
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
