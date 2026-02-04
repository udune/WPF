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

namespace ch09_Radio_Button
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

        private void GreenApple_Checked(object sender, RoutedEventArgs e)
        {
            if (rb != null)
            {
                rb.IsChecked = true;
            }
        }

        private void GreenApple_Unchecked(object sender, RoutedEventArgs e)
        {
            if (rb != null)
            {
                rb.IsChecked = false;
            }
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            foreach (object child in PersonGroup.Children)
            {
                if (child is RadioButton rbItem && rbItem.IsChecked == true)
                {
                    MessageBox.Show($"선택된 사람: {rbItem.Content}");
                    return;
                }
            }

            MessageBox.Show("선택된 사람이 없습니다.");
        }
    }
}