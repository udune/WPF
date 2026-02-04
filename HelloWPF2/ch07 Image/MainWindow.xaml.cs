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

namespace ch07_Image
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

        private void imgRobot_MouseEnter(object sender, MouseEventArgs e)
        {
            Uri newUri = new Uri("https://cdn2.iconfinder.com/data/icons/free-1/128/Android__logo__robot-512.png");
            BitmapImage newImage = new BitmapImage(newUri);
            imgRobot.Source = newImage;
        }

        private void imgRobot_MouseLeave(object sender, MouseEventArgs e)
        {
            imgRobot.Source = new BitmapImage(new Uri("pack://application:,,,/img/1.png"));
        }
    }
}