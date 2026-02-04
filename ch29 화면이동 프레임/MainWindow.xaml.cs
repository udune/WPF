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

namespace ch29_화면이동_프레임
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호작용 논리
    /// Frame과 NavigationService를 사용하여 페이지 간 탐색을 제어하는 메인 윈도우입니다.
    /// 뒤로/앞으로 버튼으로 탐색 기록을 관리하고 페이지를 전환합니다.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// MainWindow 생성자
        /// 윈도우가 생성될 때 호출되어 초기화 작업을 수행합니다.
        /// </summary>
        public MainWindow()
        {
            // XAML에 정의된 UI 요소들을 초기화합니다.
            InitializeComponent();
            /*
             * InitializeComponent() 호출 후:
             * 1. Frame(frm) 생성
             * 2. Source="/Views/Page1.xaml"에 의해 Page1 로드
             * 3. 버튼(뒤로, 앞으로) 생성
             * 4. 이벤트 핸들러 연결 (Back_Click, Front_Click)
             * 
             * 초기 상태:
             * - Frame에 Page1 표시
             * - 탐색 기록: [Page1]
             * - CanGoBack: false (뒤로 갈 페이지 없음)
             * - CanGoForward: false (앞으로 갈 페이지 없음)
             */
        }

        /// <summary>
        /// 뒤로 버튼 클릭 이벤트 핸들러
        /// Frame의 NavigationService를 사용하여 이전 페이지로 이동합니다.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체 (뒤로 버튼)</param>
        /// <param name="e">라우트 이벤트 인자</param>
        public void Back_Click(object sender, RoutedEventArgs e)
        {
            // NavigationService를 통해 이전 페이지로 이동 가능한지 확인
            if (frm.NavigationService.CanGoBack)
            {
                /*
                 * CanGoBack:
                 * - 이전 페이지가 있는지 확인
                 * - 탐색 기록에서 현재보다 이전 항목 존재 여부
                 * 
                 * true인 경우:
                 * - 이전에 방문한 페이지가 있음
                 * - GoBack() 호출 가능
                 * 
                 * false인 경우:
                 * - 첫 페이지 (더 이상 뒤로 갈 수 없음)
                 * - GoBack() 호출하면 예외 발생
                 * 
                 * 예:
                 * 탐색 기록: [Page1] ← 현재
                 * CanGoBack: false
                 * 
                 * 탐색 기록: [Page1] → [Page2] ← 현재
                 * CanGoBack: true (Page1로 갈 수 있음)
                 */
                
                // 이전 페이지로 이동
                frm.NavigationService.GoBack();
                /*
                 * GoBack():
                 * - 탐색 기록에서 이전 페이지로 이동
                 * - Back 스택에서 페이지를 가져와 표시
                 * - Forward 스택에 현재 페이지 추가
                 * 
                 * 동작:
                 * 1. 탐색 기록에서 이전 항목 찾기
                 * 2. 해당 Page 인스턴스 복원
                 * 3. Frame에 표시
                 * 4. OnNavigatedFrom 이벤트 (현재 페이지)
                 * 5. OnNavigatedTo 이벤트 (이전 페이지)
                 * 
                 * 예:
                 * 이동 전: [Page1] → [Page2] → [Page3] ← 현재
                 * GoBack() 호출
                 * 이동 후: [Page1] → [Page2] ← 현재 | [Page3]
                 *                          Back Stack   Forward Stack
                 * 
                 * Forward 스택:
                 * - GoBack() 호출 시 현재 페이지가 Forward 스택에 추가
                 * - GoForward()로 다시 돌아갈 수 있음
                 * 
                 * 시나리오:
                 * 1. Page1 → Page2 → Page3 (탐색)
                 * 2. GoBack() → Page2 표시
                 * 3. GoBack() → Page1 표시
                 * 4. GoForward() → Page2 표시
                 * 5. GoForward() → Page3 표시
                 * 
                 * 주의:
                 * - CanGoBack이 false일 때 호출하면 InvalidOperationException
                 * - 반드시 CanGoBack 확인 필요
                 */
            }
            /*
             * CanGoBack이 false인 경우:
             * - 아무 동작도 하지 않음
             * - 이미 첫 페이지
             * - 버튼을 비활성화할 수도 있음 (선택적)
             * 
             * 개선 방안:
             * 뒤로.IsEnabled = frm.NavigationService.CanGoBack;
             * 
             * Frame.Navigated 이벤트:
             * frm.Navigated += (s, args) =>
             * {
             *     뒤로.IsEnabled = frm.NavigationService.CanGoBack;
             *     앞으로.IsEnabled = frm.NavigationService.CanGoForward;
             * };
             */
        }

        /// <summary>
        /// 앞으로 버튼 클릭 이벤트 핸들러
        /// Frame의 NavigationService를 사용하여 다음 페이지로 이동합니다.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체 (앞으로 버튼)</param>
        /// <param name="e">라우트 이벤트 인자</param>
        public void Front_Click(object sender, RoutedEventArgs e) 
        {
            // NavigationService를 통해 다음 페이지로 이동 가능한지 확인
            if (frm.NavigationService.CanGoForward) 
            {
                /*
                 * CanGoForward:
                 * - 다음 페이지가 있는지 확인
                 * - Forward 스택에 항목 존재 여부
                 * 
                 * true인 경우:
                 * - GoBack() 호출 후 Forward 스택에 페이지 있음
                 * - GoForward() 호출 가능
                 * 
                 * false인 경우:
                 * - Forward 스택이 비어 있음
                 * - 새로운 페이지로 Navigate() 호출 시 Forward 스택 초기화
                 * - GoForward() 호출하면 예외 발생
                 * 
                 * 예:
                 * 탐색 기록: [Page1] → [Page2] → [Page3] ← 현재
                 * CanGoForward: false (Forward 스택 비어 있음)
                 * 
                 * GoBack() 호출 후:
                 * 탐색 기록: [Page1] → [Page2] ← 현재 | [Page3]
                 * CanGoForward: true (Page3로 갈 수 있음)
                 * 
                 * Forward 스택 초기화:
                 * [Page1] → [Page2] ← 현재 | [Page3]
                 * Navigate(Page4) 호출
                 * [Page1] → [Page2] → [Page4] ← 현재
                 * Forward 스택의 Page3 제거됨
                 */
                
                // 다음 페이지로 이동
                frm.NavigationService.GoForward(); 
                /*
                 * GoForward():
                 * - Forward 스택에서 페이지를 가져와 표시
                 * - Back 스택에 현재 페이지 추가
                 * 
                 * 동작:
                 * 1. Forward 스택에서 다음 항목 찾기
                 * 2. 해당 Page 인스턴스 복원
                 * 3. Frame에 표시
                 * 4. OnNavigatedFrom 이벤트 (현재 페이지)
                 * 5. OnNavigatedTo 이벤트 (다음 페이지)
                 * 
                 * 예:
                 * 이동 전: [Page1] → [Page2] ← 현재 | [Page3]
                 * GoForward() 호출
                 * 이동 후: [Page1] → [Page2] → [Page3] ← 현재
                 * 
                 * 시나리오:
                 * 1. Page1 → Page2 → Page3 (Navigate)
                 * 2. GoBack() → Page2
                 * 3. GoBack() → Page1
                 *    [Page1] ← 현재 | [Page2] → [Page3]
                 * 4. GoForward() → Page2
                 *    [Page1] → [Page2] ← 현재 | [Page3]
                 * 5. GoForward() → Page3
                 *    [Page1] → [Page2] → [Page3] ← 현재
                 * 
                 * Forward 스택 초기화 시나리오:
                 * 1. Page1 → Page2 → Page3
                 * 2. GoBack() → Page2
                 *    [Page1] → [Page2] ← 현재 | [Page3]
                 * 3. Navigate(Page4) 호출
                 *    [Page1] → [Page2] → [Page4] ← 현재
                 *    Forward 스택의 Page3 제거됨
                 * 4. CanGoForward: false
                 * 
                 * 주의:
                 * - CanGoForward가 false일 때 호출하면 InvalidOperationException
                 * - 반드시 CanGoForward 확인 필요
                 */
            }  
            /*
             * CanGoForward가 false인 경우:
             * - 아무 동작도 하지 않음
             * - Forward 스택이 비어 있음
             * - 버튼을 비활성화할 수도 있음 (선택적)
             */
        }
        
        /*
         * NavigationService 활용 예제:
         * 
         * ========== 1. 버튼 활성화/비활성화 ========= =
         * 
         * Frame.Navigated 이벤트 사용:
         * public MainWindow()
         * {
         *     InitializeComponent();
         *     
         *     frm.Navigated += Frame_Navigated;
         *     UpdateNavigationButtons();
         * }
         * 
         * private void Frame_Navigated(object sender, NavigationEventArgs e)
         * {
         *     UpdateNavigationButtons();
         * }
         * 
         * private void UpdateNavigationButtons()
         * {
         *     뒤로.IsEnabled = frm.NavigationService.CanGoBack;
         *     앞으로.IsEnabled = frm.NavigationService.CanGoForward;
         * }
         * 
         * ========== 2. 탐색 이벤트 처리 ========= =
         * 
         * Navigating 이벤트 (탐색 시작):
         * frm.Navigating += (s, e) =>
         * {
         *     // 탐색 취소 가능
         *     if (hasUnsavedChanges)
         *     {
         *         var result = MessageBox.Show("저장하지 않은 변경사항이 있습니다. 계속하시겠습니까?",
         *                                     "확인", MessageBoxButton.YesNo);
         *         if (result == MessageBoxResult.No)
         *         {
         *             e.Cancel = true; // 탐색 취소
         *         }
         *     }
         * };
         * 
         * Navigated 이벤트 (탐색 완료):
         * frm.Navigated += (s, e) =>
         * {
         *     // 새 페이지 로드 완료
         *     var currentPage = frm.Content as Page;
         *     this.Title = $"MainWindow - {currentPage?.Title}";
         * };
         * 
         * NavigationFailed 이벤트 (탐색 실패):
         * frm.NavigationFailed += (s, e) =>
         * {
         *     MessageBox.Show($"페이지 로드 실패: {e.Exception.Message}");
         *     e.Handled = true; // 예외 처리됨
         * };
         * 
         * ========== 3. 탐색 기록 관리 ========= =
         * 
         * 기록 제거:
         * private void ClearHistory_Click(object sender, RoutedEventArgs e)
         * {
         *     while (frm.NavigationService.CanGoBack)
         *     {
         *         frm.NavigationService.RemoveBackEntry();
         *     }
         * }
         * 
         * 특정 항목 제거:
         * frm.NavigationService.RemoveBackEntry(); // 가장 최근 항목 제거
         * 
         * ========== 4. 프로그램 방식 탐색 ========= =
         * 
         * 코드에서 페이지 이동:
         * private void NavigateToPage2_Click(object sender, RoutedEventArgs e)
         * {
         *     frm.NavigationService.Navigate(new Page2());
         * }
         * 
         * URI 사용:
         * frm.NavigationService.Navigate(new Uri("/Views/Page2.xaml", UriKind.Relative));
         * 
         * 데이터 전달:
         * var data = new UserData { Name = "홍길동", Age = 30 };
         * frm.NavigationService.Navigate(new Page2(), data);
         * 
         * ========== 5. 현재 페이지 정보 ========= =
         * 
         * private void ShowCurrentPage_Click(object sender, RoutedEventArgs e)
         * {
         *     var currentPage = frm.Content as Page;
         *     if (currentPage != null)
         *     {
         *         MessageBox.Show($"현재 페이지: {currentPage.GetType().Name}");
         *     }
         * }
         * 
         * ========== 6. 탐색 스택 확인 ========= =
         * 
         * private void ShowNavigationStack_Click(object sender, RoutedEventArgs e)
         * {
         *     var backStack = new List<string>();
         *     var journal = frm.NavigationService.Journal;
         *     
         *     foreach (var entry in journal.BackStack)
         *     {
         *         backStack.Add(entry.Name);
         *     }
         *     
         *     MessageBox.Show($"Back Stack: {string.Join(" → ", backStack)}");
         * }
         * 
         * ========== 7. 새로고침 (Refresh) ========= =
         * 
         * private void Refresh_Click(object sender, RoutedEventArgs e)
         * {
         *     frm.NavigationService.Refresh();
         *     // 현재 페이지를 다시 로드
         * }
         * 
         * ========== 8. 홈으로 이동 ========= =
         * 
         * private void GoHome_Click(object sender, RoutedEventArgs e)
         * {
         *     while (frm.NavigationService.CanGoBack)
         *     {
         *         frm.NavigationService.GoBack();
         *     }
         *     // 첫 페이지로 이동
         * }
         * 
         * ========== 9. 조건부 탐색 ========= =
         * 
         * private void ConditionalNavigate_Click(object sender, RoutedEventArgs e)
         * {
         *     if (ValidateInput())
         *     {
         *         frm.NavigationService.Navigate(new Page2());
         *     }
         *     else
         *     {
         *         MessageBox.Show("입력을 확인하세요.");
         *     }
         * }
         * 
         * ========== 10. MVVM 패턴과 통합 ========= =
         * 
         * INavigationService 인터페이스:
         * public interface INavigationService
         * {
         *     void Navigate(Type pageType);
         *     void Navigate(Type pageType, object parameter);
         *     void GoBack();
         *     void GoForward();
         *     bool CanGoBack { get; }
         *     bool CanGoForward { get; }
         * }
         * 
         * 구현:
         * public class NavigationService : INavigationService
         * {
         *     private readonly Frame _frame;
         *     
         *     public NavigationService(Frame frame)
         *     {
         *         _frame = frame;
         *     }
         *     
         *     public void Navigate(Type pageType)
         *     {
         *         var page = Activator.CreateInstance(pageType) as Page;
         *         _frame.NavigationService.Navigate(page);
         *     }
         *     
         *     public void Navigate(Type pageType, object parameter)
         *     {
         *         var page = Activator.CreateInstance(pageType) as Page;
         *         _frame.NavigationService.Navigate(page, parameter);
         *     }
         *     
         *     public void GoBack()
         *     {
         *         if (_frame.NavigationService.CanGoBack)
         *             _frame.NavigationService.GoBack();
         *     }
         *     
         *     public void GoForward()
         *     {
         *         if (_frame.NavigationService.CanGoForward)
         *             _frame.NavigationService.GoForward();
         *     }
         *     
         *     public bool CanGoBack => _frame.NavigationService.CanGoBack;
         *     public bool CanGoForward => _frame.NavigationService.CanGoForward;
         * }
         * 
         * MainWindow에서 등록:
         * public MainWindow()
         * {
         *     InitializeComponent();
         *     
         *     var navigationService = new NavigationService(frm);
         *     // DI 컨테이너에 등록
         *     ServiceLocator.Register<INavigationService>(navigationService);
         * }
         * 
         * ViewModel에서 사용:
         * public class MainViewModel
         * {
         *     private readonly INavigationService _navigationService;
         *     
         *     public ICommand NavigateCommand { get; }
         *     public ICommand BackCommand { get; }
         *     
         *     public MainViewModel(INavigationService navigationService)
         *     {
         *         _navigationService = navigationService;
         *         
         *         NavigateCommand = new RelayCommand(() => _navigationService.Navigate(typeof(Page2)));
         *         BackCommand = new RelayCommand(() => _navigationService.GoBack(),
         *                                       () => _navigationService.CanGoBack);
         *     }
         * }
         * 
         * ========== 주의사항 ========= =
         * 
         * 1. 예외 처리:
         *    - CanGoBack/CanGoForward 확인 필수
         *    - NavigationFailed 이벤트 처리
         * 
         * 2. 메모리 관리:
         *    - 탐색 기록이 계속 쌓임
         *    - 필요 시 RemoveBackEntry() 호출
         *    - Page 인스턴스는 GC가 자동 관리
         * 
         * 3. 데이터 전달:
         *    - Navigate(page, data) 사용
         *    - OnNavigatedTo에서 수신
         *    - 복잡한 데이터는 ViewModel 사용
         * 
         * 4. 이벤트 해제:
         *    - Window.Closing에서 이벤트 핸들러 해제
         *    - 메모리 누수 방지
         */
    }
}