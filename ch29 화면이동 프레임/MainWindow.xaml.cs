using System.Windows;
using System.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch29_화면이동_프레임
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 탐색 완료 이벤트 구독
            mainFrame.Navigated += MainFrame_Navigated;
        }

        // 뒤로 가기
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.CanGoBack)
            {
                mainFrame.GoBack();
            }
        }

        // 앞으로 가기
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (mainFrame.CanGoForward)
            {
                mainFrame.GoForward();
            }
        }

        // 홈으로 이동
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Views.Page1());
        }

        // 탐색 상태 업데이트
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateNavigationStatus();
        }

        private void UpdateNavigationStatus()
        {
            string backStatus = mainFrame.CanGoBack ? "가능" : "불가";
            string forwardStatus = mainFrame.CanGoForward ? "가능" : "불가";
            navStatus.Text = $"뒤로: {backStatus} | 앞으로: {forwardStatus}";
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "2_3", "<DockPanel Height=\"160\">\n    <Border DockPanel.Dock=\"Top\" Background=\"#3F51B5\" Height=\"30\">\n        <TextBlock Text=\"내비게이션\" Foreground=\"White\" Margin=\"8,6\"/>\n    </Border>\n    <Frame BorderBrush=\"Gray\" BorderThickness=\"1\" NavigationUIVisibility=\"Hidden\"/>\n</DockPanel>" },
            { "3_3", "<DockPanel Height=\"140\">\n    <StackPanel DockPanel.Dock=\"Top\" Orientation=\"Horizontal\" Margin=\"6\">\n        <Button Content=\"뒤로\" Width=\"70\" Margin=\"0,0,6,0\"/>\n        <Button Content=\"앞으로\" Width=\"70\"/>\n    </StackPanel>\n    <TextBlock Text=\"페이지 본문\" Margin=\"8\"/>\n</DockPanel>" },
            { "4_3", "private void Forward_Click(object sender, RoutedEventArgs e)\n{\n    if (NavigationService.CanGoForward)\n    {\n        NavigationService.GoForward();\n    }\n}" },
            { "5_3", "private void ClearHistory_Click(object sender, RoutedEventArgs e)\n{\n    while (NavigationService.CanGoBack)\n    {\n        NavigationService.RemoveBackEntry();\n    }\n}" },
            { "2_1", "<Frame Height=\"120\" BorderBrush=\"Gray\" BorderThickness=\"1\"/>" },
            { "2_2", "<Frame Height=\"120\" BorderBrush=\"Gray\" BorderThickness=\"1\" NavigationUIVisibility=\"Hidden\"/>" },
            { "3_1", "<StackPanel Margin=\"10\">\n    <TextBlock Text=\"첫 번째 페이지\" FontSize=\"18\" FontWeight=\"Bold\"/>\n    <TextBlock Text=\"Page 는 Frame 안에 표시되는 화면 단위입니다.\" Margin=\"0,6,0,0\"/>\n</StackPanel>" },
            { "3_2", "<StackPanel Margin=\"10\">\n    <TextBlock Text=\"1 단계\" FontSize=\"16\"/>\n    <Button Content=\"다음 단계로\" Width=\"120\" HorizontalAlignment=\"Left\" Margin=\"0,8,0,0\"/>\n</StackPanel>" },
            { "4_1", "private void Next_Click(object sender, RoutedEventArgs e)\n{\n    NavigationService.Navigate(new Page2());\n}" },
            { "4_2", "private void Back_Click(object sender, RoutedEventArgs e)\n{\n    if (NavigationService.CanGoBack)\n    {\n        NavigationService.GoBack();\n    }\n}" },
            { "5_1", "private void Go_Click(object sender, RoutedEventArgs e)\n{\n    Page2 page = new Page2(nameBox.Text);\n    NavigationService.Navigate(page);\n}" },
            { "5_2", "private void Go2_Click(object sender, RoutedEventArgs e)\n{\n    Page2 page = new Page2();\n    page.UserName = nameBox.Text;\n    NavigationService.Navigate(page);\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "4_3", new[] { "CanGoForward", "GoForward" } },
            { "5_3", new[] { "while", "CanGoBack", "RemoveBackEntry" } },
            { "4_1", new[] { "NavigationService", "Navigate", "new Page2" } },
            { "4_2", new[] { "CanGoBack", "GoBack" } },
            { "5_1", new[] { "new Page2(", "Navigate" } },
            { "5_2", new[] { "new Page2", "UserName", "Navigate" } },
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
