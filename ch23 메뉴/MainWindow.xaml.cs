using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ch23_메뉴
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 기본 사용법 - 메시지 표시
        private void ShowMessage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("메뉴가 클릭되었습니다!", "알림");
        }

        // 기본 사용법 - 시간 표시
        private void ShowTime_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"현재 시간: {DateTime.Now:HH:mm:ss}", "시간");
        }

        // 아이콘과 단축키 - 정렬 옵션 (라디오 버튼 스타일)
        private void SortOption_Checked(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (sortByName == null || sortByDate == null || sortBySize == null || sortStatusText == null) return;

            sortByName.IsChecked = (item == sortByName);
            sortByDate.IsChecked = (item == sortByDate);
            sortBySize.IsChecked = (item == sortBySize);
            sortStatusText.Text = $"현재 정렬: {item?.Header}";
        }

        // 스타일과 외관 - 기본 테마
        private void DefaultTheme_Click(object sender, RoutedEventArgs e)
        {
            themedMenu.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            themedMenu.Foreground = Brushes.Black;
        }

        // 스타일과 외관 - 다크 테마
        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            themedMenu.Background = new SolidColorBrush(Color.FromRgb(45, 45, 48));
            themedMenu.Foreground = Brushes.White;
        }

        // 스타일과 외관 - 블루 테마
        private void BlueTheme_Click(object sender, RoutedEventArgs e)
        {
            themedMenu.Background = new SolidColorBrush(Color.FromRgb(0, 122, 204));
            themedMenu.Foreground = Brushes.White;
        }

        // 실용 예제 - 새 파일
        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            editorTextBox.Text = "";
            MessageBox.Show("새 문서가 생성되었습니다.", "알림");
        }

        // 실용 예제 - 보기 옵션 (라디오 버튼 스타일)
        private void ViewOption_Checked(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (viewFit == null || viewActual == null) return;

            viewFit.IsChecked = (item == viewFit);
            viewActual.IsChecked = (item == viewActual);
        }

        // 실용 예제 - 테마 옵션 (라디오 버튼 스타일)
        private void ThemeOption_Checked(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (themeLight == null || themeDark == null || themeSystem == null) return;

            themeLight.IsChecked = (item == themeLight);
            themeDark.IsChecked = (item == themeDark);
            themeSystem.IsChecked = (item == themeSystem);

            UpdateSettingsStatus();
        }

        // 실용 예제 - 언어 옵션 (라디오 버튼 스타일)
        private void LanguageOption_Checked(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (langKorean == null || langEnglish == null || langJapanese == null) return;

            langKorean.IsChecked = (item == langKorean);
            langEnglish.IsChecked = (item == langEnglish);
            langJapanese.IsChecked = (item == langJapanese);

            UpdateSettingsStatus();
        }

        // 설정 상태 업데이트
        private void UpdateSettingsStatus()
        {
            if (settingsStatus == null) return;

            string theme = themeLight?.IsChecked == true ? "라이트 모드" :
                          themeDark?.IsChecked == true ? "다크 모드" : "시스템 설정";
            string lang = langKorean?.IsChecked == true ? "한국어" :
                         langEnglish?.IsChecked == true ? "English" : "日本語";

            settingsStatus.Text = $"현재: {theme}, {lang}";
        }
    }
}
