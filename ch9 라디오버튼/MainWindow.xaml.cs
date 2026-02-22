using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch9_라디오버튼
{
    public partial class MainWindow : Window
    {
        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<RadioButton Content=\"좋아요\"/>" },
            { "1_2", "<StackPanel>\n    <RadioButton GroupName=\"Fruit\" Content=\"사과\"/>\n    <RadioButton GroupName=\"Fruit\" Content=\"바나나\"/>\n</StackPanel>" },
            { "1_3", "<RadioButton Content=\"기본 선택\" IsChecked=\"True\"/>" },

            // 탭 2: 정렬
            { "2_1", "<RadioButton Content=\"가운데 정렬\" HorizontalAlignment=\"Center\"/>" },
            { "2_2", "<StackPanel Orientation=\"Horizontal\">\n    <RadioButton Content=\"예\" Margin=\"5,0\"/>\n    <RadioButton Content=\"아니오\" Margin=\"5,0\"/>\n</StackPanel>" },

            // 탭 3: 폰트와 스타일
            { "3_1", "<RadioButton Content=\"큰 글씨\" FontSize=\"20\"/>" },
            { "3_2", "<RadioButton Content=\"굵은 글씨\" FontWeight=\"Bold\"/>" },
            { "3_3", "<RadioButton Content=\"기울임체\" FontStyle=\"Italic\"/>" },
            { "3_4", "<RadioButton Content=\"코드 폰트\" FontFamily=\"Consolas\"/>" },

            // 탭 4: 색상과 외관
            { "4_1", "<RadioButton Content=\"파란 글씨\" Foreground=\"Blue\"/>" },
            { "4_2", "<RadioButton Content=\"초록 배경\" Background=\"LightGreen\"/>" },
            { "4_3", "<RadioButton Content=\"주황 테두리\" BorderBrush=\"Orange\" BorderThickness=\"2\"/>" },
            { "4_4", "<RadioButton Content=\"여백 있는 버튼\" Padding=\"10\" Background=\"LightBlue\"/>" },

            // 탭 5: 이벤트와 상호작용 (코드 비교 방식)
            { "5_1", "private void RadioButton_Checked(object sender, RoutedEventArgs e)\n{\n    var rb = sender as RadioButton;\n    MessageBox.Show($\"선택됨: {rb.Content}\");\n}" },
            { "5_2", "private string GetSelectedSize()\n{\n    if (rbSmall.IsChecked == true) return \"Small\";\n    else if (rbMedium.IsChecked == true) return \"Medium\";\n    else return \"Large\";\n}" },
            { "5_3", "private void SelectOptionB()\n{\n    rbOption2.IsChecked = true;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { "sender", "RadioButton", ".Content" } },
            { "5_2", new[] { "IsChecked", "true", "return" } },
            { "5_3", new[] { "IsChecked", "true" } },
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
                    if (trimmed.StartsWith("<RadioButton"))
                    {
                        fullXaml = xamlCode.Replace("<RadioButton",
                            "<RadioButton xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<StackPanel"))
                    {
                        fullXaml = xamlCode.Replace("<StackPanel",
                            "<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<WrapPanel"))
                    {
                        fullXaml = xamlCode.Replace("<WrapPanel",
                            "<WrapPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
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

        #region 기존 이벤트 핸들러

        // Checked 이벤트
        private void RbEvent_Checked(object sender, RoutedEventArgs e)
        {
            if (txtEventStatus == null) return;
            var rb = sender as RadioButton;
            txtEventStatus.Text = $"선택됨: {rb?.Content}";
        }

        // 크기 그룹
        private void RbSize_Checked(object sender, RoutedEventArgs e)
        {
            UpdateOrder();
        }

        // 색상 그룹
        private void RbColor_Checked(object sender, RoutedEventArgs e)
        {
            UpdateOrder();
        }

        private void UpdateOrder()
        {
            if (txtOrderResult == null) return;

            string size = rbSmall.IsChecked == true ? "Small"
                         : rbMedium.IsChecked == true ? "Medium" : "Large";

            string color = rbRed.IsChecked == true ? "빨강"
                          : rbGreen.IsChecked == true ? "초록" : "파랑";

            txtOrderResult.Text = $"주문: {size} / {color}";
        }

        // 코드로 선택 변경
        private void BtnSelectA_Click(object sender, RoutedEventArgs e)
        {
            rbDyn1.IsChecked = true;
        }

        private void BtnSelectB_Click(object sender, RoutedEventArgs e)
        {
            rbDyn2.IsChecked = true;
        }

        private void BtnSelectC_Click(object sender, RoutedEventArgs e)
        {
            rbDyn3.IsChecked = true;
        }

        // 미리보기 연동
        private void RbPreview_Checked(object sender, RoutedEventArgs e)
        {
            if (brdPreview == null) return;

            if (rbPreviewRed.IsChecked == true)
            {
                brdPreview.Background = Brushes.Salmon;
                txtPreview.Text = "빨강 배경";
            }
            else if (rbPreviewGreen.IsChecked == true)
            {
                brdPreview.Background = Brushes.SeaGreen;
                txtPreview.Text = "초록 배경";
            }
            else
            {
                brdPreview.Background = Brushes.SteelBlue;
                txtPreview.Text = "파랑 배경";
            }
        }

        #endregion
    }
}
