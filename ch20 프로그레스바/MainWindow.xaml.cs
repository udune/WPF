using System.Windows;

namespace ch20_프로그레스바
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 버튼 클릭 시 ProgressBar 값 10씩 증가
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pb.Value += 10;
        }
    }
}