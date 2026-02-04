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

namespace ch16_DockPanel;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        Button newBtn = new Button();
        newBtn.Content = "버튼";
        newBtn.Margin = new Thickness(0, 5, 0, 0);
        newBtn.Padding = new Thickness(10);
        newBtn.Click += (sender, args) => MessageBox.Show($"{newBtn.Content} 클릭됨");
        
        ChangePosition(newBtn, Dock.Left);
    }

    private void ChangePosition(Button targetButton, Dock newDock)
    {
        DockPanel.SetDock(targetButton, newDock);
        
        targetButton.Content = $"{newDock}으로 이동됨";
    }
}