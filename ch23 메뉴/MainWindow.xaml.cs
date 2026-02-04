using System.Windows;

namespace ch23_메뉴
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 메뉴 클릭 시 UserControl1으로 화면 전환
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new UserControl1();
        }
    }
}
