using System.Collections.Generic;
using System.Windows;

namespace ch8_체크박스
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Checked / Unchecked 이벤트
        private void ChkBasic_Checked(object sender, RoutedEventArgs e)
        {
            if (txtBasicStatus != null)
                txtBasicStatus.Text = "체크 상태: 체크됨 ✔";
        }

        private void ChkBasic_Unchecked(object sender, RoutedEventArgs e)
        {
            if (txtBasicStatus != null)
                txtBasicStatus.Text = "체크 상태: 해제됨";
        }

        // IsThreeState + Indeterminate 이벤트
        private void ChkThree_Checked(object sender, RoutedEventArgs e)
        {
            if (txtThreeStatus != null)
                txtThreeStatus.Text = "상태: 체크됨 (True)";
        }

        private void ChkThree_Unchecked(object sender, RoutedEventArgs e)
        {
            if (txtThreeStatus != null)
                txtThreeStatus.Text = "상태: 해제됨 (False)";
        }

        private void ChkThree_Indeterminate(object sender, RoutedEventArgs e)
        {
            if (txtThreeStatus != null)
                txtThreeStatus.Text = "상태: 불확정 (Null)";
        }

        // 여러 체크박스 조합
        private void ChkFruit_Changed(object sender, RoutedEventArgs e)
        {
            if (txtFruitResult == null) return;

            var fruits = new List<string>();
            if (chkApple.IsChecked == true) fruits.Add("사과");
            if (chkBanana.IsChecked == true) fruits.Add("바나나");
            if (chkGrape.IsChecked == true) fruits.Add("포도");
            if (chkOrange.IsChecked == true) fruits.Add("오렌지");

            txtFruitResult.Text = fruits.Count > 0
                ? $"선택된 과일: {string.Join(", ", fruits)}"
                : "선택된 과일: (없음)";
        }

        // 전체 선택 / 해제
        private void ChkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            if (chkItem1 == null) return;
            chkItem1.IsChecked = true;
            chkItem2.IsChecked = true;
            chkItem3.IsChecked = true;
        }

        private void ChkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkItem1 == null) return;
            chkItem1.IsChecked = false;
            chkItem2.IsChecked = false;
            chkItem3.IsChecked = false;
        }
    }
}
