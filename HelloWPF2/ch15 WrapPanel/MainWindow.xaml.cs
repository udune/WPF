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

namespace ch15_Wrap_Panel;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        for (int i = 0; i < 5; i++)
        {
            AddTag($"tag {i}");
        }
    }

    private void AddTag(string tagName)
    {
        Border tag = new Border
        {
            Background = Brushes.LightBlue,
            CornerRadius = new CornerRadius(15),
            Padding = new Thickness(10, 5, 10, 5),
            Child = new TextBlock { Text = tagName }
        };
        
        myWrapPanel.Children.Add(tag);
    }
}