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

namespace ch31_유저컨트롤.uc
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// 재사용 가능한 텍스트 입력 컨트롤입니다.
    /// 제목, 최대 글자 수, 진행률 표시 기능을 포함합니다.
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        /*
         * UserControl 상속:
         * - System.Windows.Controls.UserControl 클래스 상속
         * - 재사용 가능한 사용자 정의 컨트롤
         * - XAML과 C# 코드의 조합
         * 
         * partial 클래스:
         * - XAML 파일과 C# 파일이 하나의 클래스 구성
         * - UserControl1.xaml (UI 정의)
         * - UserControl1.xaml.cs (로직 정의)
         * - InitializeComponent()가 자동 생성된 부분에 포함
         */
        
        /// <summary>
        /// 제목 속성
        /// Label의 Content에 바인딩되어 제목을 표시합니다.
        /// </summary>
        public string Title { get; set; }
        /*
         * Title 속성:
         * - 컨트롤의 제목
         * - 외부에서 설정 가능 (public)
         * - 자동 속성 (Auto-Property)
         * 
         * 사용:
         * XAML:
         * <local2:UserControl1 Title="이름"/>
         * 
         * 코드:
         * var control = new UserControl1();
         * control.Title = "이름";
         * 
         * 바인딩:
         * UserControl1.xaml:
         * <Label Content="{Binding Title}"/>
         * 
         * DataContext:
         * - 생성자에서 DataContext = this 설정
         * - this.Title → Label.Content
         * 
         * 현재 구현의 한계:
         * - 런타임에 Title 변경 시 UI 자동 업데이트 안 됨
         * - 생성 시 설정한 값만 반영
         * 
         * 개선 방안:
         * - INotifyPropertyChanged 구현
         * - DependencyProperty 사용 (권장)
         */
        
        /// <summary>
        /// 최대 글자 수 속성
        /// TextBox의 MaxLength와 ProgressBar의 Maximum에 바인딩됩니다.
        /// </summary>
        public string MaxLength { get; set; }
        /*
         * MaxLength 속성:
         * - 입력 가능한 최대 글자 수
         * - 외부에서 설정 가능
         * - 타입: string (XAML에서 쉽게 사용)
         * 
         * 사용:
         * XAML:
         * <local2:UserControl1 MaxLength="30"/>
         * 
         * 바인딩:
         * UserControl1.xaml:
         * <TextBox MaxLength="{Binding MaxLength}"/>
         * <ProgressBar Maximum="{Binding MaxLength}"/>
         * 
         * 타입 변환:
         * - XAML 바인딩이 자동으로 string → int 변환
         * - MaxLength="30" (string) → TextBox.MaxLength = 30 (int)
         * 
         * 개선 방안:
         * public int MaxLength { get; set; } = 100;
         * - int 타입으로 변경
         * - 기본값 설정
         * - 타입 안전성 향상
         * 
         * 유효성 검사:
         * private int _maxLength = 100;
         * public int MaxLength
         * {
         *     get { return _maxLength; }
         *     set
         *     {
         *         if (value > 0 && value <= 1000)
         *             _maxLength = value;
         *     }
         * }
         */
        
        /// <summary>
        /// ProgressBar 높이 속성
        /// ProgressBar의 Height에 바인딩됩니다.
        /// </summary>
        public string PbHeight { get; set; }
        /*
         * PbHeight 속성:
         * - ProgressBar의 높이
         * - 외부에서 설정 가능
         * - 타입: string
         * 
         * 사용:
         * XAML:
         * <local2:UserControl1 PbHeight="20"/>
         * 
         * 바인딩:
         * UserControl1.xaml:
         * <ProgressBar Height="{Binding PbHeight}"/>
         * 
         * 타입 변환:
         * - string → double 자동 변환
         * - PbHeight="20" (string) → Height = 20.0 (double)
         * 
         * 개선 방안:
         * public double PbHeight { get; set; } = 20.0;
         * - double 타입으로 변경
         * - 기본값 20픽셀
         * 
         * 명명 개선:
         * public double ProgressBarHeight { get; set; }
         * - 더 명확한 이름
         */

        /// <summary>
        /// UserControl1 생성자
        /// 컨트롤이 생성될 때 호출되어 초기화 작업을 수행합니다.
        /// </summary>
        public UserControl1()
        {
            // XAML에 정의된 UI 요소들을 초기화합니다.
            InitializeComponent();
            /*
             * InitializeComponent():
             * - XAML 파일 로드 및 파싱
             * - UI 요소 생성 (Grid, Label, TextBox, ProgressBar 등)
             * - 이벤트 핸들러 연결
             * - Name 속성이 있는 요소를 필드로 등록
             * 
             * 자동 생성된 코드:
             * - obj/Debug/.../UserControl1.g.cs 파일에 포함
             * - partial 클래스의 나머지 부분
             * - txt, Pb 등의 필드 선언 포함
             * 
             * 호출 순서:
             * 1. 생성자 진입
             * 2. InitializeComponent() 호출
             * 3. XAML UI 요소 생성
             * 4. DataContext = this 설정
             * 5. 바인딩 활성화
             * 6. 생성자 종료
             */
            
            // DataContext를 현재 인스턴스로 설정하여 바인딩 활성화
            DataContext = this;
            /*
             * DataContext = this:
             * - 바인딩의 소스를 현재 UserControl1 인스턴스로 설정
             * - this는 UserControl1 객체
             * 
             * 바인딩 활성화:
             * XAML:
             * <Label Content="{Binding Title}"/>
             * 
             * 바인딩 해석:
             * 1. DataContext 확인 (this, UserControl1 인스턴스)
             * 2. Title 속성 찾기
             * 3. this.Title 값 읽기
             * 4. Label.Content에 설정
             * 
             * 바인딩 경로:
             * DataContext (this) → Title 속성 → Label.Content
             * 
             * 다른 바인딩:
             * {Binding MaxLength} → this.MaxLength
             * {Binding PbHeight} → this.PbHeight
             * 
             * 왜 필요한가?
             * - DataContext 없으면 바인딩 실패
             * - {Binding Title}이 어디서 Title을 찾을지 모름
             * - DataContext = this로 명확히 지정
             * 
             * 대안:
             * RelativeSource 사용:
             * <Label Content="{Binding Title, RelativeSource={RelativeSource AncestorType=UserControl}}"/>
             * - 하지만 DataContext = this가 더 간단
             */
        }
        
        /*
         * 개선된 UserControl1 예제:
         * 
         * ========== 1. 의존성 속성 (DependencyProperty) ========= =
         * 
         * Title 의존성 속성:
         * public static readonly DependencyProperty TitleProperty =
         *     DependencyProperty.Register("Title", typeof(string),
         *         typeof(UserControl1), new PropertyMetadata(string.Empty, OnTitleChanged));
         * 
         * public string Title
         * {
         *     get { return (string)GetValue(TitleProperty); }
         *     set { SetValue(TitleProperty, value); }
         * }
         * 
         * private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
         * {
         *     var control = d as UserControl1;
         *     // Title 변경 시 추가 작업
         * }
         * 
         * 장점:
         * - 바인딩 완벽 지원
         * - 애니메이션 지원
         * - 스타일 지원
         * - 변경 알림 자동
         * 
         * ========== 2. INotifyPropertyChanged 구현 =========
         * 
         * public class UserControl1 : UserControl, INotifyPropertyChanged
         * {
         *     public event PropertyChangedEventHandler PropertyChanged;
         *     
         *     private string _title;
         *     public string Title
         *     {
         *         get { return _title; }
         *         set
         *         {
         *             if (_title != value)
         *             {
         *                 _title = value;
         *                 OnPropertyChanged();
         *             }
         *         }
         *     }
         *     
         *     protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
         *     {
         *         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         *     }
         * }
         * 
         * 장점:
         * - 런타임에 속성 변경 시 UI 자동 업데이트
         * - 바인딩 변경 알림
         * 
         * ========== 3. 이벤트 추가 ========= =
         * 
         * TextChanged 이벤트:
         * public event EventHandler<TextChangedEventArgs> TextChanged;
         * 
         * 생성자:
         * public UserControl1()
         * {
         *     InitializeComponent();
         *     DataContext = this;
         *     
         *     txt.TextChanged += Txt_TextChanged;
         * }
         * 
         * private void Txt_TextChanged(object sender, TextChangedEventArgs e)
         * {
         *     TextChanged?.Invoke(this, e);
         * }
         * 
         * MainWindow에서 사용:
         * uc1.TextChanged += (s, e) =>
         * {
         *     MessageBox.Show("텍스트 변경됨");
         * };
         * 
         * ========== 4. 메서드 추가 ========= =
         * 
         * 텍스트 가져오기:
         * public string GetText()
         * {
         *     return txt.Text;
         * }
         * 
         * 텍스트 설정:
         * public void SetText(string text)
         * {
         *     txt.Text = text;
         * }
         * 
         * 텍스트 초기화:
         * public void ClearText()
         * {
         *     txt.Clear();
         * }
         * 
         * 진행률 가져오기:
         * public double GetProgress()
         * {
         *     return Pb.Value;
         * }
         * 
         * 유효성 검사:
         * public bool Validate()
         * {
         *     if (IsRequired && string.IsNullOrWhiteSpace(txt.Text))
         *     {
         *         MessageBox.Show($"{Title}을(를) 입력해주세요.");
         *         return false;
         *     }
         *     return true;
         * }
         * 
         * ========== 5. 속성 추가 ========= =
         * 
         * 읽기 전용 속성:
         * public string TextContent => txt.Text;
         * public double ProgressValue => Pb.Value;
         * public int CurrentLength => txt.Text.Length;
         * 
         * 설정 가능 속성:
         * public bool IsRequired { get; set; }
         * public string PlaceholderText { get; set; }
         * 
         * ========== 6. 초기화 개선 ========= =
         * 
         * 기본값 설정:
         * public UserControl1()
         * {
         *     InitializeComponent();
         *     
         *     // 기본값 설정
         *     Title = "제목";
         *     MaxLength = "100";
         *     PbHeight = "20";
         *     
         *     DataContext = this;
         * }
         * 
         * ========== 7. 유효성 검사 ========= =
         * 
         * 입력 제한:
         * private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
         * {
         *     // 숫자만 입력 허용
         *     e.Handled = !int.TryParse(e.Text, out _);
         * }
         * 
         * XAML:
         * <TextBox Name="txt" PreviewTextInput="txt_PreviewTextInput"/>
         * 
         * ========== 8. 스타일 적용 ========= =
         * 
         * 오류 표시:
         * public bool HasError
         * {
         *     get { return (bool)GetValue(HasErrorProperty); }
         *     set { SetValue(HasErrorProperty, value); }
         * }
         * 
         * public static readonly DependencyProperty HasErrorProperty =
         *     DependencyProperty.Register("HasError", typeof(bool),
         *         typeof(UserControl1), new PropertyMetadata(false));
         * 
         * XAML:
         * <Border BorderBrush="{Binding HasError, Converter={StaticResource BoolToColorConverter}}">
         * 
         * ========== 9. 명령 (Command) 추가 ========= =
         * 
         * ICommand 속성:
         * public ICommand SubmitCommand { get; set; }
         * 
         * 생성자:
         * SubmitCommand = new RelayCommand(Submit, CanSubmit);
         * 
         * private void Submit()
         * {
         *     // 제출 로직
         * }
         * 
         * private bool CanSubmit()
         * {
         *     return !string.IsNullOrWhiteSpace(txt.Text);
         * }
         * 
         * XAML:
         * <Button Command="{Binding SubmitCommand}">제출</Button>
         * 
         * ========== 10. 전체 개선 버전 ========= =
         * 
         * public partial class UserControl1 : UserControl, INotifyPropertyChanged
         * {
         *     public event PropertyChangedEventHandler PropertyChanged;
         *     public event EventHandler<string> TextSubmitted;
         *     
         *     // 의존성 속성
         *     public static readonly DependencyProperty TitleProperty =
         *         DependencyProperty.Register("Title", typeof(string),
         *             typeof(UserControl1), new PropertyMetadata("제목"));
         *     
         *     public string Title
         *     {
         *         get { return (string)GetValue(TitleProperty); }
         *         set { SetValue(TitleProperty, value); }
         *     }
         *     
         *     // 일반 속성
         *     private int _maxLength = 100;
         *     public int MaxLength
         *     {
         *         get { return _maxLength; }
         *         set
         *         {
         *             if (_maxLength != value)
         *             {
         *                 _maxLength = value;
         *                 OnPropertyChanged();
         *             }
         *         }
         *     }
         *     
         *     // 읽기 전용 속성
         *     public string TextContent => txt.Text;
         *     public int CurrentLength => txt.Text.Length;
         *     
         *     // 생성자
         *     public UserControl1()
         *     {
         *         InitializeComponent();
         *         DataContext = this;
         *         
         *         txt.TextChanged += Txt_TextChanged;
         *     }
         *     
         *     // 이벤트 핸들러
         *     private void Txt_TextChanged(object sender, TextChangedEventArgs e)
         *     {
         *         OnPropertyChanged(nameof(TextContent));
         *         OnPropertyChanged(nameof(CurrentLength));
         *     }
         *     
         *     // 메서드
         *     public void ClearText()
         *     {
         *         txt.Clear();
         *     }
         *     
         *     public bool Validate()
         *     {
         *         return !string.IsNullOrWhiteSpace(txt.Text);
         *     }
         *     
         *     // INotifyPropertyChanged 구현
         *     protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
         *     {
         *         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         *     }
         * }
         * 
         * ========== 주의사항 ========= =
         * 
         * 1. DataContext:
         *    - 반드시 설정해야 바인딩 작동
         *    - DataContext = this;
         * 
         * 2. 속성 타입:
         *    - string보다 적절한 타입 사용 권장
         *    - int, double, bool 등
         * 
         * 3. 바인딩 모드:
         *    - OneWay: 소스 → 대상
         *    - TwoWay: 양방향 (기본, TextBox.Text)
         *    - OneTime: 한 번만
         * 
         * 4. 의존성 속성:
         *    - 실무에서 권장
         *    - 바인딩, 애니메이션, 스타일 완벽 지원
         * 
         * 5. 캡슐화:
         *    - 내부 컨트롤 직접 노출보다
         *    - 속성/메서드로 간접 노출 권장
         */
    }
}
