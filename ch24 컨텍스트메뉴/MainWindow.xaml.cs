using System.Windows;

namespace ch24_컨텍스트메뉴
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 진하게 메뉴 클릭
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            tb.FontWeight = FontWeights.Bold;
        }

        // 기울기 체크 시
        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            tb.FontStyle = FontStyles.Italic;
        }

        // 기울기 체크 해제 시
        private void MenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            tb.FontStyle = FontStyles.Normal;
        }
    }
}