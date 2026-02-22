using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch12_슬라이더
{
    public partial class MainWindow : Window
    {
        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<Slider />" },
            { "1_2", "<Slider Minimum=\"0\" Maximum=\"200\" Value=\"100\"/>" },
            { "1_3", "<Slider SmallChange=\"5\" LargeChange=\"20\"/>" },

            // 탭 2: 방향과 크기
            { "2_1", "<Slider Orientation=\"Vertical\" Height=\"150\"/>" },
            { "2_2", "<Slider Width=\"300\"/>" },
            { "2_3", "<Slider IsDirectionReversed=\"True\"/>" },

            // 탭 3: 틱과 스냅
            { "3_1", "<Slider TickPlacement=\"Both\"/>" },
            { "3_2", "<Slider TickPlacement=\"TopLeft\" TickFrequency=\"2\"/>" },
            { "3_3", "<Slider TickPlacement=\"BottomRight\" TickFrequency=\"5\" IsSnapToTickEnabled=\"True\"/>" },
            { "3_4", "<Slider TickPlacement=\"BottomRight\" Ticks=\"0,2,4,6,8,10\"/>" },

            // 탭 4: 색상과 외관
            { "4_1", "<Slider Background=\"LightGreen\"/>" },
            { "4_2", "<Slider Foreground=\"Blue\" TickPlacement=\"Both\"/>" },
            { "4_3", "<Slider Margin=\"10\" Background=\"LightPink\"/>" },
            { "4_4", "<Slider AutoToolTipPlacement=\"TopLeft\"/>" },

            // 탭 5: 이벤트와 바인딩 (코드 비교 방식)
            { "5_1", "private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)\n{\n    txtValue.Text = $\"현재 값: {e.NewValue}\";\n}" },
            { "5_2", "FontSize=\"{Binding ElementName=mySlider, Path=Value}\"" },
            { "5_3", "private void SetSliderValue()\n{\n    mySlider.Value = 50;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { "e.NewValue", "Text" } },
            { "5_2", new[] { "ElementName", "Path", "Value" } },
            { "5_3", new[] { ".Value", "50" } },
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
                    if (trimmed.StartsWith("<Slider"))
                    {
                        fullXaml = xamlCode.Replace("<Slider",
                            "<Slider xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
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

        // ValueChanged 이벤트 예제
        private void SldEvent_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (txtEventValue != null)
            {
                txtEventValue.Text = $"현재 값: {e.NewValue:F1}";
            }
        }

        // 볼륨 슬라이더
        private void SldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (txtVolume != null)
            {
                txtVolume.Text = $"볼륨: {e.NewValue:F0}%";
            }
        }

        // RGB 색상 조절
        private void SldRGB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sldR == null || sldG == null || sldB == null || brushPreview == null) return;

            byte r = (byte)sldR.Value;
            byte g = (byte)sldG.Value;
            byte b = (byte)sldB.Value;

            brushPreview.Color = Color.FromRgb(r, g, b);

            if (txtR != null) txtR.Text = r.ToString();
            if (txtG != null) txtG.Text = g.ToString();
            if (txtB != null) txtB.Text = b.ToString();
        }

        #endregion
    }
}
