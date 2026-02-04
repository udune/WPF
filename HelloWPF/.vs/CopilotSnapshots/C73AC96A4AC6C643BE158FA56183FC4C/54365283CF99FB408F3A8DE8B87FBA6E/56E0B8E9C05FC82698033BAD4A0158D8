using System.Windows;
using ch34_MVVM.Models;
using ch34_MVVM.Commands;

namespace ch34_MVVM.ViewModels
{
    /// <summary>
    /// PersonViewModel - MVVM 패턴의 ViewModel 계층
    /// View와 Model 사이의 중재자 역할을 합니다.
    /// View에 표시할 데이터와 Command를 제공하며 비즈니스 로직을 담당합니다.
    /// 
    /// MVVM의 ViewModel:
    /// - View의 상태와 동작을 관리
    /// - Model의 데이터를 View에 맞게 가공
    /// - Command를 통해 사용자 입력 처리
    /// - View를 알지 못함 (View에 대한 참조 없음)
    /// </summary>
    public class PersonViewModel
    {
        /*
         * ========== MVVM 패턴 개요 ==========
         * 
         * MVVM (Model-View-ViewModel):
         * - WPF의 데이터 바인딩을 활용한 디자인 패턴
         * - Martin Fowler의 Presentation Model에서 파생
         * - Microsoft가 WPF/Silverlight용으로 개발
         * 
         * 구성 요소:
         * 
         * 1. Model:
         *    - 데이터와 비즈니스 로직
         *    - PersonModel (Name, Age)
         *    - INotifyPropertyChanged 구현
         * 
         * 2. View:
         *    - UI (XAML)
         *    - PersonView.xaml
         *    - DataContext로 ViewModel 바인딩
         * 
         * 3. ViewModel:
         *    - View의 상태와 동작
         *    - PersonViewModel (이 클래스)
         *    - Command, 데이터 컬렉션
         * 
         * 데이터 흐름:
         * View ↔ DataBinding ↔ ViewModel ↔ Model
         * 
         * 장점:
         * - View와 로직의 완전한 분리
         * - 테스트 용이 (ViewModel 단위 테스트)
         * - 디자이너와 개발자 협업
         * - 재사용성
         * - 유지보수성
         */
        
        /// <summary>
        /// PersonCommand 속성
        /// 버튼 클릭 등의 사용자 동작을 처리하는 명령입니다.
        /// View의 Button.Command에 바인딩됩니다.
        /// </summary>
        public PersonCommand PersonCommand { get; set; }
        /*
         * PersonCommand:
         * - ICommand 인터페이스 구현
         * - View에서 바인딩
         * - 코드 비하인드 없이 동작 처리
         * 
         * XAML 바인딩:
         * <Button Command="{Binding PersonCommand}"
         *         CommandParameter="{Binding ElementName=tBox1, Path=Text}">
         *     버튼
         * </Button>
         * 
         * 동작:
         * 1. 버튼 클릭
         * 2. PersonCommand.CanExecute 확인
         * 3. PersonCommand.Execute 호출
         * 4. Message 메서드 실행
         * 
         * 다른 Command 예:
         * public ICommand AddCommand { get; set; }
         * public ICommand DeleteCommand { get; set; }
         * public ICommand SaveCommand { get; set; }
         */

        /// <summary>
        /// PersonList 속성
        /// Person 데이터 컬렉션입니다.
        /// View의 ListView.ItemsSource에 바인딩됩니다.
        /// </summary>
        public List<PersonModel> PersonList { get; set; }
        /*
         * PersonList:
         * - PersonModel 컬렉션
         * - View에서 표시할 데이터
         * - ListView에 바인딩
         * 
         * XAML 바인딩:
         * <ListView ItemsSource="{Binding PersonList}">
         * 
         * 타입 선택:
         * 
         * List<T>:
         * - 단순 컬렉션
         * - 변경 알림 없음
         * - 읽기 전용 표시에 적합
         * 
         * ObservableCollection<T>:
         * - 변경 알림 있음
         * - Add/Remove 시 UI 자동 업데이트
         * - 동적 컬렉션에 적합
         * 
         * public ObservableCollection<PersonModel> PersonList { get; set; }
         * 
         * 사용:
         * PersonList.Add(new PersonModel { Name = "새사람", Age = 30 });
         * → ListView에 자동으로 추가됨
         * 
         * PersonList.Remove(selectedPerson);
         * → ListView에서 자동으로 제거됨
         * 
         * 이 예제:
         * - List<PersonModel> 사용
         * - 초기 데이터만 표시
         * - 동적 추가/제거 없음
         */

