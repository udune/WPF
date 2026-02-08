using System.Windows;

namespace ch30_탭컨트롤_모달_모달리스
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 모달리스 창 열기
        private void Modalless_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Owner = this;
            window1.Title = "모달리스 창 (Show)";
            window1.Show();

            resultText.Text = "모달리스 창이 열렸습니다. 이 창도 계속 사용할 수 있습니다.";
        }

        // 모달 창 열기
        private void Modal_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Owner = this;
            window1.Title = "모달 창 (ShowDialog)";

            bool? result = window1.ShowDialog();

            if (result == true)
            {
                resultText.Text = "모달 창 결과: 확인 (DialogResult = true)";
            }
            else if (result == false)
            {
                resultText.Text = "모달 창 결과: 취소 (DialogResult = false)";
            }
            else
            {
                resultText.Text = "모달 창 결과: X 버튼으로 닫힘 (DialogResult = null)";
            }
        }
    }
}
