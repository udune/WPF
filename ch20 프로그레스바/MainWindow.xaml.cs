using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ch20_프로그레스바
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer? timer;

        public MainWindow()
        {
            InitializeComponent();
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
