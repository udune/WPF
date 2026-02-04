using System.Windows;
using System.Windows.Controls;

namespace ch12_슬라이더
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 슬라이더 값 변경 이벤트 - 현재 값을 TextBlock에 표시
        private void sld_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tb.Text = sld.Value.ToString();
        }
    }
}