using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ch21_스테이터스바
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer? clockTimer;
        private int imageZoom = 100;

        public MainWindow()
        {
            InitializeComponent();
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
