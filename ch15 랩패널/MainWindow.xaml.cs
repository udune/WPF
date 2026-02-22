using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch15_랩패널
{
    public partial class MainWindow : Window
    {
        private int itemCount = 0;

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<WrapPanel>\n    <Button Content=\"버튼 1\"/>\n    <Button Content=\"버튼 2\"/>\n    <Button Content=\"버튼 3\"/>\n</WrapPanel>" },
            { "1_2", "<WrapPanel Orientation=\"Vertical\" Height=\"100\">\n    <Button Content=\"A\"/>\n    <Button Content=\"B\"/>\n    <Button Content=\"C\"/>\n</WrapPanel>" },

            // 탭 2: ItemWidth/ItemHeight
            { "2_1", "<WrapPanel ItemWidth=\"120\">\n    <Button Content=\"항목 1\"/>\n    <Button Content=\"항목 2\"/>\n</WrapPanel>" },
            { "2_2", "<WrapPanel ItemHeight=\"60\">\n    <Button Content=\"항목 1\"/>\n    <Button Content=\"항목 2\"/>\n</WrapPanel>" },
            { "2_3", "<WrapPanel ItemWidth=\"100\" ItemHeight=\"40\">\n    <Button Content=\"A\"/>\n    <Button Content=\"B\"/>\n</WrapPanel>" },

            // 탭 3: 실용 패턴
            { "3_1", "<WrapPanel>\n    <Border Background=\"LightBlue\" CornerRadius=\"10\" Padding=\"8,4\">\n        <TextBlock Text=\"태그\"/>\n    </Border>\n</WrapPanel>" },
            { "3_2", "<WrapPanel ItemWidth=\"100\" ItemHeight=\"80\">\n    <Border Background=\"LightCoral\" Margin=\"4\">\n        <TextBlock Text=\"카드 1\"/>\n    </Border>\n    <Border Background=\"LightGreen\" Margin=\"4\">\n        <TextBlock Text=\"카드 2\"/>\n    </Border>\n</WrapPanel>" },

            // 탭 4: 스타일
            { "4_1", "<WrapPanel Background=\"LightGreen\">\n    <Button Content=\"버튼 1\"/>\n    <Button Content=\"버튼 2\"/>\n</WrapPanel>" },
            { "4_2", "<WrapPanel FlowDirection=\"RightToLeft\">\n    <Button Content=\"첫번째\"/>\n    <Button Content=\"두번째\"/>\n</WrapPanel>" },

            // 탭 5: 코드비하인드 (코드 비교 방식)
            { "5_1", "private void AddButton()\n{\n    // myWrapPanel에 Button 추가\n    myWrapPanel.Children.Add(new Button { Content = \"새 버튼\" });\n}" },
            { "5_2", "private void ChangeOrientation()\n{\n    // myWrapPanel의 Orientation 변경\n    myWrapPanel.Orientation = Orientation.Vertical;\n}" },
            { "5_3", "private void ChangeItemWidth()\n{\n    // myWrapPanel의 ItemWidth 변경\n    myWrapPanel.ItemWidth = 150;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { "Children.Add", "Button" } },
            { "5_2", new[] { "Orientation", "Vertical" } },
            { "5_3", new[] { "ItemWidth", "150" } },
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
                    if (trimmed.StartsWith("<WrapPanel"))
                    {
                        fullXaml = xamlCode.Replace("<WrapPanel",
                            "<WrapPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
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

        // 탭5: 동적으로 버튼 추가
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            itemCount++;
            wpDynamic.Children.Add(new Button
            {
                Content = "항목 " + itemCount,
                Margin = new Thickness(4),
                Padding = new Thickness(12, 6, 12, 6)
            });
            tbDynamicCount.Text = "자식 요소: " + wpDynamic.Children.Count + "개";
        }

        // 탭5: 전체 삭제
        private void ClearItems_Click(object sender, RoutedEventArgs e)
        {
            wpDynamic.Children.Clear();
            itemCount = 0;
            tbDynamicCount.Text = "자식 요소: 0개";
        }

        // 탭5: Orientation을 Horizontal로 변경
        private void SetWrapHorizontal_Click(object sender, RoutedEventArgs e)
        {
            wpOrientation.Orientation = Orientation.Horizontal;
            tbWrapOrientation.Text = "현재: Horizontal";
        }

        // 탭5: Orientation을 Vertical로 변경
        private void SetWrapVertical_Click(object sender, RoutedEventArgs e)
        {
            wpOrientation.Orientation = Orientation.Vertical;
            tbWrapOrientation.Text = "현재: Vertical";
        }

        // 탭5: 슬라이더로 ItemWidth 변경
        private void sliderItemWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (wpItemWidth != null)
            {
                wpItemWidth.ItemWidth = sliderItemWidth.Value;
                tbItemWidth.Text = ((int)sliderItemWidth.Value).ToString();
            }
        }

        #endregion
    }
}
