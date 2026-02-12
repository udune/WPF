using System.Windows;
using System.Windows.Interop;

namespace ch30_탭컨트롤_모달_모달리스
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 확인 결과를 저장하는 속성 (모달리스 창에서 사용)
        /// </summary>
        public bool? Result { get; private set; }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Result = true;

            // 모달 창인 경우에만 DialogResult 설정 (모달리스는 예외 발생)
            if (ComponentDispatcher.IsThreadModal)
                this.DialogResult = true;
            else
                this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = false;

            // 모달 창인 경우에만 DialogResult 설정 (모달리스는 예외 발생)
            if (ComponentDispatcher.IsThreadModal)
                this.DialogResult = false;
            else
                this.Close();
        }
    }
}
