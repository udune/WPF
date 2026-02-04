using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ch2_Label
{
    public partial class MainWindow : Window
    {
        private int _clickCount;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnChangeText_Click(object sender, RoutedEventArgs e)
        {
            _clickCount++;
            lblDynamic.Content = $"코드에서 변경된 텍스트 (클릭 {_clickCount}회)";
        }

        private void BtnRedStyle_Click(object sender, RoutedEventArgs e)
        {
            lblStyleDemo.Background = Brushes.Tomato;
            lblStyleDemo.Foreground = Brushes.White;
            lblStyleDemo.FontWeight = FontWeights.Bold;
            lblStyleDemo.Content = "빨간 스타일 적용됨";
        }

        private void BtnBlueStyle_Click(object sender, RoutedEventArgs e)
        {
            lblStyleDemo.Background = Brushes.SteelBlue;
            lblStyleDemo.Foreground = Brushes.White;
            lblStyleDemo.FontStyle = FontStyles.Italic;
            lblStyleDemo.Content = "파란 스타일 적용됨";
        }

        private void BtnResetStyle_Click(object sender, RoutedEventArgs e)
        {
            lblStyleDemo.Background = Brushes.Transparent;
            lblStyleDemo.Foreground = Brushes.Black;
            lblStyleDemo.FontWeight = FontWeights.Normal;
            lblStyleDemo.FontStyle = FontStyles.Normal;
            lblStyleDemo.Content = "스타일이 변경됩니다";
        }

        private void BtnVisible_Click(object sender, RoutedEventArgs e)
        {
            lblVisibility.Visibility = Visibility.Visible;
        }

        private void BtnHidden_Click(object sender, RoutedEventArgs e)
        {
            lblVisibility.Visibility = Visibility.Hidden;
        }

        private void BtnCollapsed_Click(object sender, RoutedEventArgs e)
        {
            lblVisibility.Visibility = Visibility.Collapsed;
        }
    }
}
