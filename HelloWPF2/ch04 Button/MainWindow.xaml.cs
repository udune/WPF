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

namespace ch04_Button
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

        private void myButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            myTextBlock.Text = "Button double-clicked!";
            myTextBlock.Background = Brushes.Salmon;
            myTextBlock.Foreground = Brushes.White;
        }

        private void myButton_MouseEnter(object sender, MouseEventArgs e)
        {
            myTextBlock.Text = "Mouse entered the button area.";
            myTextBlock.Background = Brushes.Red;
        }

        private void myButton_MouseLeave(object sender, MouseEventArgs e)
        {
            myTextBlock.Text = "Mouse left the button area.";   
            myTextBlock.Background = Brushes.Black;
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myTextBlock.Text = "Button clicked!";
        }
    }
}