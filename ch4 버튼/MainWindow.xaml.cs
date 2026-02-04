using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ch4_버튼
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Click 이벤트
        private int _clickCount;
        private void BtnClick_Click(object sender, RoutedEventArgs e)
        {
            _clickCount++;
            txtClickResult.Text = $"버튼을 {_clickCount}번 클릭했습니다";
        }

        // MouseDoubleClick 이벤트
        private void BtnDouble_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtDoubleClick.Background = Brushes.Salmon;
            txtDoubleClick.Foreground = Brushes.White;
            txtDoubleClick.Text = "더블클릭 감지!";
        }

        // MouseEnter / MouseLeave 이벤트
        private void BtnHover_MouseEnter(object sender, MouseEventArgs e)
        {
            btnHover.Foreground = Brushes.Red;
            btnHover.Content = "마우스 진입!";
        }

        private void BtnHover_MouseLeave(object sender, MouseEventArgs e)
        {
            btnHover.Foreground = Brushes.Black;
            btnHover.Content = "마우스를 올려보세요";
        }

        // 동적 스타일 변경
        private void BtnRedStyle_Click(object sender, RoutedEventArgs e)
        {
            btnStyleTarget.Background = Brushes.Tomato;
            btnStyleTarget.Foreground = Brushes.White;
            btnStyleTarget.FontWeight = FontWeights.Bold;
            btnStyleTarget.Content = "빨간 스타일 적용됨";
        }

        private void BtnBlueStyle_Click(object sender, RoutedEventArgs e)
        {
            btnStyleTarget.Background = Brushes.SteelBlue;
            btnStyleTarget.Foreground = Brushes.White;
            btnStyleTarget.FontStyle = FontStyles.Italic;
            btnStyleTarget.Content = "파란 스타일 적용됨";
        }

        private void BtnResetStyle_Click(object sender, RoutedEventArgs e)
        {
            btnStyleTarget.ClearValue(Button.BackgroundProperty);
            btnStyleTarget.ClearValue(Button.ForegroundProperty);
            btnStyleTarget.FontWeight = FontWeights.Normal;
            btnStyleTarget.FontStyle = FontStyles.Normal;
            btnStyleTarget.Content = "스타일이 변경됩니다";
        }

        // Visibility 제어
        private void BtnVisible_Click(object sender, RoutedEventArgs e)
        {
            btnVisibility.Visibility = Visibility.Visible;
        }

        private void BtnHidden_Click(object sender, RoutedEventArgs e)
        {
            btnVisibility.Visibility = Visibility.Hidden;
        }

        private void BtnCollapsed_Click(object sender, RoutedEventArgs e)
        {
            btnVisibility.Visibility = Visibility.Collapsed;
        }
    }
}
