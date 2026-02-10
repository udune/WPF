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

namespace ch31_유저컨트롤
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 탭 4: 외부 접근 - uc1 텍스트 가져오기
        private void Button_GetText_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"입력된 텍스트: {uc1.txt.Text}", "텍스트 확인");
        }

        // 탭 4: 외부 접근 - uc1 진행률 가져오기
        private void Button_GetProgress_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"진행률: {uc1.Pb.Value}/{uc1.Pb.Maximum}", "진행률 확인");
        }

        // 탭 4: 외부 접근 - uc2 텍스트 가져오기
        private void Button_GetUc2Text_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"uc2 텍스트: {uc2.txt.Text}", "텍스트 확인");
        }

        // 탭 4: 외부 접근 - uc2 텍스트 설정하기
        private void Button_SetUc2Text_Click(object sender, RoutedEventArgs e)
        {
            uc2.txt.Text = "코드에서 설정한 텍스트입니다.";
        }
    }
}
