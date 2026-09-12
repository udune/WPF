using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch30_탭컨트롤_모달_모달리스
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 모달리스 창 열기
        private void Modalless_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Owner = this;
            window1.Title = "모달리스 창 (Show)";
            window1.Show();

            resultText.Text = "모달리스 창이 열렸습니다. 이 창도 계속 사용할 수 있습니다.";
        }

        // 모달 창 열기
        private void Modal_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Owner = this;
            window1.Title = "모달 창 (ShowDialog)";

            bool? result = window1.ShowDialog();

            if (result == true)
            {
                resultText.Text = "모달 창 결과: 확인 (DialogResult = true)";
            }
            else if (result == false)
            {
                resultText.Text = "모달 창 결과: 취소 (DialogResult = false)";
            }
            else
            {
                resultText.Text = "모달 창 결과: X 버튼으로 닫힘 (DialogResult = null)";
            }
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "2_3", "<TabControl Height=\"120\" SelectedIndex=\"1\">\n    <TabItem Header=\"첫째\"/>\n    <TabItem Header=\"둘째\"/>\n    <TabItem Header=\"셋째\"/>\n</TabControl>" },
            { "3_3", "<TabControl Height=\"120\" TabStripPlacement=\"Bottom\">\n    <TabItem Header=\"가\" FontWeight=\"Bold\"/>\n    <TabItem Header=\"나\"/>\n</TabControl>" },
            { "4_3", "private void Center_Click(object sender, RoutedEventArgs e)\n{\n    Window1 window = new Window1();\n    window.Owner = this;\n    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;\n    window.ShowDialog();\n}" },
            { "5_3", "private void Cancel_Click(object sender, RoutedEventArgs e)\n{\n    this.DialogResult = false;\n}" },
            { "2_1", "<TabControl Height=\"120\">\n    <TabItem Header=\"홈\"/>\n    <TabItem Header=\"설정\"/>\n    <TabItem Header=\"정보\"/>\n</TabControl>" },
            { "2_2", "<TabControl Height=\"120\">\n    <TabItem Header=\"홈\">\n        <TextBlock Text=\"홈 화면입니다.\" Margin=\"10\"/>\n    </TabItem>\n    <TabItem Header=\"설정\">\n        <TextBlock Text=\"설정 화면입니다.\" Margin=\"10\"/>\n    </TabItem>\n</TabControl>" },
            { "3_1", "<TabControl Height=\"120\" TabStripPlacement=\"Left\">\n    <TabItem Header=\"첫째\"/>\n    <TabItem Header=\"둘째\"/>\n</TabControl>" },
            { "3_2", "<TabControl Height=\"120\">\n    <TabItem>\n        <TabItem.Header>\n            <StackPanel Orientation=\"Horizontal\">\n                <Ellipse Width=\"10\" Height=\"10\" Fill=\"Green\" Margin=\"0,0,5,0\"/>\n                <TextBlock Text=\"상태\"/>\n            </StackPanel>\n        </TabItem.Header>\n        <TextBlock Text=\"내용\" Margin=\"10\"/>\n    </TabItem>\n</TabControl>" },
            { "4_1", "private void OpenModal_Click(object sender, RoutedEventArgs e)\n{\n    Window1 window = new Window1();\n    window.Owner = this;\n    window.ShowDialog();\n}" },
            { "4_2", "private void OpenModeless_Click(object sender, RoutedEventArgs e)\n{\n    Window1 window = new Window1();\n    window.Owner = this;\n    window.Show();\n}" },
            { "5_1", "private void Ok_Click(object sender, RoutedEventArgs e)\n{\n    this.DialogResult = true;\n}" },
            { "5_2", "private void Ask_Click(object sender, RoutedEventArgs e)\n{\n    Window1 window = new Window1();\n    bool? result = window.ShowDialog();\n    if (result == true)\n    {\n        resultText.Text = \"확인을 눌렀습니다.\";\n    }\n    else\n    {\n        resultText.Text = \"취소했습니다.\";\n    }\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "4_3", new[] { "Owner = this", "WindowStartupLocation", "ShowDialog" } },
            { "5_3", new[] { "DialogResult", "false" } },
            { "4_1", new[] { "new Window1", "ShowDialog" } },
            { "4_2", new[] { "new Window1", "Show()" } },
            { "5_1", new[] { "DialogResult", "true" } },
            { "5_2", new[] { "bool?", "ShowDialog", "result == true" } },
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
