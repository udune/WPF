using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Authentication;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ch06_Password_Box
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string inputPassword = pwd.Password;

            if (string.IsNullOrEmpty(inputPassword))
            {
                MessageBox.Show("비밀번호를 입력해 주세요.");
                return;
            }

            MessageBox.Show($"입력한 비밀번호: {inputPassword}");
        }
    }
}