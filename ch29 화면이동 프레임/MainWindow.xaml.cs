using System.Windows;
using System.Windows.Navigation;

namespace ch29_화면이동_프레임
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 탐색 완료 이벤트 구독
            mainFrame.Navigated += MainFrame_Navigated;
        }

        // 뒤로 가기
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.CanGoBack)
            {
                mainFrame.GoBack();
            }
        }

        // 앞으로 가기
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.CanGoForward)
            {
                mainFrame.GoForward();
            }
        }

        // 홈으로 이동
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Views.Page1());
        }

        // 탐색 상태 업데이트
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateNavigationStatus();
        }

        private void UpdateNavigationStatus()
        {
            string backStatus = mainFrame.CanGoBack ? "가능" : "불가";
            string forwardStatus = mainFrame.CanGoForward ? "가능" : "불가";
            navStatus.Text = $"뒤로: {backStatus} | 앞으로: {forwardStatus}";
        }
    }
}
