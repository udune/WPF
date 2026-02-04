using System.Windows;

namespace ch22_툴바
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 볼륨 슬라이더 변경
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (volumeText != null)
            {
                volumeText.Text = $"{volumeSlider.Value:0}%";
            }
        }

        // ToolBarTray 잠금
        private void LockCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            toggleableTray.IsLocked = true;
        }

        // ToolBarTray 잠금 해제
        private void LockCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            toggleableTray.IsLocked = false;
        }

        // 브라우저 이동 버튼
        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"이동: {urlTextBox.Text}", "알림");
        }
    }
}
