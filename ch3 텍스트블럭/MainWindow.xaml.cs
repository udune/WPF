using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace ch3_텍스트블럭
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
            tbDynamic.Text = $"코드에서 변경된 텍스트 (클릭 {_clickCount}회)";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }

        private void BtnRedStyle_Click(object sender, RoutedEventArgs e)
        {
            tbStyleDemo.Background = Brushes.Tomato;
            tbStyleDemo.Foreground = Brushes.White;
            tbStyleDemo.FontWeight = FontWeights.Bold;
            tbStyleDemo.Text = "빨간 스타일 적용됨";
        }

        private void BtnBlueStyle_Click(object sender, RoutedEventArgs e)
        {
            tbStyleDemo.Background = Brushes.SteelBlue;
            tbStyleDemo.Foreground = Brushes.White;
            tbStyleDemo.FontStyle = FontStyles.Italic;
            tbStyleDemo.Text = "파란 스타일 적용됨";
        }

        private void BtnResetStyle_Click(object sender, RoutedEventArgs e)
        {
            tbStyleDemo.Background = Brushes.Transparent;
            tbStyleDemo.Foreground = Brushes.Black;
            tbStyleDemo.FontWeight = FontWeights.Normal;
            tbStyleDemo.FontStyle = FontStyles.Normal;
            tbStyleDemo.Text = "스타일이 변경됩니다";
        }
    }
}
