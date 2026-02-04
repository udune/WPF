using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ch29_화면이동_프레임.Views
{
    /// <summary>
    /// Page1.xaml에 대한 상호 작용 논리
    /// Frame에 호스팅되는 첫 번째 페이지입니다.
    /// "이동" 버튼 클릭 시 Page2로 이동합니다.
    /// </summary>
    public partial class Page1 : Page
    {
        /*
         * Page 클래스 상속:
         * - System.Windows.Controls.Page 상속
         * - Frame에서 호스팅 가능
         * - NavigationService 자동 제공
         * 
         * Window vs Page:
         * - Window: 독립적인 창, Show()/ShowDialog()
         * - Page: Frame 내부, NavigationService.Navigate()
         */
        
        /// <summary>
        /// Page1 생성자
        /// 페이지가 생성될 때 호출되어 초기화 작업을 수행합니다.
        /// </summary>
        public Page1()
        {
            // XAML에 정의된 UI 요소들을 초기화합니다.
            InitializeComponent();
            /*
             * InitializeComponent() 호출 후:
             * 1. Label ("페이지1") 생성
             * 2. Button ("이동") 생성
             * 3. 이벤트 핸들러 연결 (Button_Click)
             * 4. Background = Salmon 적용
             * 
             * Page 생명주기:
             * 1. 생성자 호출 (new Page1())
             * 2. InitializeComponent()
             * 3. Loaded 이벤트
             * 4. OnNavigatedTo (탐색 완료)
             * 5. 화면에 표시
             * 
             * NavigationService:
             * - 생성자에서는 아직 null
             * - Loaded 이벤트 이후 사용 가능
             * - OnNavigatedTo에서 안전하게 사용
             */
        }

        /// <summary>
        /// "이동" 버튼 클릭 이벤트 핸들러
        /// Page2를 생성하고 NavigationService를 통해 이동합니다.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체 (Button)</param>
        /// <param name="e">라우트 이벤트 인자</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Page2 인스턴스 생성
            Page2 page2 = new Page2();
            /*
             * Page2 인스턴스 생성:
             * - new Page2() 호출
             * - Page2 생성자 실행
             * - InitializeComponent() 호출
             * - UI 요소 생성
             * 
             * 매번 새 인스턴스:
             * - Navigate() 호출마다 새 Page 생성
             * - 이전 인스턴스는 GC가 관리
             * - GoBack()으로 복원 가능 (탐색 기록에 저장됨)
             * 
             * 인스턴스 재사용:
             * - Page를 필드로 저장하여 재사용 가능
             * - private Page2 _page2 = new Page2();
             * - NavigationService.Navigate(_page2);
             */
            
            // NavigationService를 사용하여 Page2로 이동
            NavigationService.Navigate(page2);
            /*
             * NavigationService:
             * - Page 클래스의 속성
             * - Frame의 NavigationService와 동일
             * - Frame에 로드된 후 사용 가능
             * 
             * Navigate(page):
             * - 지정된 Page로 이동
             * - Frame에 새 Page 표시
             * - 탐색 기록에 추가
             * 
             * 동작 순서:
             * 1. NavigationService.Navigate(page2) 호출
             * 2. Page1의 OnNavigatedFrom 이벤트
             * 3. Frame에 Page2 로드
             * 4. Page2의 Loaded 이벤트
             * 5. Page2의 OnNavigatedTo 이벤트
             * 6. Page2 화면에 표시
             * 
             * 탐색 기록:
             * 이동 전: [Page1] ← 현재
             * Navigate(page2) 호출
             * 이동 후: [Page1] → [Page2] ← 현재
             * 
             * Back 스택에 Page1 추가:
             * - MainWindow의 "뒤로" 버튼으로 돌아갈 수 있음
             * - CanGoBack: true
             * 
             * Forward 스택 초기화:
             * - 새로운 페이지로 이동 시 Forward 스택 비워짐
             * - 이전에 GoBack()으로 생긴 Forward 기록 제거
             * 
             * 다른 Navigate 방법:
             * 
             * 1. URI 사용:
             *    NavigationService.Navigate(new Uri("/Views/Page2.xaml", UriKind.Relative));
             *    
             * 2. 데이터 전달:
             *    var data = new UserData { Name = "홍길동", Age = 30 };
             *    NavigationService.Navigate(page2, data);
             *    
             * 3. 타입 지정:
             *    NavigationService.Navigate(typeof(Page2));
             */
        }
        
        /*
         * Page 생명주기 이벤트 활용:
         * 
         * ========== 1. OnNavigatedTo (페이지로 이동 완료) ==========
         * 
         * 데이터 수신:
         * protected override void OnNavigatedTo(NavigationEventArgs e)
         * {
         *     base.OnNavigatedTo(e);
         *     
         *     // 전달받은 데이터 처리
         *     if (e.ExtraData is UserData data)
         *     {
         *         nameLabel.Content = data.Name;
         *         ageLabel.Content = data.Age;
         *     }
         *     
         *     // 페이지 로드 완료 후 작업
         *     LoadData();
         *     InitializeControls();
         * }
         * 
         * ========== 2. OnNavigatedFrom (페이지에서 떠남) ==========
         * 
         * 정리 작업:
         * protected override void OnNavigatedFrom(NavigationEventArgs e)
         * {
         *     base.OnNavigatedFrom(e);
         *     
         *     // 데이터 저장
         *     SaveData();
         *     
         *     // 타이머, 이벤트 핸들러 정리
         *     CleanupResources();
         * }
         * 
         * ========== 3. OnNavigatingFrom (페이지 떠나기 전) ==========
         * 
         * 탐색 확인 및 취소:
         * protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
         * {
         *     base.OnNavigatingFrom(e);
         *     
         *     // 저장하지 않은 변경사항 확인
         *     if (hasUnsavedChanges)
         *     {
         *         var result = MessageBox.Show("저장하지 않은 변경사항이 있습니다. 계속하시겠습니까?",
         *                                     "확인", MessageBoxButton.YesNo);
         *         if (result == MessageBoxResult.No)
         *         {
         *             e.Cancel = true; // 탐색 취소
         *         }
         *     }
         *     
         *     // 유효성 검사
         *     if (!ValidateInput())
         *     {
         *         MessageBox.Show("입력을 확인하세요.");
         *         e.Cancel = true;
         *     }
         * }
         * 
         * ========== 4. Loaded 이벤트 ==========
         * 
         * XAML:
         * <Page Loaded="Page_Loaded">
         * 
         * 코드:
         * private void Page_Loaded(object sender, RoutedEventArgs e)
         * {
         *     // 페이지 로드 완료
         *     // UI 요소 초기화
         *     textBox.Focus();
         * }
         * 
         * ========== 5. Unloaded 이벤트 ==========
         * 
         * XAML:
         * <Page Unloaded="Page_Unloaded">
         * 
         * 코드:
         * private void Page_Unloaded(object sender, RoutedEventArgs e)
         * {
         *     // 페이지 언로드
         *     // 리소스 해제
         * }
         * 
         * ========== 데이터 전달 예제 ==========
         * 
         * 1. 단순 데이터:
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     var name = textBox.Text;
         *     NavigationService.Navigate(new Page2(), name);
         * }
         * 
         * Page2에서:
         * protected override void OnNavigatedTo(NavigationEventArgs e)
         * {
         *     base.OnNavigatedTo(e);
         *     
         *     if (e.ExtraData is string name)
         *     {
         *         welcomeLabel.Content = $"환영합니다, {name}님!";
         *     }
         * }
         * 
         * 2. 복잡한 데이터:
         * public class UserData
         * {
         *     public string Name { get; set; }
         *     public int Age { get; set; }
         *     public string Email { get; set; }
         * }
         * 
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     var userData = new UserData
         *     {
         *         Name = nameTextBox.Text,
         *         Age = int.Parse(ageTextBox.Text),
         *         Email = emailTextBox.Text
         *     };
         *     
         *     NavigationService.Navigate(new Page2(), userData);
         * }
         * 
         * 3. 딕셔너리:
         * var parameters = new Dictionary<string, object>
         * {
         *     ["Name"] = "홍길동",
         *     ["Age"] = 30,
         *     ["IsAdmin"] = true
         * };
         * NavigationService.Navigate(new Page2(), parameters);
         * 
         * ========== NavigationService null 체크 ==========
         * 
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     if (NavigationService != null)
         *     {
         *         NavigationService.Navigate(new Page2());
         *     }
         *     else
         *     {
         *         MessageBox.Show("NavigationService를 사용할 수 없습니다.");
         *     }
         * }
         * 
         * NavigationService가 null인 경우:
         * - Page가 Frame에 로드되지 않음
         * - 독립적으로 생성된 Page
         * - 생성자에서 접근 시
         * 
         * ========== 조건부 탐색 ==========
         * 
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     // 입력 유효성 검사
         *     if (string.IsNullOrWhiteSpace(nameTextBox.Text))
         *     {
         *         MessageBox.Show("이름을 입력하세요.");
         *         return;
         *     }
         *     
         *     if (!int.TryParse(ageTextBox.Text, out int age) || age < 0 || age > 150)
         *     {
         *         MessageBox.Show("올바른 나이를 입력하세요.");
         *         return;
         *     }
         *     
         *     // 유효성 검사 통과
         *     var userData = new UserData { Name = nameTextBox.Text, Age = age };
         *     NavigationService.Navigate(new Page2(), userData);
         * }
         * 
         * ========== 비동기 탐색 ==========
         * 
         * private async void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     // 데이터 로딩
         *     loadingIndicator.Visibility = Visibility.Visible;
         *     
         *     var data = await LoadDataAsync();
         *     
         *     loadingIndicator.Visibility = Visibility.Collapsed;
         *     
         *     // 데이터와 함께 탐색
         *     NavigationService.Navigate(new Page2(), data);
         * }
         * 
         * private async Task<UserData> LoadDataAsync()
         * {
         *     await Task.Delay(2000); // 네트워크 요청 시뮬레이션
         *     return new UserData { Name = "홍길동", Age = 30 };
         * }
         * 
         * ========== 페이지 간 통신 (이벤트) ==========
         * 
         * Page1에서 이벤트 정의:
         * public event EventHandler<DataEventArgs> DataSubmitted;
         * 
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     var data = new UserData { Name = nameTextBox.Text };
         *     DataSubmitted?.Invoke(this, new DataEventArgs { Data = data });
         *     NavigationService.Navigate(new Page2());
         * }
         * 
         * MainWindow에서 구독:
         * var page1 = new Page1();
         * page1.DataSubmitted += (s, args) =>
         * {
         *     // 데이터 처리
         *     ProcessData(args.Data);
         * };
         * frm.Navigate(page1);
         * 
         * ========== MVVM 패턴과 Page ==========
         * 
         * ViewModel:
         * public class Page1ViewModel : INotifyPropertyChanged
         * {
         *     private readonly INavigationService _navigationService;
         *     
         *     public ICommand NavigateCommand { get; }
         *     
         *     public Page1ViewModel(INavigationService navigationService)
         *     {
         *         _navigationService = navigationService;
         *         NavigateCommand = new RelayCommand(Navigate);
         *     }
         *     
         *     private void Navigate()
         *     {
         *         _navigationService.Navigate(typeof(Page2));
         *     }
         * }
         * 
         * Page1:
         * public Page1()
         * {
         *     InitializeComponent();
         *     DataContext = new Page1ViewModel(new NavigationService(NavigationService));
         * }
         * 
         * XAML:
         * <Button Content="이동" Command="{Binding NavigateCommand}"/>
         * 
         * ========== 주의사항 ==========
         * 
         * 1. NavigationService null:
         *    - 생성자에서는 null
         *    - Loaded 이벤트 이후 사용
         *    - OnNavigatedTo에서 안전
         * 
         * 2. 메모리 관리:
         *    - 매번 새 Page 인스턴스 생성
         *    - 이벤트 핸들러 해제 필요
         *    - OnNavigatedFrom에서 정리
         * 
         * 3. 데이터 전달:
         *    - Navigate(page, data) 사용
         *    - OnNavigatedTo에서 수신
         *    - ExtraData 타입 확인
         * 
         * 4. 순환 참조:
         *    - Page1 → Page2 → Page1 가능
         *    - 무한 루프 주의
         *    - 조건 확인 필요
         */
    }
}
