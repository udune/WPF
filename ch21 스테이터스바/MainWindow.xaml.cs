using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace ch21_스테이터스바
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer? clockTimer;
        private int imageZoom = 100;

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "3_4", "<StatusBar>\n    <StatusBarItem Content=\"확대/축소:\"/>\n    <StatusBarItem>\n        <Slider Width=\"100\" Minimum=\"50\" Maximum=\"200\" Value=\"100\"/>\n    </StatusBarItem>\n    <StatusBarItem>\n        <TextBlock Text=\"100%\"/>\n    </StatusBarItem>\n</StatusBar>" },
            { "4_4", "private void Connect_Click(object sender, RoutedEventArgs e)\n{\n    connectionIndicator.Fill = Brushes.Green;\n    connectionText.Text = \"연결됨\";\n}" },
            { "5_4", "<StatusBar Background=\"#2D2D30\">\n    <StatusBarItem>\n        <StackPanel Orientation=\"Horizontal\">\n            <Ellipse Width=\"8\" Height=\"8\" Fill=\"LimeGreen\" Margin=\"0,0,6,0\"/>\n            <TextBlock Text=\"온라인\" Foreground=\"White\"/>\n        </StackPanel>\n    </StatusBarItem>\n    <Separator/>\n    <StatusBarItem>\n        <TextBlock Text=\"↓ 2.5 MB/s\" Foreground=\"LightGreen\"/>\n    </StatusBarItem>\n</StatusBar>" },
            { "1_3", "<StatusBar>\n    <StatusBarItem Content=\"report.txt\"/>\n    <Separator/>\n    <StatusBarItem Content=\"UTF-8\"/>\n</StatusBar>" },
            { "2_3", "<StatusBar Height=\"28\" Background=\"#E3F2FD\">\n    <StatusBarItem Content=\"준비\"/>\n</StatusBar>" },
            { "3_3", "<StatusBar>\n    <StatusBarItem>\n        <StackPanel Orientation=\"Horizontal\">\n            <ProgressBar Width=\"120\" Height=\"12\" Value=\"45\"/>\n            <TextBlock Text=\"45%\" Margin=\"6,0,0,0\"/>\n        </StackPanel>\n    </StatusBarItem>\n</StatusBar>" },
            { "4_3", "private void ShowTime_Click(object sender, RoutedEventArgs e)\n{\n    statusText.Content = DateTime.Now.ToString(\"HH:mm:ss\");\n}" },
            { "5_3", "<StatusBar>\n    <StatusBarItem Content=\"줄 12, 열 34\"/>\n    <Separator/>\n    <StatusBarItem Content=\"UTF-8\"/>\n    <Separator/>\n    <StatusBarItem Content=\"저장됨\"/>\n</StatusBar>" },
            // 탭 1: 기본 사용법
            { "1_1", "<StatusBar>\n    <StatusBarItem Content=\"상태: 준비됨\"/>\n</StatusBar>" },
            { "1_2", "<StatusBar>\n    <StatusBarItem Content=\"파일명\"/>\n    <Separator/>\n    <StatusBarItem Content=\"100%\"/>\n    <Separator/>\n    <StatusBarItem Content=\"저장됨\"/>\n</StatusBar>" },

            // 탭 2: 레이아웃
            { "2_1", "<StatusBar>\n    <StatusBar.ItemsPanel>\n        <ItemsPanelTemplate>\n            <DockPanel/>\n        </ItemsPanelTemplate>\n    </StatusBar.ItemsPanel>\n    <StatusBarItem Content=\"좌측\" DockPanel.Dock=\"Left\"/>\n    <StatusBarItem Content=\"우측\" DockPanel.Dock=\"Right\"/>\n</StatusBar>" },
            { "2_2", "<StatusBar Height=\"30\">\n    <StatusBarItem Content=\"패딩 적용\" Padding=\"15,5\"/>\n</StatusBar>" },

            // 탭 3: 다양한 콘텐츠
            { "3_1", "<StatusBar>\n    <StatusBarItem>\n        <ProgressBar Width=\"100\" Height=\"15\" Value=\"50\"/>\n    </StatusBarItem>\n    <StatusBarItem Content=\"50%\"/>\n</StatusBar>" },
            { "3_2", "<StatusBar>\n    <StatusBarItem>\n        <StackPanel Orientation=\"Horizontal\">\n            <Ellipse Width=\"10\" Height=\"10\" Fill=\"Green\" Margin=\"0,0,5,0\"/>\n            <TextBlock Text=\"온라인\"/>\n        </StackPanel>\n    </StatusBarItem>\n</StatusBar>" },

            // 탭 4: 동적 업데이트 (코드 비교)
            { "4_1", "private void BtnComplete_Click(object sender, RoutedEventArgs e)\n{\n    statusBarItem.Content = \"작업 완료!\";\n}" },
            { "4_2", "private void SetError_Click(object sender, RoutedEventArgs e)\n{\n    ellipse.Fill = Brushes.Red;\n    textBlock.Text = \"오류 발생\";\n}" },

            // 탭 5: 실용 예제
            { "5_1", "<StatusBar>\n    <StatusBarItem Content=\"10개 파일\"/>\n    <Separator/>\n    <StatusBarItem Content=\"2개 선택\"/>\n    <Separator/>\n    <StatusBarItem Content=\"500 KB\"/>\n</StatusBar>" },
            { "5_2", "<StatusBar Background=\"#333333\">\n    <StatusBarItem>\n        <StackPanel Orientation=\"Horizontal\">\n            <Ellipse Width=\"10\" Height=\"10\" Fill=\"Blue\" Margin=\"0,0,5,0\"/>\n            <TextBlock Text=\"동기화 중\" Foreground=\"White\"/>\n        </StackPanel>\n    </StatusBarItem>\n</StatusBar>" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "4_4", new[] { "connectionIndicator.Fill", "Brushes.Green", "connectionText.Text" } },
            { "4_3", new[] { "statusText.Content", "DateTime.Now" } },
            { "4_1", new[] { ".Content", "작업 완료" } },
            { "4_2", new[] { "Brushes.Red", ".Fill", ".Text", "오류 발생" } },
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        // XAML 실행 메서드
        private void ExecuteXaml(string xamlCode, StackPanel resultPanel, Border resultBorder)
        {
            resultPanel.Children.Clear();
            resultBorder.Visibility = Visibility.Visible;

            try
            {
                string fullXaml = xamlCode.Trim();

                if (!fullXaml.Contains("xmlns="))
                {
                    // 루트 태그가 무엇이든 프레젠테이션 네임스페이스를 붙여 줍니다.
                    Match root = Regex.Match(fullXaml, @"^<([A-Za-z_][\w.]*)");
                    if (root.Success)
                    {
                        string name = root.Groups[1].Value;
                        fullXaml = "<" + name
                            + " xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'"
                            + " xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'"
                            + fullXaml.Substring(name.Length + 1);
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

        // 다양한 콘텐츠 - 새로고침 버튼
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("새로고침되었습니다.", "알림");
        }

        // 다양한 콘텐츠 - 줌 슬라이더
        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (zoomText != null)
            {
                zoomText.Text = $"{zoomSlider.Value:0}%";
            }
        }

        // 동적 업데이트 - 상태 텍스트
        private void SetStatusReady_Click(object sender, RoutedEventArgs e)
        {
            statusText.Content = "준비";
        }

        private void SetStatusProcessing_Click(object sender, RoutedEventArgs e)
        {
            statusText.Content = "처리 중...";
        }

        private void SetStatusComplete_Click(object sender, RoutedEventArgs e)
        {
            statusText.Content = "완료";
        }

        private void SetStatusError_Click(object sender, RoutedEventArgs e)
        {
            statusText.Content = "오류 발생!";
        }

        // 동적 업데이트 - ProgressBar 연동
        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (progressBar == null || progressPercent == null || progressStatusText == null) return;

            progressBar.Value = progressSlider.Value;
            progressPercent.Text = $"{progressSlider.Value:0}%";

            if (progressSlider.Value >= 100)
                progressStatusText.Content = "완료";
            else
                progressStatusText.Content = "로딩 중...";
        }

        // 동적 업데이트 - 시계 시작
        private void StartClock_Click(object sender, RoutedEventArgs e)
        {
            if (clockTimer == null)
            {
                clockTimer = new DispatcherTimer();
                clockTimer.Interval = TimeSpan.FromSeconds(1);
                clockTimer.Tick += (s, args) =>
                {
                    timeText.Text = DateTime.Now.ToString("HH:mm:ss");
                };
            }
            clockTimer.Start();
            timeText.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        // 동적 업데이트 - 시계 정지
        private void StopClock_Click(object sender, RoutedEventArgs e)
        {
            clockTimer?.Stop();
        }

        // 동적 업데이트 - 연결
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            connectionIndicator.Fill = Brushes.Green;
            connectionText.Text = "연결됨";
        }

        // 동적 업데이트 - 연결 해제
        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            connectionIndicator.Fill = Brushes.Gray;
            connectionText.Text = "연결 안 됨";
        }

        // 실용 예제 - 줌 인
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (imageZoom < 200)
            {
                imageZoom += 10;
                imageZoomText.Text = $"{imageZoom}%";
            }
        }

        // 실용 예제 - 줌 아웃
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (imageZoom > 10)
            {
                imageZoom -= 10;
                imageZoomText.Text = $"{imageZoom}%";
            }
        }
    }
}
