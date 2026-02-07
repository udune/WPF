using System.Windows;
using System.Windows.Controls;

namespace ch29_화면이동_프레임.Views
{
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // Page1으로 이동 (새 인스턴스 생성)
            NavigationService.Navigate(new Page1());
        }
    }
}
