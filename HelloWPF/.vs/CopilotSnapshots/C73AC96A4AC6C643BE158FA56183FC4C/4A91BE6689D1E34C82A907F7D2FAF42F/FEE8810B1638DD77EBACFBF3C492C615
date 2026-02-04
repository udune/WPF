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

namespace ch30_탭컨트롤_모달_모달리스
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Modalless_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
        }

        private void Modal_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.ShowDialog();
        }
        
        /*
         * Show() vs ShowDialog() 비교:
         * 
         * ========== Show() - 모달리스 (Modalless) =========
         * 
         * 코드:
         * Window1 window1 = new Window1();
         * window1.Show();
         * // 즉시 계속 실행
         * MessageBox.Show("Show() 호출 직후");
         * 
         * 특징:
         * - 반환 타입: void
         * - 즉시 반환
         * - 부모 창 조작 가능 ✓
         * - 여러 창 동시 열기 가능
         * - 창 간 자유로운 전환
         * 
         * 사용 예:
         * private void ShowTool_Click(object sender, RoutedEventArgs e)
         * {
         *     if (_toolWindow == null || !_toolWindow.IsVisible)
         *     {
         *         _toolWindow = new ToolWindow();
         *         _toolWindow.Owner = this;
         *         _toolWindow.Show();
         *     }
         *     else
         *     {
         *         _toolWindow.Activate(); // 기존 창 활성화
         *     }
         * }
         * 
         * ========== ShowDialog() - 모달 (Modal) =========
         * 
         * 코드:
         * Window1 window1 = new Window1();
         * bool? result = window1.ShowDialog();
         * // Window1 닫힐 때까지 대기
         * if (result == true)
         * {
         *     MessageBox.Show("확인 버튼 클릭");
         * }
         * 
         * 특징:
         * - 반환 타입: bool?
         * - 창 닫힐 때까지 대기 (블로킹)
         * - 부모 창 조작 불가능 ✗
         * - 결과 반환 (DialogResult)
         * - 순차적 흐름 보장
         * 
         * 사용 예:
         * private void Login_Click(object sender, RoutedEventArgs e)
         * {
         *     var loginWindow = new LoginWindow();
         *     loginWindow.Owner = this;
         *     
         *     if (loginWindow.ShowDialog() == true)
         *     {
         *         var user = loginWindow.LoggedInUser;
         *         LoadUserData(user);
         *     }
         *     else
         *     {
         *         MessageBox.Show("로그인이 취소되었습니다.");
         *     }
         * }
         * 
         * ========== DialogResult 설정 (Window1에서) =========
         * 
         * 확인 버튼:
         * private void OK_Click(object sender, RoutedEventArgs e)
         * {
         *     this.DialogResult = true; // 확인
         *     // Close()는 자동 호출됨
         * }
         * 
         * 취소 버튼:
         * private void Cancel_Click(object sender, RoutedEventArgs e)
         * {
         *     this.DialogResult = false; // 취소
         *     // Close()는 자동 호출됨
         * }
         * 
         * X 버튼 (기본):
         * // DialogResult 설정 안 함
         * // 반환값: null
         * 
         * DialogResult 속성:
         * - 설정 시 자동으로 창이 닫힘
         * - Close() 명시적으로 호출할 필요 없음
         * - ShowDialog()의 반환값이 됨
         * 
         * ========== Owner 속성 설정 =========
         * 
         * 모달리스:
         * Window1 window1 = new Window1();
         * window1.Owner = this; // MainWindow를 Owner로
         * window1.Show();
         * 
         * 모달:
         * Window1 window1 = new Window1();
         * window1.Owner = this;
         * window1.ShowDialog();
         * 
         * Owner 효과:
         * - 부모 창 최소화 → 자식 창도 최소화
         * - 부모 창 닫기 → 자식 창도 닫힘
         * - 자식 창이 부모 창 앞에 표시
         * - 작업 표시줄에 그룹화
         * - 중앙 배치 (CenterOwner)
         * 
         * ========== WindowStartupLocation =========
         * 
         * 코드:
         * window1.WindowStartupLocation = WindowStartupLocation.CenterOwner;
         * 
         * 또는 XAML:
         * <Window WindowStartupLocation="CenterOwner">
         * 
         * 옵션:
         * - Manual: 수동 (Left, Top 속성)
         * - CenterScreen: 화면 중앙
         * - CenterOwner: 부모 창 중앙 (Owner 필요)
         * 
         * ========== 실전 활용 패턴 =========
         * 
         * 1. 로그인 창 (모달):
         * private void ShowLogin()
         * {
         *     var loginWindow = new LoginWindow();
         *     loginWindow.Owner = this;
         *     loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
         *     
         *     if (loginWindow.ShowDialog() == true)
         *     {
         *         CurrentUser = loginWindow.LoggedInUser;
         *         LoadUserData();
         *     }
         *     else
         *     {
         *         Application.Current.Shutdown();
         *     }
         * }
         * 
         * 2. 설정 창 (모달):
         * private void Settings_Click(object sender, RoutedEventArgs e)
         * {
         *     var settingsWindow = new SettingsWindow();
         *     settingsWindow.Owner = this;
         *     settingsWindow.CurrentSettings = _settings.Clone();
         *     
         *     if (settingsWindow.ShowDialog() == true)
         *     {
         *         _settings = settingsWindow.CurrentSettings;
         *         ApplySettings();
         *     }
         * }
         * 
         * 3. 도구 창 (모달리스):
         * private ToolWindow _toolWindow;
         * 
         * private void ShowTool_Click(object sender, RoutedEventArgs e)
         * {
         *     if (_toolWindow == null || !_toolWindow.IsVisible)
         *     {
         *         _toolWindow = new ToolWindow();
         *         _toolWindow.Owner = this;
         *         _toolWindow.Topmost = true; // 항상 위
         *         _toolWindow.Show();
         *     }
         *     else
         *     {
         *         _toolWindow.Activate();
         *     }
         * }
         * 
         * 4. 찾기 창 (모달리스):
         * private FindWindow _findWindow;
         * 
         * private void Find_Click(object sender, RoutedEventArgs e)
         * {
         *     if (_findWindow == null || !_findWindow.IsVisible)
         *     {
         *         _findWindow = new FindWindow();
         *         _findWindow.Owner = this;
         *         _findWindow.Show();
         *     }
         *     else
         *     {
         *         _findWindow.Activate();
         *         _findWindow.Focus();
         *     }
         * }
         * 
         * 5. 확인 대화상자 (모달):
         * private void Delete_Click(object sender, RoutedEventArgs e)
         * {
         *     var confirmWindow = new ConfirmWindow
         *     {
         *         Owner = this,
         *         Message = "정말로 삭제하시겠습니까?"
         *     };
         *     
         *     if (confirmWindow.ShowDialog() == true)
         *     {
         *         DeleteItem();
         *     }
         * }
         * 
         * 6. 진행률 창 (모달):
         * private async void Process_Click(object sender, RoutedEventArgs e)
         * {
         *     var progressWindow = new ProgressWindow();
         *     progressWindow.Owner = this;
         *     
         *     var progress = new Progress<int>(value =>
         *     {
         *         progressWindow.ProgressValue = value;
         *     });
         *     
         *     var task = ProcessDataAsync(progress);
         *     
         *     progressWindow.ShowDialog();
         *     
         *     await task;
         * }
         * 
         * 7. 파일 선택 대화상자 (모달):
         * private void OpenFile_Click(object sender, RoutedEventArgs e)
         * {
         *     var openFileDialog = new Microsoft.Win32.OpenFileDialog
         *     {
         *         Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
         *     };
         *     
         *     if (openFileDialog.ShowDialog() == true)
         *     {
         *         LoadFile(openFileDialog.FileName);
         *     }
         * }
         * 
         * ========== 창 생명주기 관리 =========
         * 
         * 모달리스 창 닫기:
         * protected override void OnClosed(EventArgs e)
         * {
         *     base.OnClosed(e);
         *     
         *     // 모달리스 창들 닫기
         *     _toolWindow?.Close();
         *     _findWindow?.Close();
         * }
         * 
         * 창 활성화 확인:
         * private void Activate_Click(object sender, RoutedEventArgs e)
         * {
         *     if (_toolWindow != null && _toolWindow.IsVisible)
         *     {
         *         _toolWindow.Activate();
         *     }
         * }
         * 
         * ========== 주의사항 =========
         * 
         * 1. 모달 남용:
         *    - 너무 많은 모달 창은 사용자 경험 저해
         *    - 필요한 경우에만 사용
         * 
         * 2. Owner 설정:
         *    - Owner 설정 권장
         *    - 부모-자식 관계 명확히
         * 
         * 3. 메모리 관리:
         *    - 모달리스 창은 명시적으로 닫기
         *    - 부모 창 닫을 때 자식 창도 닫기
         * 
         * 4. DialogResult:
         *    - 모달 창에서만 의미 있음
         *    - 모달리스 창에서는 무시됨
         * 
         * 5. 창 재사용:
         *    - 모달리스 창은 재사용 가능
         *    - IsVisible 확인 후 Activate()
         */
    }
}