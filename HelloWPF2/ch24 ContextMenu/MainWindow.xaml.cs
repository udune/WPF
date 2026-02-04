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

namespace ch24_ContextMenu
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
        private void MenuItem_UnChecked(object sender, RoutedEventArgs e)
        {
            tb.FontStyle = FontStyles.Normal;
        }
    }
}