        /// <summary>
        /// PersonViewModel 생성자
        /// ViewModel 초기화 및 데이터 준비를 수행합니다.
        /// </summary>
        public PersonViewModel()
        {
            /*
             * 생성자에서 수행하는 작업:
             * 1. 초기 데이터 생성
             * 2. Command 초기화
             * 3. 이벤트 핸들러 등록 (필요 시)
             * 
             * 호출 시점:
             * PersonView.xaml.cs:
             * DataContext = new PersonViewModel();
             * 
             * View와 ViewModel 연결:
             * - DataContext 설정으로 바인딩 활성화
             * - View의 모든 바인딩이 이 ViewModel 참조
             */
            
            /*
             * ========== PersonList 초기화 ==========
             */
            PersonList = new List<PersonModel>
            {
                new PersonModel {Name = "홍길동", Age = 100},
                new PersonModel {Name = "임꺽정", Age = 90},
                new PersonModel {Name = "타요", Age = 10},
                new PersonModel {Name = "뽀로로", Age = 12},
                new PersonModel {Name = "뽈리", Age = 7}
            };
            /*
             * Collection Initializer:
             * - C# 3.0 기능
             * - 간결한 컬렉션 초기화
             * 
             * Object Initializer:
             * new PersonModel {Name = "홍길동", Age = 100}
             * - C# 3.0 기능
             * - 속성 초기화 간편화
             * 
             * 동등한 코드:
             * PersonList = new List<PersonModel>();
             * var person1 = new PersonModel();
             * person1.Name = "홍길동";
             * person1.Age = 100;
             * PersonList.Add(person1);
             * ...
             * 
             * 실무 예:
             * - 데이터베이스에서 로드
             * - API 호출로 가져오기
             * - 파일에서 읽기
             * 
             * async 생성자 패턴:
             * public PersonViewModel()
             * {
             *     PersonList = new ObservableCollection<PersonModel>();
             *     _ = LoadDataAsync();
             * }
             * 
             * private async Task LoadDataAsync()
             * {
             *     var data = await _personService.GetAllAsync();
             *     foreach (var person in data)
             *     {
             *         PersonList.Add(person);
             *     }
             * }
             */

            /*
             * ========== PersonCommand 초기화 ==========
             */
            PersonCommand = new PersonCommand(Message, CheckMessage);
            /*
             * PersonCommand 생성:
             * - new PersonCommand(실행_메서드, 실행가능_판단_메서드)
             * - Message: 버튼 클릭 시 실행
             * - CheckMessage: 버튼 활성화 여부 판단
             * 
             * 람다 표현식 사용:
             * PersonCommand = new PersonCommand(
             *     txt => MessageBox.Show(txt),           // Message 대신
             *     txt => !string.IsNullOrEmpty(txt)      // CheckMessage 대신
             * );
             * 
             * 장점:
             * - 간결함
             * - 인라인 정의
             * 
             * 단점:
             * - 복잡한 로직은 가독성 저하
             * - 재사용 불가
             * 
             * 이 예제:
             * - 메서드 참조 사용
             * - 가독성 좋음
             * - 재사용 가능
             */
        }

        /// <summary>
        /// Message 메서드
        /// Command가 실행될 때 호출되어 메시지를 표시합니다.
        /// </summary>
        /// <param name="txt">표시할 텍스트 (CommandParameter)</param>
        private void Message(string? txt)
        {
            /*
             * Message 메서드:
             * - PersonCommand의 Execute에서 호출
             * - 실제 비즈니스 로직
             * 
             * 매개변수 txt:
             * - CommandParameter 값
             * - XAML에서 전달
             * 
             * XAML:
             * <Button Command="{Binding PersonCommand}"
             *         CommandParameter="{Binding ElementName=tBox1, Path=Text}"/>
             * 
             * txt = tBox1.Text
             * 
             * 실행 흐름:
             * 1. 사용자가 버튼 클릭
             * 2. CheckMessage(tBox1.Text) → true
             * 3. PersonCommand.Execute(tBox1.Text)
             * 4. Message(tBox1.Text) 호출
             * 5. MessageBox 표시
             */
            MessageBox.Show(txt);
            /*
             * MessageBox.Show:
             * - WPF/Windows Forms 메시지 박스
             * - 간단한 알림 표시
             * 
             * 실무 예:
             * 
             * 1. 데이터 저장:
             * private void Save(string? name)
             * {
             *     if (string.IsNullOrEmpty(name)) return;
             *     
             *     var person = new PersonModel { Name = name, Age = 0 };
             *     PersonList.Add(person);
             *     MessageBox.Show($"{name}이(가) 추가되었습니다.");
             * }
             * 
             * 2. 선택 항목 삭제:
             * private void Delete(PersonModel? person)
             * {
             *     if (person == null) return;
             *     
             *     PersonList.Remove(person);
             *     MessageBox.Show($"{person.Name}이(가) 삭제되었습니다.");
             * }
             * 
             * 3. 데이터 검증:
             * private void Validate(string? input)
             * {
             *     if (string.IsNullOrEmpty(input))
             *     {
             *         MessageBox.Show("입력값이 없습니다.");
             *         return;
             *     }
             *     
             *     if (input.Length < 2)
             *     {
             *         MessageBox.Show("이름은 2자 이상이어야 합니다.");
             *         return;
             *     }
             *     
             *     MessageBox.Show($"유효한 입력: {input}");
             * }
             * 
             * 4. 비동기 작업:
             * private async void SaveAsync(string? name)
             * {
             *     if (string.IsNullOrEmpty(name)) return;
             *     
             *     try
             *     {
             *         await _personService.SaveAsync(name);
             *         MessageBox.Show("저장 완료!");
             *     }
             *     catch (Exception ex)
             *     {
             *         MessageBox.Show($"오류: {ex.Message}");
             *     }
             * }
             * 
             * 현대적 대안:
             * - ContentDialog (UWP/WinUI)
             * - 커스텀 다이얼로그 (WPF)
             * - Snackbar/Toast (Material Design)
             */
        }

