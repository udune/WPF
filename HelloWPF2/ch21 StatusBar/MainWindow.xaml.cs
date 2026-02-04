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

namespace ch21_StatusBar
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

        private void sl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sl.Value == 100)
            {
                sb.Content = "완료";
            }
            else
            {
                sb.Content = "로딩중..";
            }

            pb.Value = sl.Value;
        }
    }
}