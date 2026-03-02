using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace ch20_프로그레스바
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer? timer;

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<ProgressBar Height=\"25\" Value=\"50\"/>" },
            { "1_2", "<ProgressBar Height=\"25\" Maximum=\"500\" Value=\"250\"/>" },

            // 탭 2: 진행 모드
            { "2_1", "<ProgressBar Height=\"25\" IsIndeterminate=\"True\"/>" },
            { "2_2", "progressBar.IsIndeterminate = true;" },

            // 탭 3: 스타일과 색상
            { "3_1", "<ProgressBar Height=\"25\" Value=\"80\" Foreground=\"DodgerBlue\"/>" },
            { "3_2", "<ProgressBar Height=\"30\" Value=\"90\" Background=\"#E0E0E0\" Foreground=\"Green\"/>" },

            // 탭 4: 값 표시
            { "4_1", "<Grid>\n    <ProgressBar Height=\"30\" Value=\"65\"/>\n    <TextBlock Text=\"65%\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n</Grid>" },
            { "4_2", "progressBar.Value = 75;\npercentText.Text = \"75%\";" },

            // 탭 5: 실용 예제
            { "5_1", "DispatcherTimer timer = new DispatcherTimer();\ntimer.Interval = TimeSpan.FromMilliseconds(200);" },
            { "5_2", "if (value < 30)\n{\n    progressBar.Foreground = Brushes.Red;\n}\nelse if (value < 70)\n{\n    progressBar.Foreground = Brushes.Orange;\n}\nelse\n{\n    progressBar.Foreground = Brushes.Green;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "2_2", new[] { "progressBar", "IsIndeterminate", "true" } },
            { "4_2", new[] { "progressBar.Value", "75", "percentText.Text", "75%" } },
            { "5_1", new[] { "DispatcherTimer", "Interval", "TimeSpan.FromMilliseconds", "200" } },
            { "5_2", new[] { "Brushes.Red", "Brushes.Orange", "Brushes.Green", "else if", "Foreground" } },
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        // XAML 실행
        private void ExecuteXaml(string xamlCode, StackPanel resultPanel, Border resultBorder)
        {
            resultPanel.Children.Clear();
            resultBorder.Visibility = Visibility.Visible;

            try
            {
                string fullXaml = xamlCode;

                // 네임스페이스가 없으면 추가
                if (!xamlCode.Contains("xmlns="))
                {
                    // ProgressBar 처리
                    if (xamlCode.TrimStart().StartsWith("<ProgressBar"))
                    {
                        fullXaml = xamlCode.Replace("<ProgressBar",
                            "<ProgressBar xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    // Grid 처리
                    else if (xamlCode.TrimStart().StartsWith("<Grid"))
                    {
                        fullXaml = xamlCode.Replace("<Grid",
                            "<Grid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else
                    {
                        // 기타 컨트롤
                        fullXaml = $@"<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                            {xamlCode}
                        </StackPanel>";
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

        // 기본 사용법 - 값 증가
        private void IncreaseValue_Click(object sender, RoutedEventArgs e)
        {
            if (basicProgressBar.Value < basicProgressBar.Maximum)
                basicProgressBar.Value += 10;
        }

        // 기본 사용법 - 값 감소
        private void DecreaseValue_Click(object sender, RoutedEventArgs e)
        {
            if (basicProgressBar.Value > basicProgressBar.Minimum)
                basicProgressBar.Value -= 10;
        }

        // 기본 사용법 - 초기화
        private void ResetValue_Click(object sender, RoutedEventArgs e)
        {
            basicProgressBar.Value = 0;
        }

        // 진행 모드 - 확정 모드
        private void SetDeterminate_Click(object sender, RoutedEventArgs e)
        {
            modeProgressBar.IsIndeterminate = false;
        }

        // 진행 모드 - 불확정 모드
        private void SetIndeterminate_Click(object sender, RoutedEventArgs e)
        {
            modeProgressBar.IsIndeterminate = true;
        }

        // 스타일 - 상태별 색상 변경
        private void StatusSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (statusProgressBar == null || statusText == null) return;

            double value = statusSlider.Value;
            statusProgressBar.Value = value;

            if (value < 30)
            {
                statusProgressBar.Foreground = Brushes.Red;
                statusText.Text = $"{value:0}% - 위험";
            }
            else if (value < 70)
            {
                statusProgressBar.Foreground = Brushes.Orange;
                statusText.Text = $"{value:0}% - 진행 중";
            }
            else
            {
                statusProgressBar.Foreground = Brushes.Green;
                statusText.Text = $"{value:0}% - 양호";
            }
        }

        // 값 표시 - 동적 값 변경
        private void ValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (dynamicValueBar == null || dynamicValueText == null) return;

            dynamicValueBar.Value = valueSlider.Value;
            dynamicValueText.Text = $"{valueSlider.Value:0}%";
        }

        // 실용 예제 - 타이머 시작
        private void StartTimer_Click(object sender, RoutedEventArgs e)
        {
            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += (s, args) =>
                {
                    if (timerProgressBar.Value < 100)
                    {
                        timerProgressBar.Value += 1;
                        timerProgressText.Text = $"{timerProgressBar.Value}%";
                    }
                    else
                    {
                        timer.Stop();
                    }
                };
            }
            timer.Start();
        }

        // 실용 예제 - 타이머 일시정지
        private void PauseTimer_Click(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
        }

        // 실용 예제 - 타이머 초기화
        private void ResetTimer_Click(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
            timerProgressBar.Value = 0;
            timerProgressText.Text = "0%";
        }

        // 실용 예제 - 다운로드 시뮬레이션
        private async void StartDownload_Click(object sender, RoutedEventArgs e)
        {
            downloadProgressBar.Value = 0;
            downloadPercentText.Text = "";
            downloadProgressBar.IsIndeterminate = true;
            downloadStatusText.Text = "연결 중...";

            await Task.Delay(1000);

            downloadProgressBar.IsIndeterminate = false;
            downloadStatusText.Text = "다운로드 중...";

            for (int i = 0; i <= 100; i += 5)
            {
                downloadProgressBar.Value = i;
                downloadPercentText.Text = $"{i}%";
                await Task.Delay(100);
            }

            downloadStatusText.Text = "완료!";
        }
    }
}
