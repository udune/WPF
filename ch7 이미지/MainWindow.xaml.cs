using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ch7_이미지
{
    public partial class MainWindow : Window
    {
        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<Image Source=\"https://via.placeholder.com/100\"/>" },
            { "1_2", "<Image Source=\"https://via.placeholder.com/150\" Width=\"80\" Height=\"80\"/>" },

            // 탭 2: 정렬
            { "2_1", "<Image Source=\"https://via.placeholder.com/60\" Width=\"60\" Height=\"60\" HorizontalAlignment=\"Center\"/>" },
            { "2_2", "<Image Source=\"https://via.placeholder.com/60\" Width=\"60\" Height=\"60\" Margin=\"15\"/>" },

            // 탭 3: Stretch 모드
            { "3_1", "<Image Source=\"https://via.placeholder.com/150\" Width=\"200\" Height=\"100\" Stretch=\"Fill\"/>" },
            { "3_2", "<Image Source=\"https://via.placeholder.com/150\" Width=\"200\" Height=\"100\" Stretch=\"Uniform\"/>" },
            { "3_3", "<Image Source=\"https://via.placeholder.com/80\" Width=\"150\" Height=\"150\" Stretch=\"None\"/>" },

            // 탭 4: 외관과 효과
            { "4_1", "<Image Source=\"https://via.placeholder.com/80\" Width=\"80\" Height=\"80\" Opacity=\"0.5\"/>" },
            { "4_2", "<Border BorderBrush=\"Blue\" BorderThickness=\"3\">\n    <Image Source=\"https://via.placeholder.com/80\" Width=\"80\" Height=\"80\"/>\n</Border>" },

            // 탭 5: 이벤트와 상호작용 (코드 비교)
            { "5_1", "private void Img_MouseEnter(object sender, MouseEventArgs e)\n{\n    img.Width = 150;\n    img.Height = 150;\n}" },
            { "5_2", "private void SetWebImage()\n{\n    img.Source = new BitmapImage(new Uri(\"https://example.com/image.png\"));\n}" },
            { "5_3", "private void ToggleVisibility()\n{\n    if (img.Visibility == Visibility.Visible)\n        img.Visibility = Visibility.Collapsed;\n    else\n        img.Visibility = Visibility.Visible;\n}" }
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { "Width", "Height", "150" } },
            { "5_2", new[] { "BitmapImage", "Uri", ".Source" } },
            { "5_3", new[] { "Visibility", "Visible", "Collapsed", "if" } }
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
                    // Image 또는 Border에 네임스페이스 추가
                    if (xamlCode.TrimStart().StartsWith("<Image"))
                    {
                        fullXaml = xamlCode.Replace("<Image",
                            "<Image xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (xamlCode.TrimStart().StartsWith("<Border"))
                    {
                        fullXaml = xamlCode.Replace("<Border",
                            "<Border xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
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

        // MouseEnter / MouseLeave 이벤트
        private void ImgHover_MouseEnter(object sender, MouseEventArgs e)
        {
            imgHover.Width = 120;
            imgHover.Height = 120;
            txtHoverStatus.Text = "마우스 진입 → 크기 확대";
        }

        private void ImgHover_MouseLeave(object sender, MouseEventArgs e)
        {
            imgHover.Width = 80;
            imgHover.Height = 80;
            txtHoverStatus.Text = "마우스 이탈 → 원래 크기";
        }

        // MouseDown 이벤트
        private void ImgClick_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgClick.Opacity = imgClick.Opacity > 0.3 ? imgClick.Opacity - 0.2 : 1.0;
            txtClickStatus.Text = $"현재 Opacity: {imgClick.Opacity:F1}";
        }

        // 동적 이미지 변경
        private void BtnLocalImage_Click(object sender, RoutedEventArgs e)
        {
            imgDynamic.Source = new BitmapImage(new Uri("/img/1.png", UriKind.Relative));
        }

        private void BtnWebImage_Click(object sender, RoutedEventArgs e)
        {
            imgDynamic.Source = new BitmapImage(
                new Uri("https://cdn2.iconfinder.com/data/icons/free-1/128/Android__logo__robot-512.png"));
        }

        private void BtnClearImage_Click(object sender, RoutedEventArgs e)
        {
            imgDynamic.Source = null;
        }

        // Visibility 제어
        private void BtnImgVisible_Click(object sender, RoutedEventArgs e)
            => imgVisibility.Visibility = Visibility.Visible;

        private void BtnImgHidden_Click(object sender, RoutedEventArgs e)
            => imgVisibility.Visibility = Visibility.Hidden;

        private void BtnImgCollapsed_Click(object sender, RoutedEventArgs e)
            => imgVisibility.Visibility = Visibility.Collapsed;

        #endregion
    }
}
