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

namespace ch08_Checkbox
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

        private void chkApple_Checked(object sender, RoutedEventArgs e)
        {
            tb.Text= "Apple is checked.";
        }

        private void chkApple_Unchecked(object sender, RoutedEventArgs e)
        {
            tb.Text = "Apple is unchecked.";
        }
    }
}