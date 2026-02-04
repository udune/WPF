using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ch34_MVVM.Commands
{
    /// <summary>
    /// PersonCommand - MVVM 패턴의 Command 구현
    /// ICommand 인터페이스를 구현하여 View에서 사용자 동작을 처리합니다.
    /// 버튼 클릭 등의 이벤트를 코드 비하인드 없이 ViewModel에서 처리할 수 있게 합니다.
    /// 
    /// ICommand:
    /// - WPF의 명령 패턴 인터페이스
    /// - Button, MenuItem 등의 Command 속성에 바인딩
    /// - 이벤트 핸들러의 대안
    /// - MVVM의 핵심 메커니즘
    /// </summary>
    public class PersonCommand : ICommand
    {
        /*
         * ========== ICommand 인터페이스 ==========
         * 
         * 필수 구현 멤버:
         * 1. CanExecuteChanged 이벤트
         * 2. CanExecute 메서드
         * 3. Execute 메서드
         * 
         * 목적:
         * - View와 ViewModel의 느슨한 결합
         * - 재사용 가능한 명령
         * - 실행 가능 여부 제어
         * 
         * 장점:
         * - 코드 비하인드 제거
         * - 테스트 용이
         * - 비즈니스 로직 분리
         * 
         * XAML 사용:
         * <Button Command="{Binding PersonCommand}" CommandParameter="파라미터"/>
         * 
         * 작동 과정:
         * 1. 버튼 클릭
         * 2. CanExecute 확인 (실행 가능 여부)
         * 3. Execute 호출 (실제 동작 실행)
         */
        
        /// <summary>
        /// CanExecuteChanged 이벤트
        /// 명령의 실행 가능 여부가 변경되었을 때 발생합니다.
        /// WPF가 이 이벤트를 구독하여 Button의 IsEnabled를 자동으로 업데이트합니다.
        /// </summary>
        public event EventHandler? CanExecuteChanged;
        /*
         * CanExecuteChanged:
         * - 명령 실행 가능 여부 변경 알림
         * - CommandManager.RequerySuggested와 연동
         * 
         * 이벤트 발생 시점:
         * - CanExecute 결과가 변경될 때
         * - UI 입력 상태 변경 시
         * 
         * 수동 발생:
         * CanExecuteChanged?.Invoke(this, EventArgs.Empty);
         * 
         * CommandManager 사용:
         * public event EventHandler CanExecuteChanged
         * {
         *     add { CommandManager.RequerySuggested += value; }
         *     remove { CommandManager.RequerySuggested -= value; }
         * }
         * 
         * CommandManager.RequerySuggested:
         * - WPF가 자동으로 발생
         * - 포커스 변경, 키 입력 등
         * - 모든 Command의 CanExecute 재평가
         * 
         * 결과:
         * - CanExecute가 false면 Button이 비활성화
         * - CanExecute가 true면 Button이 활성화
         */

        /*
         * ========== 델리게이트 필드 ==========
         * 
         * Action<string?>:
         * - 반환값이 없는 메서드 델리게이트
         * - string? 매개변수 1개
         * - void Method(string? param) 시그니처
         * 
         * Predicate<string?>:
         * - bool을 반환하는 메서드 델리게이트
         * - string? 매개변수 1개
         * - bool Method(string? param) 시그니처
         * 
         * 목적:
         * - Execute와 CanExecute의 실제 로직 저장
         * - 생성자에서 주입 (Dependency Injection)
         * - 유연한 명령 구현
         */
        Action<string?> execute;
        /*
         * execute:
         * - 명령이 실행될 때 호출할 메서드
         * - 실제 비즈니스 로직
         * - ViewModel에서 정의
         * 
         * 예:
         * private void Message(string? txt)
         * {
         *     MessageBox.Show(txt);
         * }
         * 
         * 전달:
         * new PersonCommand(Message, CheckMessage);
         */
        Predicate<string?> canExecute;
        /*
         * canExecute:
         * - 명령 실행 가능 여부를 판단하는 메서드
         * - true: 버튼 활성화
         * - false: 버튼 비활성화
         * 
         * 예:
         * private bool CheckMessage(string? txt)
         * {
         *     return txt?.Length > 0;
         * }
         * 
         * 결과:
         * - 텍스트가 있으면 버튼 활성화
         * - 텍스트가 없으면 버튼 비활성화
         */

        /// <summary>
        /// PersonCommand 생성자
        /// 명령이 실행할 동작과 실행 가능 여부를 판단하는 로직을 주입받습니다.
        /// </summary>
        /// <param name="msg">실행할 동작 (Action)</param>
        /// <param name="check">실행 가능 여부 판단 (Predicate)</param>
        public PersonCommand(Action<string?> msg, Predicate<string?> check)
        {
            /*
             * 생성자 매개변수:
             * 
             * Action<string?> msg:
             * - Execute에서 호출할 메서드
             * - 실제 명령 로직
             * 
             * Predicate<string?> check:
             * - CanExecute에서 호출할 메서드
             * - 실행 가능 여부 판단
             * 
             * Dependency Injection:
             * - 생성자를 통한 의존성 주입
             * - 느슨한 결합
             * - 테스트 용이
             * 
             * 사용 예 (ViewModel):
             * PersonCommand = new PersonCommand(
             *     txt => MessageBox.Show(txt),           // execute
             *     txt => !string.IsNullOrEmpty(txt)      // canExecute
             * );
             * 
             * 또는:
             * PersonCommand = new PersonCommand(Message, CheckMessage);
             * 
             * private void Message(string? txt)
             * {
             *     MessageBox.Show(txt);
             * }
             * 
             * private bool CheckMessage(string? txt)
             * {
             *     return txt?.Length > 0;
             * }
             */
            execute = msg;
            canExecute = check;
        }

        /// <summary>
        /// CanExecute - 명령 실행 가능 여부 판단
        /// 이 메서드의 반환값에 따라 버튼의 활성화/비활성화가 결정됩니다.
        /// </summary>
        /// <param name="parameter">명령 매개변수 (CommandParameter)</param>
        /// <returns>실행 가능하면 true, 아니면 false</returns>
        public bool CanExecute(object? parameter)
        {
            /*
             * CanExecute 메서드:
             * - 명령 실행 전 호출
             * - 버튼 활성화 여부 결정
             * - 자동으로 자주 호출됨
             * 
             * parameter:
             * - CommandParameter 바인딩 값
             * - object 타입 (모든 타입 가능)
             * - null 가능
             * 
             * XAML:
             * <Button Command="{Binding PersonCommand}"
             *         CommandParameter="{Binding ElementName=tBox1, Path=Text}"/>
             * 
             * parameter = tBox1.Text
             * 
             * 타입 변환:
             * parameter as string
             * - 안전한 캐스팅
             * - 실패 시 null 반환
             * 
             * canExecute.Invoke:
             * - Predicate 델리게이트 호출
             * - 생성자에서 주입받은 메서드 실행
             * 
             * 예:
             * private bool CheckMessage(string? txt)
             * {
             *     if (txt?.Length > 0) return true;
             *     return false;
             * }
             * 
             * 간단히:
             * private bool CheckMessage(string? txt) => !string.IsNullOrEmpty(txt);
             * 
             * 결과:
             * - txt가 비어있지 않으면 true (버튼 활성화)
             * - txt가 비어있으면 false (버튼 비활성화)
             * 
             * 호출 시점:
             * - 처음 바인딩될 때
             * - CanExecuteChanged 이벤트 발생 시
             * - CommandManager.RequerySuggested 발생 시
             * - 포커스 변경, 텍스트 입력 등
             * 
             * 성능 고려:
             * - 자주 호출되므로 빠르게 실행되어야 함
             * - 무거운 로직은 피해야 함
             * - 단순한 검증만 수행
             */
            return canExecute.Invoke(parameter as string);
        }

        /// <summary>
        /// Execute - 명령 실행
        /// 버튼이 클릭되면 이 메서드가 호출되어 실제 동작을 수행합니다.
        /// </summary>
        /// <param name="parameter">명령 매개변수 (CommandParameter)</param>
        public void Execute(object? parameter)
        {
            /*
             * Execute 메서드:
             * - 명령의 실제 동작 수행
             * - 버튼 클릭 시 호출
             * - CanExecute가 true일 때만 호출됨
             * 
             * parameter:
             * - CommandParameter 바인딩 값
             * - CanExecute의 parameter와 동일
             * 
             * XAML:
             * <Button Command="{Binding PersonCommand}"
             *         CommandParameter="{Binding ElementName=tBox1, Path=Text}"/>
             * 
             * 버튼 클릭:
             * 1. CanExecute 확인 (true/false)
             * 2. true이면 Execute 호출
             * 3. parameter로 tBox1.Text 전달
             * 
             * execute.Invoke:
             * - Action 델리게이트 호출
             * - 생성자에서 주입받은 메서드 실행
             * 
             * 예 (ViewModel):
             * private void Message(string? txt)
             * {
             *     MessageBox.Show(txt);
             * }
             * 
             * PersonCommand = new PersonCommand(Message, CheckMessage);
             * 
             * 실행 흐름:
             * 1. 사용자가 버튼 클릭
             * 2. WPF가 CanExecute(parameter) 호출
             * 3. CheckMessage(tBox1.Text) 실행 → true 반환
             * 4. WPF가 Execute(parameter) 호출
             * 5. Message(tBox1.Text) 실행
             * 6. MessageBox.Show(tBox1.Text) 표시
             * 
             * 비동기 실행:
             * async void Execute(object? parameter)
             * {
             *     await execute.Invoke(parameter as string);
             * }
             * 
             * 예외 처리:
             * public void Execute(object? parameter)
             * {
             *     try
             *     {
             *         execute.Invoke(parameter as string);
             *     }
             *     catch (Exception ex)
             *     {
             *         MessageBox.Show($"오류: {ex.Message}");
             *     }
             * }
             */
            execute.Invoke(parameter as string);
        }
        
        /*
         * ========== PersonCommand 사용 예제 ==========
         * 
         * ViewModel:
         * public class PersonViewModel
         * {
         *     public PersonCommand PersonCommand { get; set; }
         *     
         *     public PersonViewModel()
         *     {
         *         PersonCommand = new PersonCommand(Message, CheckMessage);
         *     }
         *     
         *     private void Message(string? txt)
         *     {
         *         MessageBox.Show(txt);
         *     }
         *     
         *     private bool CheckMessage(string? txt)
         *     {
         *         return !string.IsNullOrEmpty(txt);
         *     }
         * }
         * 
         * XAML:
         * <Button Command="{Binding PersonCommand}"
         *         CommandParameter="{Binding ElementName=tBox1, Path=Text}">
         *     버튼
         * </Button>
         * 
         * 동작:
         * 1. tBox1.Text가 비어있으면 버튼 비활성화
         * 2. tBox1.Text에 입력하면 버튼 활성화
         * 3. 버튼 클릭 시 tBox1.Text를 MessageBox로 표시
         * 
         * ========== 일반적인 Command 구현 패턴 ==========
         * 
         * 1. RelayCommand (가장 일반적):
         * public class RelayCommand : ICommand
         * {
         *     private readonly Action<object> _execute;
         *     private readonly Predicate<object> _canExecute;
         *     
         *     public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
         *     {
         *         _execute = execute ?? throw new ArgumentNullException(nameof(execute));
         *         _canExecute = canExecute;
         *     }
         *     
         *     public event EventHandler CanExecuteChanged
         *     {
         *         add { CommandManager.RequerySuggested += value; }
         *         remove { CommandManager.RequerySuggested -= value; }
         *     }
         *     
         *     public bool CanExecute(object parameter)
         *     {
         *         return _canExecute == null || _canExecute(parameter);
         *     }
         *     
         *     public void Execute(object parameter)
         *     {
         *         _execute(parameter);
         *     }
         * }
         * 
         * 2. 제네릭 RelayCommand<T>:
         * public class RelayCommand<T> : ICommand
         * {
         *     private readonly Action<T> _execute;
         *     private readonly Predicate<T> _canExecute;
         *     
         *     public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
         *     {
         *         _execute = execute ?? throw new ArgumentNullException(nameof(execute));
         *         _canExecute = canExecute;
         *     }
         *     
         *     public bool CanExecute(object parameter)
         *     {
         *         return _canExecute == null || _canExecute((T)parameter);
         *     }
         *     
         *     public void Execute(object parameter)
         *     {
         *         _execute((T)parameter);
         *     }
         *     
         *     public event EventHandler CanExecuteChanged
         *     {
         *         add { CommandManager.RequerySuggested += value; }
         *         remove { CommandManager.RequerySuggested -= value; }
         *     }
         * }
         * 
         * 사용:
         * public RelayCommand<string> SaveCommand { get; }
         * 
         * SaveCommand = new RelayCommand<string>(
         *     txt => Save(txt),
         *     txt => !string.IsNullOrEmpty(txt)
         * );
         * 
         * 3. AsyncRelayCommand (비동기):
         * public class AsyncRelayCommand : ICommand
         * {
         *     private readonly Func<Task> _execute;
         *     private readonly Func<bool> _canExecute;
         *     private bool _isExecuting;
         *     
         *     public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
         *     {
         *         _execute = execute;
         *         _canExecute = canExecute;
         *     }
         *     
         *     public event EventHandler CanExecuteChanged
         *     {
         *         add { CommandManager.RequerySuggested += value; }
         *         remove { CommandManager.RequerySuggested -= value; }
         *     }
         *     
         *     public bool CanExecute(object parameter)
         *     {
         *         return !_isExecuting && (_canExecute?.Invoke() ?? true);
         *     }
         *     
         *     public async void Execute(object parameter)
         *     {
         *         _isExecuting = true;
         *         RaiseCanExecuteChanged();
         *         
         *         try
         *         {
         *             await _execute();
         *         }
         *         finally
         *         {
         *             _isExecuting = false;
         *             RaiseCanExecuteChanged();
         *         }
         *     }
         *     
         *     public void RaiseCanExecuteChanged()
         *     {
         *         CommandManager.InvalidateRequerySuggested();
         *     }
         * }
         * 
         * 사용:
         * public AsyncRelayCommand LoadDataCommand { get; }
         * 
         * LoadDataCommand = new AsyncRelayCommand(
         *     async () => await LoadDataAsync(),
         *     () => !IsLoading
         * );
         * 
         * 4. MVVM Toolkit (CommunityToolkit.Mvvm):
         * [RelayCommand(CanExecute = nameof(CanSave))]
         * private void Save(string text)
         * {
         *     MessageBox.Show(text);
         * }
         * 
         * private bool CanSave(string text)
         * {
         *     return !string.IsNullOrEmpty(text);
         * }
         * 
         * // 자동 생성됨:
         * // public ICommand SaveCommand { get; }
         * 
         * ========== Command vs 이벤트 핸들러 ==========
         * 
         * 이벤트 핸들러 (코드 비하인드):
         * XAML:
         * <Button Click="Button_Click">버튼</Button>
         * 
         * 코드:
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     MessageBox.Show("클릭!");
         * }
         * 
         * 문제점:
         * - View와 로직이 결합
         * - 테스트 어려움
         * - 재사용 불가
         * 
         * Command (MVVM):
         * XAML:
         * <Button Command="{Binding ClickCommand}">버튼</Button>
         * 
         * ViewModel:
         * public ICommand ClickCommand { get; }
         * 
         * ClickCommand = new RelayCommand(() => MessageBox.Show("클릭!"));
         * 
         * 장점:
         * - View와 로직 분리
         * - 테스트 용이
         * - 재사용 가능
         * - CanExecute로 버튼 상태 제어
         * 
         * ========== 주의사항 ==========
         * 
         * 1. CanExecute 성능:
         *    - 자주 호출되므로 빠르게 실행
         *    - 무거운 연산 피하기
         * 
         * 2. CanExecuteChanged:
         *    - CommandManager.RequerySuggested 사용 권장
         *    - 수동 발생은 필요 시에만
         * 
         * 3. 메모리 누수:
         *    - CommandManager.RequerySuggested는 강한 참조
         *    - WeakEventManager 고려
         * 
         * 4. async void:
         *    - Execute에서 async void 사용 시 주의
         *    - 예외 처리 필수
         * 
         * 5. 타입 안전성:
         *    - 제네릭 Command<T> 사용 권장
         *    - parameter 타입 검증
         */
    }
}
