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
    /// Page2.xaml에 대한 상호 작용 논리
    /// Page1에서 이동한 두 번째 페이지입니다.
    /// "이동" 버튼 클릭 시 Page3로 이동합니다.
    /// </summary>
    public partial class Page2 : Page
    {
        /// <summary>
        /// Page2 생성자
        /// 페이지가 생성될 때 호출되어 초기화 작업을 수행합니다.
        /// </summary>
        public Page2()
        {
            InitializeComponent();
            /*
             * Page2 초기화:
             * - Label ("페이지2") 생성
             * - Button ("이동") 생성
             * - Background = LightBlue 적용
             * 
             * 탐색 흐름:
             * Page1 → [Page2] → Page3
             *          ↑ 현재
             */
        }

        /// <summary>
        /// "이동" 버튼 클릭 이벤트 핸들러
        /// Page3를 생성하고 NavigationService를 통해 이동합니다.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 객체 (Button)</param>
        /// <param name="e">라우트 이벤트 인자</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Page3 인스턴스 생성
            Page3 page3 = new Page3();
            
            // NavigationService를 사용하여 Page3로 이동
            NavigationService.Navigate(page3);
            /*
             * Page3로 이동:
             * 
             * 탐색 기록:
             * 이동 전: [Page1] → [Page2] ← 현재
             * 이동 후: [Page1] → [Page2] → [Page3] ← 현재
             * 
             * Back 스택:
             * [Page1] → [Page2]
             * 
             * "뒤로" 버튼:
             * - Page3 → Page2 → Page1 순서로 이동 가능
             * 
             * Forward 스택:
             * - 비어 있음 (새로운 페이지로 이동)
             */
        }
    }
}