        /// <summary>
        /// CheckMessage 메서드
        /// Command 실행 가능 여부를 판단합니다.
        /// </summary>
        /// <param name="txt">검사할 텍스트 (CommandParameter)</param>
        /// <returns>실행 가능하면 true, 아니면 false</returns>
        private bool CheckMessage(string? txt)
        {
            /*
             * CheckMessage 메서드:
             * - PersonCommand의 CanExecute에서 호출
             * - 버튼 활성화 여부 결정
             * 
             * 매개변수 txt:
             * - Message와 동일한 값
             * - tBox1.Text
             * 
             * 반환값:
             * - true: 버튼 활성화
             * - false: 버튼 비활성화
             * 
             * 실행 흐름:
             * 1. tBox1.Text 변경
             * 2. CommandManager.RequerySuggested 발생
             * 3. CheckMessage(tBox1.Text) 호출
             * 4. 결과에 따라 버튼 상태 변경
             */
            if (txt?.Length > 0) return true;
            return false;
            /*
             * 검증 로직:
             * txt?.Length > 0
             * 
             * ?. (Null-conditional 연산자):
             * - txt가 null이면 전체 식이 null
             * - txt가 null이 아니면 Length 접근
             * 
             * 결과:
             * - txt = null → null > 0 → false
             * - txt = "" → 0 > 0 → false
             * - txt = "a" → 1 > 0 → true
             * 
             * 간결한 버전:
             * return txt?.Length > 0;
             * 
             * 또는:
             * return !string.IsNullOrEmpty(txt);
             * 
             * 실무 검증 예:
             * 
             * 1. 최소 길이:
             * private bool CheckMinLength(string? txt)
             * {
             *     return txt?.Length >= 2;
             * }
             * 
             * 2. 패턴 검증:
             * private bool CheckEmail(string? email)
             * {
             *     if (string.IsNullOrEmpty(email)) return false;
             *     return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
             * }
             * 
             * 3. 복합 조건:
             * private bool CheckCanSave(object? parameter)
             * {
             *     return !string.IsNullOrEmpty(Name) &&
             *            Age > 0 &&
             *            Age < 150 &&
             *            !IsBusy;
             * }
             * 
             * 4. 선택 항목 확인:
             * private bool CheckCanDelete(PersonModel? person)
             * {
             *     return person != null && PersonList.Contains(person);
             * }
             * 
             * 5. 권한 확인:
             * private bool CheckCanEdit(object? parameter)
             * {
             *     return CurrentUser != null && CurrentUser.HasPermission("Edit");
             * }
             * 
             * 주의사항:
             * - 빠르게 실행되어야 함 (자주 호출됨)
             * - 무거운 연산 피하기
             * - 데이터베이스 쿼리 금지
             * - 네트워크 호출 금지
             */
        }
        
