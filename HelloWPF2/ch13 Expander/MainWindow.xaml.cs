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

namespace ch13_Expander;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Expander_OnExpanded(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("관리자 권한이 필요한 영역입니다. 주의하세요!", "보안 알림");

        if (!CheckAdminPermissions())
        {
            MessageBox.Show("권한이 없습니다.");

            var expander = sender as Expander;
            expander?.IsExpanded = false;
        }

        // if (adminExpander.Content == null)
        // {
        //     LoadHeavyAdminData();
        // }
    }

    private bool CheckAdminPermissions()
    {
        return false;
    }
}