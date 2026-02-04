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

namespace ch10_Calander
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            calendar.DisplayDateStart = DateTime.Today;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = calendar.SelectedDate.Value;
                selectedDateTextBox.Text = selectedDate.ToString("D");
            }
            else
            {
                selectedDateTextBox.Text = string.Empty;
            }
        }
    }
}