        /*
         * ========== ViewModel 추가 기능 예제 ==========
         * 
         * 1. INotifyPropertyChanged 구현:
         * public class PersonViewModel : INotifyPropertyChanged
         * {
         *     public event PropertyChangedEventHandler PropertyChanged;
         *     
         *     private void OnPropertyChanged([CallerMemberName] string name = "")
         *     {
         *         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
         *     }
         *     
         *     private string _searchText;
         *     public string SearchText
         *     {
         *         get => _searchText;
         *         set
         *         {
         *             _searchText = value;
         *             OnPropertyChanged();
         *             FilterPersonList();
         *         }
         *     }
         * }
         * 
         * 2. 선택된 항목:
         * private PersonModel _selectedPerson;
         * public PersonModel SelectedPerson
         * {
         *     get => _selectedPerson;
         *     set
         *     {
         *         _selectedPerson = value;
         *         OnPropertyChanged();
         *         DeleteCommand.RaiseCanExecuteChanged();
         *     }
         * }
         * 
         * 3. 로딩 상태:
         * private bool _isLoading;
         * public bool IsLoading
         * {
         *     get => _isLoading;
         *     set
         *     {
         *         _isLoading = value;
         *         OnPropertyChanged();
         *     }
         * }
         * 
         * 4. 필터링:
         * public ObservableCollection<PersonModel> FilteredPersonList { get; set; }
         * 
         * private void FilterPersonList()
         * {
         *     FilteredPersonList.Clear();
         *     
         *     var filtered = PersonList.Where(p => 
         *         string.IsNullOrEmpty(SearchText) || 
         *         p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
         *     
         *     foreach (var person in filtered)
         *     {
         *         FilteredPersonList.Add(person);
         *     }
         * }
         * 
         * 5. CRUD 작업:
         * public ICommand AddCommand { get; set; }
         * public ICommand EditCommand { get; set; }
         * public ICommand DeleteCommand { get; set; }
         * 
         * public PersonViewModel()
         * {
         *     AddCommand = new RelayCommand(Add, CanAdd);
         *     EditCommand = new RelayCommand(Edit, CanEdit);
         *     DeleteCommand = new RelayCommand(Delete, CanDelete);
         * }
         * 
         * private void Add(object parameter)
         * {
         *     var person = new PersonModel { Name = "새 사람", Age = 0 };
         *     PersonList.Add(person);
         *     SelectedPerson = person;
         * }
         * 
         * private bool CanAdd(object parameter) => true;
         * 
         * private void Delete(object parameter)
         * {
         *     if (SelectedPerson != null)
         *     {
         *         PersonList.Remove(SelectedPerson);
         *         SelectedPerson = PersonList.FirstOrDefault();
         *     }
         * }
         * 
         * private bool CanDelete(object parameter) => SelectedPerson != null;
         * 
         * 6. 데이터 서비스 연동:
         * private readonly IPersonService _personService;
         * 
         * public PersonViewModel(IPersonService personService)
         * {
         *     _personService = personService;
         *     LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
         *     SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
         * }
         * 
         * private async Task LoadDataAsync()
         * {
         *     IsLoading = true;
         *     try
         *     {
         *         var data = await _personService.GetAllAsync();
         *         PersonList.Clear();
         *         foreach (var person in data)
         *         {
         *             PersonList.Add(person);
         *         }
         *     }
         *     finally
         *     {
         *         IsLoading = false;
         *     }
         * }
         * 
         * 7. Messenger 패턴:
         * WeakReferenceMessenger.Default.Register<PersonUpdatedMessage>(this, (r, m) =>
         * {
         *     var person = PersonList.FirstOrDefault(p => p.Id == m.PersonId);
         *     if (person != null)
         *     {
         *         person.Name = m.NewName;
         *         person.Age = m.NewAge;
         *     }
         * });
         * 
         * ========== ViewModel 테스트 ==========
         * 
         * [Fact]
         * public void PersonCommand_WithValidText_ExecutesSuccessfully()
         * {
         *     // Arrange
         *     var viewModel = new PersonViewModel();
         *     var commandParameter = "홍길동";
         *     
         *     // Act
         *     var canExecute = viewModel.PersonCommand.CanExecute(commandParameter);
         *     
         *     // Assert
         *     Assert.True(canExecute);
         * }
         * 
         * [Fact]
         * public void PersonCommand_WithEmptyText_CannotExecute()
         * {
         *     // Arrange
         *     var viewModel = new PersonViewModel();
         *     var commandParameter = "";
         *     
         *     // Act
         *     var canExecute = viewModel.PersonCommand.CanExecute(commandParameter);
         *     
         *     // Assert
         *     Assert.False(canExecute);
         * }
         * 
         * ========== 주의사항 ==========
         * 
         * 1. View 참조 금지:
         *    - ViewModel은 View를 알지 못해야 함
         *    - MessageBox 대신 Dialog Service 사용 고려
         * 
         * 2. 메모리 누수:
         *    - 이벤트 핸들러 해제
         *    - WeakReference 사용
         * 
         * 3. 스레드 안전성:
         *    - UI 스레드에서만 컬렉션 변경
         *    - Dispatcher 사용
         * 
         * 4. 성능:
         *    - ObservableCollection 대신 List (읽기 전용)
         *    - 필터링은 별도 컬렉션
         * 
         * 5. 테스트:
         *    - ViewModel은 순수 C# (테스트 용이)
         *    - Dependency Injection 활용
         */
    }
}
