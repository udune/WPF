using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch13_익스팬더
{
    public partial class MainWindow : Window
    {
        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<Expander Header=\"정보 보기\">\n</Expander>" },
            { "1_2", "<Expander Header=\"열린 상태\" IsExpanded=\"True\">\n    <TextBlock Text=\"내용\"/>\n</Expander>" },
            { "1_3", "<Expander Header=\"목록\">\n    <StackPanel>\n        <TextBlock Text=\"항목 1\"/>\n        <TextBlock Text=\"항목 2\"/>\n    </StackPanel>\n</Expander>" },

            // 탭 2: ExpandDirection
            { "2_1", "<Expander Header=\"위로 펼침\" ExpandDirection=\"Up\">\n    <TextBlock Text=\"내용\"/>\n</Expander>" },
            { "2_2", "<Expander Header=\"오른쪽으로\" ExpandDirection=\"Right\">\n    <TextBlock Text=\"내용\"/>\n</Expander>" },

            // 탭 3: 스타일과 외관
            { "3_1", "<Expander Header=\"배경색\" Background=\"LightBlue\">\n    <TextBlock Text=\"내용\"/>\n</Expander>" },
            { "3_2", "<Expander Header=\"빨간 굵은 글씨\" FontWeight=\"Bold\" Foreground=\"Red\">\n    <TextBlock Text=\"내용\"/>\n</Expander>" },
            { "3_3", "<Expander Header=\"테두리\" BorderBrush=\"Green\" BorderThickness=\"3\">\n    <TextBlock Text=\"내용\"/>\n</Expander>" },

            // 탭 4: 이벤트 (코드 비교 방식)
            { "4_1", "private void Expander_Expanded(object sender, RoutedEventArgs e)\n{\n    txtStatus.Text = \"펼쳐짐!\";\n}" },
            { "4_2", "private void ToggleExpander()\n{\n    // myExpander의 IsExpanded를 토글\n    myExpander.IsExpanded = !myExpander.IsExpanded;\n}" },

            // 탭 5: 데이터 바인딩 (코드 비교 방식)
            { "5_1", "IsExpanded=\"{Binding ElementName=chkExpand, Path=IsChecked}\"" },
            { "5_2", "Header=\"{Binding ElementName=txtHeader, Path=Text}\"" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "4_1", new[] { ".Text", "펼쳐짐" } },
            { "4_2", new[] { "IsExpanded", "!" } },
            { "5_1", new[] { "ElementName", "IsChecked" } },
            { "5_2", new[] { "ElementName", "Text" } },
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

                // XAML 네임스페이스 추가
                if (!xamlCode.Contains("xmlns="))
                {
                    string trimmed = xamlCode.TrimStart();
                    if (trimmed.StartsWith("<Expander"))
                    {
                        fullXaml = xamlCode.Replace("<Expander",
                            "<Expander xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<StackPanel"))
                    {
                        fullXaml = xamlCode.Replace("<StackPanel",
                            "<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<Grid"))
                    {
                        fullXaml = xamlCode.Replace("<Grid",
                            "<Grid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
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

        #endregion

        #region 튜토리얼 이벤트 핸들러

        // 탭3: Expanded / Collapsed 이벤트
        private void expEvent_Expanded(object sender, RoutedEventArgs e)
        {
            tbEventResult.Text = "Expanded! 펼쳐졌습니다.";
        }

        private void expEvent_Collapsed(object sender, RoutedEventArgs e)
        {
            tbEventResult.Text = "Collapsed! 접혔습니다.";
        }

        // 탭3: 코드비하인드로 IsExpanded 제어
        private void ExpandClick(object sender, RoutedEventArgs e)
        {
            expCodeBehind.IsExpanded = true;
        }

        private void CollapseClick(object sender, RoutedEventArgs e)
        {
            expCodeBehind.IsExpanded = false;
        }

        private void ToggleClick(object sender, RoutedEventArgs e)
        {
            expCodeBehind.IsExpanded = !expCodeBehind.IsExpanded;
        }

        // 탭3: 아코디언 패턴
        private void Accordion_Expanded(object sender, RoutedEventArgs e)
        {
            var current = sender as Expander;
            if (current != expAccordion1) expAccordion1.IsExpanded = false;
            if (current != expAccordion2) expAccordion2.IsExpanded = false;
            if (current != expAccordion3) expAccordion3.IsExpanded = false;
        }

        // 탭5: 헤더 동적 변경
        private void ChangeHeader_Click(object sender, RoutedEventArgs e)
        {
            expDynamic.Header = "변경된 헤더 (" + DateTime.Now.ToString("HH:mm:ss") + ")";
        }

        // 탭5: 배경색 동적 변경
        private void ChangeBackground_Click(object sender, RoutedEventArgs e)
        {
            expDynamic.Background = expDynamic.Background == Brushes.LightCoral
                ? Brushes.LightCyan : Brushes.LightCoral;
        }

        #endregion
    }
}
