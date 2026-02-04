using System.Windows;

namespace ch20_ProgressBar
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 3. 퍼센트 표시 ProgressBar 컨트롤
        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (pbWithText.Value < 100)
            {
                pbWithText.Value += 10;
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (pbWithText.Value > 0)
            {
                pbWithText.Value -= 10;
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            pbWithText.Value = 0;
        }

        // 5. 비동기 작업 시뮬레이션
        private async void StartAsync_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            pbAsync.Value = 0;

            for (int i = 0; i <= 100; i += 5)
            {
                pbAsync.Value = i;
                tbStatus.Text = $"진행 중... {i}%";
                await Task.Delay(100);
            }

            tbStatus.Text = "완료!";
            btnStart.IsEnabled = true;
        }
    }
}
