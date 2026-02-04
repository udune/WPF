using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ch7_이미지
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

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
    }
}
