using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch14_스택패널
{
    public partial class MainWindow : Window
    {
        private int count = 0;

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<StackPanel>\n    <Button Content=\"버튼 1\"/>\n    <Button Content=\"버튼 2\"/>\n</StackPanel>" },
            { "1_2", "<StackPanel Orientation=\"Horizontal\">\n    <Button Content=\"A\"/>\n    <Button Content=\"B\"/>\n</StackPanel>" },
            { "1_3", "<StackPanel>\n    <TextBlock Text=\"라벨\"/>\n    <TextBox/>\n    <Button Content=\"확인\"/>\n</StackPanel>" },

            // 탭 2: 정렬
            { "2_1", "<StackPanel>\n    <Button Content=\"중앙 정렬\" HorizontalAlignment=\"Center\"/>\n</StackPanel>" },
            { "2_2", "<StackPanel Orientation=\"Horizontal\" Height=\"100\">\n    <Button Content=\"아래 정렬\" VerticalAlignment=\"Bottom\"/>\n</StackPanel>" },
            { "2_3", "<StackPanel>\n    <Button Content=\"여백 있음\" Margin=\"10\"/>\n</StackPanel>" },

            // 탭 3: 중첩 패턴
            { "3_1", "<StackPanel>\n    <TextBlock Text=\"제목\"/>\n    <StackPanel Orientation=\"Horizontal\">\n        <Button Content=\"버튼 1\"/>\n        <Button Content=\"버튼 2\"/>\n    </StackPanel>\n</StackPanel>" },
            { "3_2", "<StackPanel Orientation=\"Horizontal\">\n    <TextBlock Text=\"이름:\" Width=\"80\"/>\n    <TextBox Width=\"150\"/>\n</StackPanel>" },

            // 탭 4: 스타일
            { "4_1", "<StackPanel Background=\"LightGreen\">\n    <TextBlock Text=\"배경색 있는 패널\" Margin=\"10\"/>\n</StackPanel>" },
            { "4_2", "<StackPanel Orientation=\"Horizontal\" FlowDirection=\"RightToLeft\">\n    <Button Content=\"첫번째\"/>\n    <Button Content=\"두번째\"/>\n</StackPanel>" },

            // 탭 5: 코드비하인드 (코드 비교 방식)
            { "5_1", "private void AddButton()\n{\n    // myPanel에 Button 추가\n    myPanel.Children.Add(new Button { Content = \"새 버튼\" });\n}" },
            { "5_2", "private void ChangeOrientation()\n{\n    // myPanel의 Orientation 변경\n    myPanel.Orientation = Orientation.Horizontal;\n}" },
            { "5_3", "private void ShowCount()\n{\n    // myPanel의 자식 요소 수를 txtResult.Text에 표시\n    txtResult.Text = myPanel.Children.Count.ToString();\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { "Children.Add", "Button" } },
            { "5_2", new[] { "Orientation", "Horizontal" } },
            { "5_3", new[] { "Children.Count" } },
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
                    if (trimmed.StartsWith("<StackPanel"))
                    {
                        fullXaml = xamlCode.Replace("<StackPanel",
                            "<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<Grid"))
                    {
                        fullXaml = xamlCode.Replace("<Grid",
                            "<Grid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<Button"))
                    {
                        fullXaml = xamlCode.Replace("<Button",
                            "<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
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

        // 탭5: 동적으로 버튼 추가
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            count++;
            spDynamic.Children.Add(new Button
            {
                Content = "동적 버튼 " + count,
                Margin = new Thickness(4),
                Padding = new Thickness(12, 4, 12, 4)
            });
        }

        // 탭5: 동적으로 텍스트 추가
        private void AddText_Click(object sender, RoutedEventArgs e)
        {
            count++;
            spDynamic.Children.Add(new TextBlock
            {
                Text = "동적 텍스트 " + count,
                Margin = new Thickness(4)
            });
        }

        // 탭5: 전체 삭제
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            spDynamic.Children.Clear();
            count = 0;
        }

        // 탭5: Orientation을 Vertical로 변경
        private void SetVertical_Click(object sender, RoutedEventArgs e)
        {
            spOrientation.Orientation = Orientation.Vertical;
            tbOrientation.Text = "현재: Vertical";
        }

        // 탭5: Orientation을 Horizontal로 변경
        private void SetHorizontal_Click(object sender, RoutedEventArgs e)
        {
            spOrientation.Orientation = Orientation.Horizontal;
            tbOrientation.Text = "현재: Horizontal";
        }

        // 탭5: 자식 요소 개수 확인
        private void CheckCount_Click(object sender, RoutedEventArgs e)
        {
            tbCount.Text = "자식 요소 수: " + spCount.Children.Count;
        }

        #endregion
    }
}
