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
using System.Windows.Shapes;

namespace ch30_탭컨트롤_모달_모달리스
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// MainWindow에서 모달/모달리스로 열리는 테스트용 윈도우입니다.
    /// "닫기" 버튼으로 창을 닫을 수 있습니다.
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        /// Window1 생성자
        /// 윈도우가 생성될 때 호출되어 초기화 작업을 수행합니다.
        /// </summary>
        public Window1()
        {
            InitializeComponent();
            /*
             * InitializeComponent() 호출 후:
             * 1. StackPanel 생성
             * 2. Button ("닫기") 생성
             * 3. 이벤트 핸들러 연결 (Button_Click)
             * 
             * 초기 상태:
             * - 창이 아직 화면에 표시되지 않음
             * - MainWindow에서 Show() 또는 ShowDialog() 호출 필요
             */
        }

        /// <summary>
        /// "닫기" 버튼 클릭 이벤트 핸들러
        /// Close() 메서드를 호출하여 창을 닫습니다.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체 (닫기 버튼)</param>
        /// <param name="e">라우트 이벤트 인자</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 창 닫기
            this.Close();
            /*
             * Close() 메서드:
             * - 현재 윈도우를 닫음
             * - Closing 이벤트 발생
             * - Closed 이벤트 발생
             * - 리소스 해제
             * 
             * 모달리스 (Show()):
             * - 창이 닫힘
             * - MainWindow는 계속 활성 상태
             * - 다른 창에 영향 없음
             * 
             * 모달 (ShowDialog()):
             * - 창이 닫힘
             * - ShowDialog()가 null 반환
             * - MainWindow 다시 활성화
             * - 부모 창으로 제어권 반환
             * 
             * DialogResult:
             * - Close()만 호출하면 DialogResult = null
             * - ShowDialog()의 반환값 = null
             * 
             * DialogResult 설정:
             * this.DialogResult = true;  // 확인
             * this.DialogResult = false; // 취소
             * // DialogResult 설정 시 자동으로 Close() 호출됨
             * // 명시적으로 Close() 호출할 필요 없음
             */
        }
        
        /*
         * DialogResult 활용 패턴:
         * 
         * ========== 1. 확인/취소 대화상자 ========= =
         * 
         * XAML:
         * <StackPanel>
         *     <Button Content="확인" Click="OK_Click"/>
         *     <Button Content="취소" Click="Cancel_Click"/>
         * </StackPanel>
         * 
         * 코드:
         * private void OK_Click(object sender, RoutedEventArgs e)
         * {
         *     // 데이터 유효성 검사
         *     if (ValidateInput())
         *     {
         *         this.DialogResult = true; // 확인
         *         // Close()는 자동 호출됨
         *     }
         *     else
         *     {
         *         MessageBox.Show("입력을 확인하세요.");
         *     }
         * }
         * 
         * private void Cancel_Click(object sender, RoutedEventArgs e)
         * {
         *     this.DialogResult = false; // 취소
         *     // Close()는 자동 호출됨
         * }
         * 
         * MainWindow:
         * Window1 window1 = new Window1();
         * if (window1.ShowDialog() == true)
         * {
         *     // 확인 버튼 클릭
         *     ProcessData();
         * }
         * 
         * ========== 2. 데이터 입력 대화상자 ========= =
         * 
         * Window1에 속성 추가:
         * public string UserName { get; set; }
         * public int UserAge { get; set; }
         * 
         * XAML:
         * <StackPanel Margin="10">
         *     <Label Content="이름:"/>
         *     <TextBox x:Name="nameTextBox"/>
         *     <Label Content="나이:"/>
         *     <TextBox x:Name="ageTextBox"/>
         *     <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
         *         <Button Content="확인" Click="OK_Click" Width="70" Margin="5"/>
         *         <Button Content="취소" Click="Cancel_Click" Width="70" Margin="5"/>
         *     </StackPanel>
         * </StackPanel>
         * 
         * 코드:
         * private void OK_Click(object sender, RoutedEventArgs e)
         * {
         *     // 입력값 검증
         *     if (string.IsNullOrWhiteSpace(nameTextBox.Text))
         *     {
         *         MessageBox.Show("이름을 입력하세요.");
         *         return;
         *     }
         *     
         *     if (!int.TryParse(ageTextBox.Text, out int age))
         *     {
         *         MessageBox.Show("올바른 나이를 입력하세요.");
         *         return;
         *     }
         *     
         *     // 속성에 저장
         *     UserName = nameTextBox.Text;
         *     UserAge = age;
         *     
         *     this.DialogResult = true;
         * }
         * 
         * MainWindow:
         * Window1 window1 = new Window1();
         * if (window1.ShowDialog() == true)
         * {
         *     string name = window1.UserName;
         *     int age = window1.UserAge;
         *     MessageBox.Show($"이름: {name}, 나이: {age}");
         * }
         * 
         * ========== 3. 초기 데이터 전달 ========= =
         * 
         * Window1에 속성 추가:
         * public string InitialValue { get; set; }
         * public string ResultValue { get; private set; }
         * 
         * 생성자 오버로드:
         * public Window1(string initialValue)
         * {
         *     InitializeComponent();
         *     InitialValue = initialValue;
         *     textBox.Text = initialValue;
         * }
         * 
         * MainWindow:
         * Window1 window1 = new Window1("초기값");
         * if (window1.ShowDialog() == true)
         * {
         *     string result = window1.ResultValue;
         * }
         * 
         * ========== 4. Closing 이벤트 처리 (취소 가능) ========= =
         * 
         * XAML:
         * <Window Closing="Window_Closing">
         * 
         * 코드:
         * private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
         * {
         *     if (hasUnsavedChanges)
         *     {
         *         var result = MessageBox.Show("저장하지 않은 변경사항이 있습니다. 닫으시겠습니까?",
         *                                     "확인", MessageBoxButton.YesNo);
         *         if (result == MessageBoxResult.No)
         *         {
         *             e.Cancel = true; // 닫기 취소
         *         }
         *     }
         * }
         * 
         * ========== 5. Closed 이벤트 처리 (정리 작업) ========= =
         * 
         * XAML:
         * <Window Closed="Window_Closed">
         * 
         * 코드:
         * private void Window_Closed(object sender, EventArgs e)
         * {
         *     // 리소스 해제
         *     CleanupResources();
         *     
         *     // 이벤트 핸들러 해제
         *     UnsubscribeEvents();
         * }
         * 
         * ========== 6. ESC 키로 취소 ========= =
         * 
         * XAML:
         * <Button Content="취소" Click="Cancel_Click" IsCancel="True"/>
         * 
         * IsCancel="True":
         * - ESC 키 누르면 자동으로 클릭됨
         * - DialogResult = false 자동 설정
         * 
         * ========== 7. Enter 키로 확인 ========= =
         * 
         * XAML:
         * <Button Content="확인" Click="OK_Click" IsDefault="True"/>
         * 
         * IsDefault="True":
         * - Enter 키 누르면 자동으로 클릭됨
         * - 기본 버튼으로 설정
         * 
         * ========== 8. 비동기 작업 후 닫기 ========= =
         * 
         * private async void OK_Click(object sender, RoutedEventArgs e)
         * {
         *     // 버튼 비활성화
         *     okButton.IsEnabled = false;
         *     cancelButton.IsEnabled = false;
         *     
         *     // 비동기 작업
         *     await SaveDataAsync();
         *     
         *     this.DialogResult = true;
         * }
         * 
         * ========== 9. Owner 설정 확인 ========= =
         * 
         * public Window1()
         * {
         *     InitializeComponent();
         *     
         *     // Owner가 설정되었는지 확인
         *     this.Loaded += (s, e) =>
         *     {
         *         if (this.Owner != null)
         *         {
         *             // Owner 중앙에 위치
         *             this.Left = Owner.Left + (Owner.Width - this.Width) / 2;
         *             this.Top = Owner.Top + (Owner.Height - this.Height) / 2;
         *         }
         *     };
         * }
         * 
         * ========== 10. 결과 객체 반환 ========= =
         * 
         * 결과 클래스:
         * public class DialogResult
         * {
         *     public bool Success { get; set; }
         *     public string Message { get; set; }
         *     public object Data { get; set; }
         * }
         * 
         * Window1:
         * public DialogResult Result { get; private set; }
         * 
         * private void OK_Click(object sender, RoutedEventArgs e)
         * {
         *     Result = new DialogResult
         *     {
         *         Success = true,
         *         Message = "성공",
         *         Data = CollectData()
         *     };
         *     
         *     this.DialogResult = true;
         * }
         * 
         * MainWindow:
         * Window1 window1 = new Window1();
         * if (window1.ShowDialog() == true)
         * {
         *     var result = window1.Result;
         *     ProcessResult(result);
         * }
         * 
         * ========== 주의사항 ========= =
         * 
         * 1. DialogResult 설정:
         *    - 모달 창에서만 의미 있음
         *    - 모달리스 창에서는 무시됨
         * 
         * 2. Close() vs DialogResult:
         *    - DialogResult 설정 시 자동으로 Close() 호출
         *    - Close()만 호출 시 DialogResult = null
         * 
         * 3. 유효성 검사:
         *    - DialogResult = true 설정 전에 검증
         *    - 유효하지 않으면 return하여 창 유지
         * 
         * 4. 이벤트 해제:
         *    - Closed 이벤트에서 리소스 해제
         *    - 메모리 누수 방지
         * 
         * 5. 비동기 작업:
         *    - async/await 사용 시 버튼 비활성화
         *    - 중복 클릭 방지
         */
    }
}
