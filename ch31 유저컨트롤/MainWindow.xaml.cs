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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace ch31_유저컨트롤
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 탭 4: 외부 접근 - uc1 텍스트 가져오기
        private void Button_GetText_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"입력된 텍스트: {uc1.txt.Text}", "텍스트 확인");
        }

        // 탭 4: 외부 접근 - uc1 진행률 가져오기
        private void Button_GetProgress_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"진행률: {uc1.Pb.Value}/{uc1.Pb.Maximum}", "진행률 확인");
        }

        // 탭 4: 외부 접근 - uc2 텍스트 가져오기
        private void Button_GetUc2Text_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"uc2 텍스트: {uc2.txt.Text}", "텍스트 확인");
        }

        // 탭 4: 외부 접근 - uc2 텍스트 설정하기
        private void Button_SetUc2Text_Click(object sender, RoutedEventArgs e)
        {
            uc2.txt.Text = "코드에서 설정한 텍스트입니다.";
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_1", "<Border BorderBrush=\"#999999\" BorderThickness=\"1\" Padding=\"10\">\n    <TextBlock Text=\"내 사용자 컨트롤\" FontWeight=\"Bold\"/>\n</Border>" },
            { "1_2", "private void Add_Click(object sender, RoutedEventArgs e)\n{\n    MyControl control = new MyControl();\n    host.Children.Add(control);\n}" },
            { "2_1", "public static readonly DependencyProperty TitleProperty =\n    DependencyProperty.Register(\"Title\", typeof(string), typeof(MyControl));\n\npublic string Title\n{\n    get { return (string)GetValue(TitleProperty); }\n    set { SetValue(TitleProperty, value); }\n}" },
            { "2_2", "<TextBlock Text=\"{Binding Title, RelativeSource={RelativeSource AncestorType=UserControl}}\" FontWeight=\"Bold\"/>" },
            { "3_1", "<Border Background=\"White\" BorderBrush=\"#DDDDDD\" BorderThickness=\"1\" Padding=\"12\" Width=\"220\">\n    <StackPanel>\n        <TextBlock Text=\"제목\" FontSize=\"15\" FontWeight=\"Bold\"/>\n        <TextBlock Text=\"내용이 들어갑니다.\" Margin=\"0,6,0,0\" TextWrapping=\"Wrap\"/>\n    </StackPanel>\n</Border>" },
            { "3_2", "<StackPanel Orientation=\"Horizontal\">\n    <Border Background=\"#E3F2FD\" Padding=\"14\" Margin=\"4\"><TextBlock Text=\"첫째\"/></Border>\n    <Border Background=\"#E8F5E9\" Padding=\"14\" Margin=\"4\"><TextBlock Text=\"둘째\"/></Border>\n    <Border Background=\"#FFF3E0\" Padding=\"14\" Margin=\"4\"><TextBlock Text=\"셋째\"/></Border>\n</StackPanel>" },
            { "4_1", "private void Change_Click(object sender, RoutedEventArgs e)\n{\n    myControl.Title = \"바뀐 제목\";\n}" },
            { "4_2", "public event EventHandler? Clicked;\n\nprivate void Inner_Click(object sender, RoutedEventArgs e)\n{\n    Clicked?.Invoke(this, EventArgs.Empty);\n}" },
            { "5_1", "public MainWindow()\n{\n    InitializeComponent();\n    myControl.Clicked += MyControl_Clicked;\n}" },
            { "5_2", "<StackPanel>\n    <Border Background=\"#FFEBEE\" Padding=\"10\" Margin=\"0,0,0,6\"><TextBlock Text=\"오류: 저장하지 못했습니다.\"/></Border>\n    <Border Background=\"#E8F5E9\" Padding=\"10\"><TextBlock Text=\"성공: 저장되었습니다.\"/></Border>\n</StackPanel>" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "1_2", new[] { "new MyControl", "Children.Add" } },
            { "2_1", new[] { "DependencyProperty.Register", "typeof(string)", "GetValue", "SetValue" } },
            { "4_1", new[] { "myControl.Title" } },
            { "4_2", new[] { "Clicked?.Invoke", "EventArgs.Empty" } },
            { "5_1", new[] { "myControl.Clicked", "+=" } },
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
