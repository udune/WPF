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
    /// Page3.xaml에 대한 상호 작용 논리
    /// Page2에서 이동한 세 번째 페이지입니다.
    /// "첫 화면" 버튼 클릭 시 Page1로 돌아갑니다.
    /// </summary>
    public partial class Page3 : Page
    {
        /// <summary>
        /// Page3 생성자
        /// 페이지가 생성될 때 호출되어 초기화 작업을 수행합니다.
        /// </summary>
        public Page3()
        {
            InitializeComponent();
            /*
             * Page3 초기화:
             * - Label ("페이지3") 생성
             * - Button ("첫 화면") 생성
             * - Background = LightGreen 적용
             * 
             * 탐색 흐름:
             * Page1 → Page2 → [Page3]
             *                  ↑ 현재
             */
        }

        /// <summary>
        /// "첫 화면" 버튼 클릭 이벤트 핸들러
        /// Page1을 생성하고 NavigationService를 통해 이동합니다.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체 (Button)</param>
        /// <param name="e">라우트 이벤트 인자</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Page1 인스턴스 생성
            Page1 page1 = new Page1();
            
            // NavigationService를 사용하여 Page1로 이동
            NavigationService.Navigate(page1);
            /*
             * Page1으로 돌아가기 (순환 탐색):
             * 
             * 탐색 기록:
             * 이동 전: [Page1] → [Page2] → [Page3] ← 현재
             * 이동 후: [Page1] → [Page2] → [Page3] → [Page1] ← 현재
             *          (원래)                       (새 인스턴스)
             * 
             * 새로운 Page1 인스턴스:
             * - 원래의 Page1과 다른 객체
             * - new Page1()로 새로 생성
             * 
             * Back 스택:
             * [Page1 (원래)] → [Page2] → [Page3]
             * 
             * "뒤로" 버튼으로 되돌아가기:
             * Page1 (새) ← Page3 ← Page2 ← Page1 (원래)
             * 
             * 순환 구조:
             * Page1 → Page2 → Page3 → Page1 → Page2 → ...
             * 
             * 주의사항:
             * - 탐색 기록이 계속 쌓임
             * - 메모리 사용 증가
             * - RemoveBackEntry()로 정리 가능
             * 
             * 대안 (GoBack으로 첫 페이지):
             * while (NavigationService.CanGoBack)
             * {
             *     NavigationService.GoBack();
             * }
             * 
             * 이 방법의 장점:
             * - 원래의 Page1 인스턴스로 돌아감
             * - 탐색 기록이 쌓이지 않음
             * - 메모리 효율적
             * 
             * 하지만 이 예제는 순환 탐색을 보여주기 위해
             * 새로운 Page1 인스턴스를 생성합니다.
             */
        }
        
        /*
         * 첫 페이지로 돌아가는 다양한 방법:
         * 
         * ========== 1. 새 인스턴스 생성 (현재 방법) ========= =
         * NavigationService.Navigate(new Page1());
         * 
         * 장점:
         * - 간단한 구현
         * - 새로운 상태로 시작
         * 
         * 단점:
         * - 탐색 기록 계속 쌓임
         * - 메모리 사용 증가
         * 
         * ========== 2. GoBack으로 첫 페이지 ========= =
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     // 첫 페이지까지 계속 뒤로 가기
         *     while (NavigationService.CanGoBack)
         *     {
         *         NavigationService.GoBack();
         *     }
         * }
         * 
         * 장점:
         * - 원래 Page1 인스턴스로 복귀
         * - 탐색 기록 정리
         * - 메모리 효율적
         * 
         * 단점:
         * - 중간 페이지들을 거쳐감
         * - 애니메이션이 여러 번 발생
         * 
         * ========== 3. 탐색 기록 초기화 후 이동 ========= =
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     // 탐색 기록 모두 제거
         *     while (NavigationService.CanGoBack)
         *     {
         *         NavigationService.RemoveBackEntry();
         *     }
         *     
         *     // 새 Page1로 이동
         *     NavigationService.Navigate(new Page1());
         * }
         * 
         * 장점:
         * - 탐색 기록 깨끗이 정리
         * - 뒤로 가기 불가능 (원하는 동작일 수 있음)
         * 
         * 단점:
         * - 이전 페이지로 돌아갈 수 없음
         * 
         * 사용 예:
         * - 로그인 후 메인 페이지
         * - 설정 저장 후 첫 화면
         * 
         * ========== 4. URI 탐색 ========= =
         * NavigationService.Navigate(new Uri("/Views/Page1.xaml", UriKind.Relative));
         * 
         * 장점:
         * - XAML 파일 직접 로드
         * - 경로 기반 탐색
         * 
         * 단점:
         * - 코드 비하인드 생성자 실행 안 될 수 있음
         * - 타입 안정성 낮음
         * 
         * ========== 5. 조건부 탐색 ========= =
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     var result = MessageBox.Show("첫 화면으로 돌아가시겠습니까?",
         *                                 "확인", MessageBoxButton.YesNo);
         *     if (result == MessageBoxResult.Yes)
         *     {
         *         NavigationService.Navigate(new Page1());
         *     }
         * }
         * 
         * ========== 실전 활용 시나리오 ========= =
         * 
         * 1. 마법사 완료:
         *    Page1 (시작) → Page2 (설정) → Page3 (완료)
         *    "완료" 버튼 → 탐색 기록 초기화 후 홈으로
         * 
         * 2. 다단계 폼:
         *    Page1 (기본 정보) → Page2 (연락처) → Page3 (확인)
         *    "제출" 버튼 → 데이터 저장 후 첫 페이지
         * 
         * 3. 튜토리얼:
         *    Page1 (소개) → Page2 (기능 1) → Page3 (기능 2)
         *    "다시 보기" 버튼 → 첫 페이지로
         * 
         * 4. 설정:
         *    Page1 (메인) → Page2 (고급 설정) → Page3 (정보)
         *    "저장" 버튼 → 설정 저장 후 메인으로
         */
    }
}
