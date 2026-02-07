using System.Windows;
using System.Windows.Controls;

namespace ch29_화면이동_프레임.Views
{
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            // Page2로 이동
            NavigationService.Navigate(new Page2());
        }
    }
}
