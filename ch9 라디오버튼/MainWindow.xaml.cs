using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ch9_라디오버튼
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Checked 이벤트
        private void RbEvent_Checked(object sender, RoutedEventArgs e)
        {
            if (txtEventStatus == null) return;
            var rb = sender as RadioButton;
            txtEventStatus.Text = $"선택됨: {rb?.Content}";
        }

        // 크기 그룹
        private void RbSize_Checked(object sender, RoutedEventArgs e)
        {
            UpdateOrder();
        }

        // 색상 그룹
        private void RbColor_Checked(object sender, RoutedEventArgs e)
        {
            UpdateOrder();
        }

        private void UpdateOrder()
        {
            if (txtOrderResult == null) return;

            string size = rbSmall.IsChecked == true ? "Small"
                         : rbMedium.IsChecked == true ? "Medium" : "Large";

            string color = rbRed.IsChecked == true ? "빨강"
                          : rbGreen.IsChecked == true ? "초록" : "파랑";

            txtOrderResult.Text = $"주문: {size} / {color}";
        }

        // 코드로 선택 변경
        private void BtnSelectA_Click(object sender, RoutedEventArgs e)
        {
            rbDyn1.IsChecked = true;
        }

        private void BtnSelectB_Click(object sender, RoutedEventArgs e)
        {
            rbDyn2.IsChecked = true;
        }

        private void BtnSelectC_Click(object sender, RoutedEventArgs e)
        {
            rbDyn3.IsChecked = true;
        }

        // 미리보기 연동
        private void RbPreview_Checked(object sender, RoutedEventArgs e)
        {
            if (brdPreview == null) return;

            if (rbPreviewRed.IsChecked == true)
            {
                brdPreview.Background = Brushes.Salmon;
                txtPreview.Text = "빨강 배경";
            }
            else if (rbPreviewGreen.IsChecked == true)
            {
                brdPreview.Background = Brushes.SeaGreen;
                txtPreview.Text = "초록 배경";
            }
            else
            {
                brdPreview.Background = Brushes.SteelBlue;
                txtPreview.Text = "파랑 배경";
            }
        }
    }
}
