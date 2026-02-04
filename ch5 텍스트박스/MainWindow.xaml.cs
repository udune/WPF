using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ch5_텍스트박스
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // TextChanged 이벤트
        private void TxtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCharCount != null && txtInput != null)
                txtCharCount.Text = $"글자 수: {txtInput.Text.Length}";
        }

        // SelectionChanged 이벤트
        private void TxtSelection_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string selected = txtSelection.SelectedText;
            if (txtSelectedText != null && txtInput != null)
            {
                txtSelectedText.Text = string.IsNullOrEmpty(selected)
                ? "선택된 텍스트: (없음)"
                : $"선택된 텍스트: {selected}";
            }
        }

        // GotFocus / LostFocus 이벤트
        private void TxtFocus_GotFocus(object sender, RoutedEventArgs e)
        {
            txtFocus.Background = Brushes.LightYellow;
            txtFocus.BorderBrush = Brushes.Orange;
            txtFocusStatus.Text = "포커스 상태: 획득 (GotFocus)";
        }

        private void TxtFocus_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFocus.ClearValue(TextBox.BackgroundProperty);
            txtFocus.ClearValue(TextBox.BorderBrushProperty);
            txtFocusStatus.Text = "포커스 상태: 해제 (LostFocus)";
        }

        // KeyDown 이벤트
        private void TxtKeyDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtKeyResult.Text = $"입력 확정: {txtKeyDown.Text}";
            }
        }

        // 동적 텍스트 조작
        private void BtnAppend_Click(object sender, RoutedEventArgs e)
        {
            txtDynamic.Text += " [추가됨]";
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtDynamic.Clear();
        }

        private void BtnUpperCase_Click(object sender, RoutedEventArgs e)
        {
            txtDynamic.Text = txtDynamic.Text.ToUpper();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtDynamic.Text = "동적으로 변경됩니다";
        }
    }
}
