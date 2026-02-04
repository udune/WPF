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

namespace ch31_유저컨트롤
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호작용 논리
    /// UserControl1을 여러 개 사용하고 각 인스턴스에 접근하는 예제입니다.
    /// uc1과 uc2의 내부 컨트롤에 접근하여 값을 읽어옵니다.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// MainWindow 생성자
        /// 윈도우가 생성될 때 호출되어 초기화 작업을 수행합니다.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            /*
             * InitializeComponent() 호출 후:
             * 1. StackPanel 생성
             * 2. 3개의 UserControl1 인스턴스 생성
             *    - 첫 번째: 이름 없음 (독립적)
             *    - 두 번째: uc1
             *    - 세 번째: uc2
             * 3. 각 UserControl1 속성 설정
             *    - Title="제목"
             *    - MaxLength="30"
             *    - PbHeight="20"
             * 4. 2개의 Button 생성
             * 5. 이벤트 핸들러 연결
             * 
             * UserControl1 인스턴스 초기화:
             * 각 UserControl1의 생성자 호출:
             * - InitializeComponent()
             * - DataContext = this
             * - 바인딩 활성화
             * 
             * 3개의 인스턴스는 완전히 독립적:
             * - 각자의 메모리 공간
             * - 각자의 UI 요소
             * - 각자의 상태 (TextBox 내용, ProgressBar 값)
             */
        }

        /// <summary>
        /// 버튼1 클릭 이벤트 핸들러
        /// uc1 (두 번째 UserControl1)의 TextBox 내용을 MessageBox로 표시합니다.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체 (버튼1)</param>
        /// <param name="e">라우트 이벤트 인자</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // uc1의 TextBox 내용을 MessageBox로 표시
            MessageBox.Show(uc1.txt.Text);
            /*
             * uc1.txt.Text:
             * 
             * uc1:
             * - MainWindow.xaml에서 정의된 UserControl1 인스턴스
             * - x:Name="uc1"로 선언
             * - 코드 비하인드에서 접근 가능한 필드
             * - 타입: UserControl1
             * 
             * txt:
             * - UserControl1.xaml에서 정의된 TextBox
             * - Name="txt"로 선언
             * - UserControl1 내부의 public 멤버
             * - 타입: TextBox
             * 
             * Text:
             * - TextBox의 Text 속성
             * - 사용자가 입력한 텍스트
             * - 타입: string
             * 
             * 접근 경로:
             * MainWindow
             *   └─ uc1 (UserControl1 인스턴스)
             *       └─ txt (TextBox)
             *           └─ Text (string)
             * 
             * 내부 컨트롤 접근:
             * - UserControl의 Name 속성이 있는 요소는
             *   외부에서 접근 가능
             * - UserControl1.xaml의 txt는 Name="txt"
             * - 따라서 uc1.txt로 접근 가능
             * 
             * MessageBox.Show():
             * - 메시지 박스 표시
             * - uc1의 TextBox에 입력된 내용 표시
             * 
             * 예:
             * 사용자가 uc1의 TextBox에 "안녕하세요" 입력
             * 버튼1 클릭
             * MessageBox에 "안녕하세요" 표시
             * 
             * 다른 인스턴스와의 관계:
             * - 첫 번째 UserControl1 (이름 없음): 접근 불가
             * - uc1 (두 번째): 이 메서드에서 접근
             * - uc2 (세 번째): Button_Click_1에서 접근
             * 
             * 각 인스턴스는 독립적:
             * - uc1의 TextBox와 uc2의 TextBox는 별개
             * - 각각 다른 내용 입력 가능
             */
        }

        /// <summary>
        /// 버튼2 클릭 이벤트 핸들러
        /// uc2 (세 번째 UserControl1)의 ProgressBar 값을 MessageBox로 표시합니다.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체 (버튼2)</param>
        /// <param name="e">라우트 이벤트 인자</param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // uc2의 ProgressBar 값을 MessageBox로 표시
            MessageBox.Show(uc2.Pb.Value.ToString());
            /*
             * uc2.Pb.Value:
             * 
             * uc2:
             * - MainWindow.xaml에서 정의된 UserControl1 인스턴스
             * - x:Name="uc2"로 선언
             * - 세 번째 UserControl1 인스턴스
             * - 타입: UserControl1
             * 
             * Pb:
             * - UserControl1.xaml에서 정의된 ProgressBar
             * - Name="Pb"로 선언
             * - UserControl1 내부의 public 멤버
             * - 타입: ProgressBar
             * 
             * Value:
             * - ProgressBar의 Value 속성
             * - 현재 진행률 값
             * - 타입: double
             * - 이 예제에서는 txt.Text.Length에 바인딩됨
             * 
             * 접근 경로:
             * MainWindow
             *   └─ uc2 (UserControl1 인스턴스)
             *       └─ Pb (ProgressBar)
             *           └─ Value (double)
             * 
             * ToString():
             * - double을 string으로 변환
             * - MessageBox.Show()는 string 필요
             * 
             * ProgressBar 값의 의미:
             * - Value는 현재 글자 수
             * - UserControl1.xaml에서 바인딩:
             *   Value="{Binding ElementName=txt, Path=Text.Length, Mode=OneWay}"
             * - txt.Text.Length → Pb.Value
             * 
             * MessageBox.Show():
             * - ProgressBar의 현재 값 표시
             * - 예: "5" (5글자 입력 시)
             * 
             * 예:
             * 사용자가 uc2의 TextBox에 "Hello" 입력
             * ProgressBar.Value = 5 (글자 수)
             * 버튼2 클릭
             * MessageBox에 "5" 표시
             * 
             * 최대값과의 관계:
             * - Maximum = 30 (MaxLength="30")
             * - Value = 0~30 (입력된 글자 수)
             * - ProgressBar: 0% ~ 100% 표시
             * 
             * 독립성:
             * - uc1의 ProgressBar와 uc2의 ProgressBar는 별개
             * - 각각 해당 TextBox의 글자 수를 표시
             */
        }
        
        /*
         * UserControl 인스턴스 접근 패턴:
         * 
         * ========== 1. 내부 컨트롤 직접 접근 (현재 방법) ==========
         * 
         * 장점:
         * - 간단하고 직접적
         * - 빠른 구현
         * 
         * 단점:
         * - UserControl의 캡슐화 위반
         * - 내부 구조 변경 시 코드 수정 필요
         * - 결합도 높음
         * 
         * 개선 방법:
         * UserControl에 속성 또는 메서드 추가
         * 
         * ========== 2. 속성 노출 (권장) ==========
         * 
         * UserControl1에 속성 추가:
         * public string TextContent
         * {
         *     get { return txt.Text; }
         *     set { txt.Text = value; }
         * }
         * 
         * public double ProgressValue
         * {
         *     get { return Pb.Value; }
         * }
         * 
         * MainWindow에서 사용:
         * MessageBox.Show(uc1.TextContent);
         * MessageBox.Show(uc2.ProgressValue.ToString());
         * 
         * 장점:
         * - 캡슐화 유지
         * - 내부 구조 변경에 유연
         * - 명확한 인터페이스
         * 
         * ========== 3. 의존성 속성 (DependencyProperty) ==========
         * 
         * UserControl1:
         * public static readonly DependencyProperty TextContentProperty =
         *     DependencyProperty.Register("TextContent", typeof(string),
         *         typeof(UserControl1), new PropertyMetadata(string.Empty));
         * 
         * public string TextContent
         * {
         *     get { return (string)GetValue(TextContentProperty); }
         *     set { SetValue(TextContentProperty, value); }
         * }
         * 
         * XAML:
         * <TextBox Text="{Binding TextContent, RelativeSource={RelativeSource AncestorType=UserControl}}"/>
         * 
         * MainWindow:
         * MessageBox.Show(uc1.TextContent);
         * 
         * 장점:
         * - 바인딩 지원
         * - 애니메이션 지원
         * - 스타일 지원
         * - WPF 표준
         * 
         * ========== 4. 이벤트 사용 ==========
         * 
         * UserControl1에 이벤트 추가:
         * public event EventHandler<string> TextSubmitted;
         * 
         * private void SubmitButton_Click(object sender, RoutedEventArgs e)
         * {
         *     TextSubmitted?.Invoke(this, txt.Text);
         * }
         * 
         * MainWindow:
         * public MainWindow()
         * {
         *     InitializeComponent();
         *     
         *     uc1.TextSubmitted += (s, text) =>
         *     {
         *         MessageBox.Show(text);
         *     };
         * }
         * 
         * 장점:
         * - 느슨한 결합
         * - 이벤트 기반 아키텍처
         * - 다중 구독 가능
         * 
         * ========== 5. 메서드 노출 ==========
         * 
         * UserControl1:
         * public string GetText()
         * {
         *     return txt.Text;
         * }
         * 
         * public void SetText(string text)
         * {
         *     txt.Text = text;
         * }
         * 
         * public void ClearText()
         * {
         *     txt.Clear();
         * }
         * 
         * MainWindow:
         * MessageBox.Show(uc1.GetText());
         * uc1.SetText("새 텍스트");
         * uc1.ClearText();
         * 
         * ========== 6. MVVM 패턴 ==========
         * 
         * ViewModel:
         * public class UserControlViewModel : INotifyPropertyChanged
         * {
         *     private string _text;
         *     public string Text
         *     {
         *         get => _text;
         *         set
         *         {
         *             _text = value;
         *             OnPropertyChanged();
         *         }
         *     }
         * }
         * 
         * UserControl1:
         * public UserControl1()
         * {
         *     InitializeComponent();
         *     DataContext = new UserControlViewModel();
         * }
         * 
         * MainWindow:
         * var viewModel = uc1.DataContext as UserControlViewModel;
         * MessageBox.Show(viewModel.Text);
         * 
         * ========== 7. 동적 컨트롤 생성 ==========
         * 
         * private void AddControl_Click(object sender, RoutedEventArgs e)
         * {
         *     var newControl = new UserControl1
         *     {
         *         Title = "새 컨트롤",
         *         MaxLength = "50",
         *         PbHeight = "25"
         *     };
         *     
         *     // 이벤트 구독
         *     newControl.TextSubmitted += NewControl_TextSubmitted;
         *     
         *     stackPanel.Children.Add(newControl);
         * }
         * 
         * private void NewControl_TextSubmitted(object sender, string text)
         * {
         *     MessageBox.Show($"새 컨트롤: {text}");
         * }
         * 
         * ========== 8. 컬렉션으로 관리 ==========
         * 
         * private List<UserControl1> _controls = new List<UserControl1>();
         * 
         * public MainWindow()
         * {
         *     InitializeComponent();
         *     
         *     _controls.Add(uc1);
         *     _controls.Add(uc2);
         * }
         * 
         * private void ShowAllTexts_Click(object sender, RoutedEventArgs e)
         * {
         *     foreach (var control in _controls)
         *     {
         *         MessageBox.Show(control.txt.Text);
         *     }
         * }
         * 
         * ========== 9. 이름으로 찾기 (동적) ==========
         * 
         * private void FindControl_Click(object sender, RoutedEventArgs e)
         * {
         *     var control = this.FindName("uc1") as UserControl1;
         *     if (control != null)
         *     {
         *         MessageBox.Show(control.txt.Text);
         *     }
         * }
         * 
         * ========== 10. 태그 사용 ==========
         * 
         * XAML:
         * <local2:UserControl1 x:Name="uc1" Tag="FirstControl"/>
         * 
         * MainWindow:
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     var tag = uc1.Tag as string;
         *     MessageBox.Show($"{tag}: {uc1.txt.Text}");
         * }
         * 
         * ========== 실전 활용 시나리오 ==========
         * 
         * 1. 폼 유효성 검사:
         * private void Submit_Click(object sender, RoutedEventArgs e)
         * {
         *     var controls = new[] { uc1, uc2 };
         *     
         *     foreach (var control in controls)
         *     {
         *         if (string.IsNullOrWhiteSpace(control.txt.Text))
         *         {
         *             MessageBox.Show($"{control.Title} 입력해주세요.");
         *             return;
         *         }
         *     }
         *     
         *     MessageBox.Show("모든 항목이 입력되었습니다.");
         * }
         * 
         * 2. 데이터 수집:
         * private void CollectData_Click(object sender, RoutedEventArgs e)
         * {
         *     var data = new Dictionary<string, string>
         *     {
         *         ["uc1"] = uc1.txt.Text,
         *         ["uc2"] = uc2.txt.Text
         *     };
         *     
         *     SaveData(data);
         * }
         * 
         * 3. 일괄 초기화:
         * private void ClearAll_Click(object sender, RoutedEventArgs e)
         * {
         *     foreach (var control in stackPanel.Children.OfType<UserControl1>())
         *     {
         *         control.txt.Clear();
         *     }
         * }
         * 
         * ========== 주의사항 ==========
         * 
         * 1. 캡슐화:
         *    - 직접 접근보다 속성/메서드 노출 권장
         *    - 내부 구조 변경에 유연하게 대응
         * 
         * 2. 의존성:
         *    - UserControl과 MainWindow의 결합도 고려
         *    - 느슨한 결합 유지
         * 
         * 3. 이름 충돌:
         *    - x:Name은 고유해야 함
         *    - 동적 생성 시 주의
         * 
         * 4. 메모리:
         *    - 많은 인스턴스 생성 시 메모리 고려
         *    - 불필요한 인스턴스 제거
         * 
         * 5. 성능:
         *    - 많은 UserControl 사용 시 성능 고려
         *    - 가상화 (Virtualization) 고려
         */
    }
}