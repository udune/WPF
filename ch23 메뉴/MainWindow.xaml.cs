using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Markup;

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

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_3", "<Menu>\n    <MenuItem Header=\"보기\">\n        <MenuItem Header=\"확대\"/>\n        <MenuItem Header=\"축소\"/>\n    </MenuItem>\n</Menu>" },
            { "2_3", "<Menu>\n    <MenuItem Header=\"삽입\">\n        <MenuItem Header=\"표\">\n            <MenuItem Header=\"행 추가\"/>\n            <MenuItem Header=\"열 추가\"/>\n        </MenuItem>\n    </MenuItem>\n</Menu>" },
            { "3_3", "<Menu>\n    <MenuItem Header=\"편집\">\n        <MenuItem Header=\"붙여넣기\" InputGestureText=\"Ctrl+V\">\n            <MenuItem.Icon>\n                <Ellipse Width=\"12\" Height=\"12\" Fill=\"Green\"/>\n            </MenuItem.Icon>\n        </MenuItem>\n    </MenuItem>\n</Menu>" },
            { "4_3", "<Menu>\n    <MenuItem Header=\"편집\">\n        <MenuItem Header=\"실행 취소\" IsEnabled=\"False\"/>\n        <MenuItem Header=\"다시 실행\"/>\n    </MenuItem>\n</Menu>" },
            { "5_3", "<Menu>\n    <MenuItem Header=\"보기\">\n        <MenuItem Header=\"큰 아이콘\" IsCheckable=\"True\" IsChecked=\"True\"/>\n        <MenuItem Header=\"자세히\" IsCheckable=\"True\"/>\n        <MenuItem Header=\"목록\" IsCheckable=\"True\"/>\n    </MenuItem>\n</Menu>" },
            { "1_1", "<Menu>\n    <MenuItem Header=\"파일\"/>\n    <MenuItem Header=\"편집\"/>\n</Menu>" },
            { "1_2", "<Menu>\n    <MenuItem Header=\"파일\">\n        <MenuItem Header=\"새로 만들기\"/>\n        <MenuItem Header=\"열기\"/>\n        <MenuItem Header=\"종료\"/>\n    </MenuItem>\n</Menu>" },
            { "2_1", "<Menu>\n    <MenuItem Header=\"편집\">\n        <MenuItem Header=\"찾기\">\n            <MenuItem Header=\"현재 문서\"/>\n            <MenuItem Header=\"전체 문서\"/>\n        </MenuItem>\n    </MenuItem>\n</Menu>" },
            { "2_2", "<Menu>\n    <MenuItem Header=\"파일\">\n        <MenuItem Header=\"저장\"/>\n        <Separator/>\n        <MenuItem Header=\"종료\"/>\n    </MenuItem>\n</Menu>" },
            { "3_1", "<Menu>\n    <MenuItem Header=\"파일\">\n        <MenuItem Header=\"저장\">\n            <MenuItem.Icon>\n                <Rectangle Width=\"12\" Height=\"12\" Fill=\"Blue\"/>\n            </MenuItem.Icon>\n        </MenuItem>\n    </MenuItem>\n</Menu>" },
            { "3_2", "<Menu>\n    <MenuItem Header=\"편집\">\n        <MenuItem Header=\"복사\" InputGestureText=\"Ctrl+C\"/>\n    </MenuItem>\n</Menu>" },
            { "4_1", "<Menu Background=\"#333333\">\n    <MenuItem Header=\"파일\" Foreground=\"White\"/>\n    <MenuItem Header=\"편집\" Foreground=\"White\"/>\n</Menu>" },
            { "4_2", "<Menu>\n    <MenuItem Header=\"보기\">\n        <MenuItem Header=\"자동 줄바꿈\" IsCheckable=\"True\" IsChecked=\"True\"/>\n    </MenuItem>\n</Menu>" },
            { "5_1", "<Menu>\n    <MenuItem Header=\"파일\">\n        <MenuItem Header=\"새로 만들기\"/>\n        <MenuItem Header=\"열기\"/>\n        <Separator/>\n        <MenuItem Header=\"종료\"/>\n    </MenuItem>\n    <MenuItem Header=\"도움말\">\n        <MenuItem Header=\"정보\"/>\n    </MenuItem>\n</Menu>" },
            { "5_2", "private void Save_Click(object sender, RoutedEventArgs e)\n{\n    MessageBox.Show(\"저장되었습니다\");\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_2", new[] { "MessageBox.Show", "저장되었습니다" } },
        };

        // XAML 실행 메서드
        private void ExecuteXaml(string xamlCode, StackPanel resultPanel, Border resultBorder)
        {
            resultPanel.Children.Clear();
            resultBorder.Visibility = Visibility.Visible;

            try
            {
                string fullXaml = xamlCode.Trim();

                if (!fullXaml.Contains("xmlns="))
                {
                    // 루트 태그가 무엇이든 프레젠테이션 네임스페이스를 붙여 줍니다.
                    Match root = Regex.Match(fullXaml, @"^<([A-Za-z_][\w.]*)");
                    if (root.Success)
                    {
                        string name = root.Groups[1].Value;
                        fullXaml = "<" + name
                            + " xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'"
                            + " xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'"
                            + fullXaml.Substring(name.Length + 1);
                    }
                }

                if (XamlReader.Parse(fullXaml) is UIElement element)
                {
                    resultPanel.Children.Add(element);
                    resultPanel.Children.Add(new TextBlock
                    {
                        Text = "성공적으로 실행되었습니다!",
                        Foreground = Brushes.Green,
                        Margin = new Thickness(0, 10, 0, 0)
                    });
                }
            }
            catch (Exception ex)
            {
                resultPanel.Children.Add(new TextBlock
                {
                    Text = $"오류: {ex.Message}",
                    Foreground = Brushes.Red,
                    TextWrapping = TextWrapping.Wrap
                });
            }
        }

        // XAML 실행 버튼
        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var txtPractice = FindName($"txtPractice{tag}") as TextBox;
                var resultPanel = FindName($"resultPanel{tag}") as StackPanel;
                var resultBorder = FindName($"resultBorder{tag}") as Border;

                if (txtPractice != null && resultPanel != null && resultBorder != null)
                {
                    ExecuteXaml(txtPractice.Text, resultPanel, resultBorder);
                }
            }
        }

        // 힌트 토글 버튼
        private void BtnHint_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var txtHint = FindName($"txtHint{tag}") as TextBlock;
                if (txtHint != null)
                {
                    txtHint.Visibility = txtHint.Visibility == Visibility.Visible
                        ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        // 정답 보기 버튼
        private void BtnAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var txtPractice = FindName($"txtPractice{tag}") as TextBox;
                if (txtPractice != null && _answers.TryGetValue(tag, out var answer))
                {
                    txtPractice.Text = answer;
                }
            }
        }

        // 코드 비교 확인 버튼
        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                var txtPractice = FindName($"txtPractice{tag}") as TextBox;
                var txtResult = FindName($"txtResult{tag}") as TextBlock;

                if (txtPractice != null && txtResult != null && _requiredKeywords.TryGetValue(tag, out var keywords))
                {
                    txtResult.Visibility = Visibility.Visible;
                    string userCode = txtPractice.Text;

                    var missing = keywords.Where(k => !userCode.Contains(k)).ToList();

                    if (missing.Count == 0)
                    {
                        txtResult.Text = "정답입니다! 모든 필수 요소가 포함되어 있습니다.";
                        txtResult.Foreground = Brushes.Green;
                    }
                    else
                    {
                        txtResult.Text = $"다시 확인해보세요. 누락된 요소: {string.Join(", ", missing)}";
                        txtResult.Foreground = Brushes.Red;
                    }
                }
            }
        }

    }
}
