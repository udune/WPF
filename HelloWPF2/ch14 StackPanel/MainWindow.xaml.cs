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

namespace ch14_StackPanel;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Button newBtn = new Button();
        newBtn.Content = $"동적 생성 항목 {DynamicContainer.Children.Count}";
        newBtn.Margin = new Thickness(0, 5, 0, 0);
        newBtn.Padding = new Thickness(10);
        newBtn.Click += (sender, args) => MessageBox.Show($"{newBtn.Content} 클릭됨");
        DynamicContainer.Children.Add(newBtn);
    }
}