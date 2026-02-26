using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch16_독패널
{
    public partial class MainWindow : Window
    {
        private int dynamicCount = 0;
        private readonly Color[] colors = {
            Color.FromRgb(0xBB, 0xDE, 0xFB),
            Color.FromRgb(0xFF, 0xE0, 0xB2),
            Color.FromRgb(0xC8, 0xE6, 0xC9),
            Color.FromRgb(0xE1, 0xBE, 0xE7)
        };

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<DockPanel Height=\"120\">\n    <Button DockPanel.Dock=\"Top\">Top</Button>\n    <Button DockPanel.Dock=\"Left\">Left</Button>\n    <Border Background=\"LightYellow\">\n        <TextBlock Text=\"Center\"/>\n    </Border>\n</DockPanel>" },
            { "1_2", "<DockPanel Height=\"120\">\n    <Button DockPanel.Dock=\"Top\">Top 1</Button>\n    <Button DockPanel.Dock=\"Top\">Top 2</Button>\n    <Border Background=\"LightYellow\">\n        <TextBlock Text=\"Center\"/>\n    </Border>\n</DockPanel>" },

            // 탭 2: 배치순서/LastChildFill
            { "2_1", "<DockPanel Height=\"120\">\n    <Button DockPanel.Dock=\"Left\">Left</Button>\n    <Button DockPanel.Dock=\"Top\">Top</Button>\n    <Border Background=\"LightYellow\">\n        <TextBlock Text=\"Center\"/>\n    </Border>\n</DockPanel>" },
            { "2_2", "<DockPanel Height=\"100\" LastChildFill=\"False\">\n    <Button DockPanel.Dock=\"Left\">Left</Button>\n    <Button DockPanel.Dock=\"Right\">Right</Button>\n</DockPanel>" },

            // 탭 3: 실용 레이아웃
            { "3_1", "<DockPanel Height=\"150\">\n    <Border DockPanel.Dock=\"Top\" Background=\"LightBlue\" Padding=\"8\">\n        <TextBlock Text=\"헤더\"/>\n    </Border>\n    <Border DockPanel.Dock=\"Bottom\" Background=\"LightGray\" Padding=\"4\">\n        <TextBlock Text=\"상태바\"/>\n    </Border>\n    <Border Background=\"White\" Padding=\"10\">\n        <TextBlock Text=\"콘텐츠\"/>\n    </Border>\n</DockPanel>" },
            { "3_2", "<DockPanel Height=\"120\">\n    <StackPanel DockPanel.Dock=\"Bottom\" Orientation=\"Horizontal\" HorizontalAlignment=\"Right\">\n        <Button Content=\"확인\" Margin=\"5\"/>\n        <Button Content=\"취소\" Margin=\"5\"/>\n    </StackPanel>\n    <Border Background=\"LightYellow\" Padding=\"10\">\n        <TextBlock Text=\"콘텐츠 영역\"/>\n    </Border>\n</DockPanel>" },

            // 탭 4: 스타일
            { "4_1", "<DockPanel Height=\"100\" Background=\"LightBlue\">\n    <Button DockPanel.Dock=\"Top\">상단</Button>\n    <TextBlock Text=\"콘텐츠\"/>\n</DockPanel>" },
            { "4_2", "<DockPanel Height=\"100\" Background=\"Gray\">\n    <Button DockPanel.Dock=\"Top\" Margin=\"5\">Top</Button>\n    <Button DockPanel.Dock=\"Left\" Margin=\"5\">Left</Button>\n    <Button Margin=\"5\">Center</Button>\n</DockPanel>" },

            // 탭 5: 코드비하인드 (코드 비교 방식)
            { "5_1", "private void MoveToBottom()\n{\n    // myElement를 Bottom으로 이동\n    DockPanel.SetDock(myElement, Dock.Bottom);\n}" },
            { "5_2", "private void AddLeftButton()\n{\n    var btn = new Button { Content = \"새 버튼\" };\n    // btn을 Left에 도킹하고 myDockPanel에 추가\n    DockPanel.SetDock(btn, Dock.Left);\n    myDockPanel.Children.Add(btn);\n}" },
            { "5_3", "private void DisableLastChildFill()\n{\n    // myDockPanel의 LastChildFill 변경\n    myDockPanel.LastChildFill = false;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { "DockPanel.SetDock", "Dock.Bottom" } },
            { "5_2", new[] { "DockPanel.SetDock", "Dock.Left", "Children" } },
            { "5_3", new[] { "LastChildFill", "false" } },
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        #region 직접 해보기 이벤트 핸들러

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

                    // 모든 필수 키워드가 포함되어 있는지 확인
                    var missingKeywords = keywords.Where(k => !userCode.Contains(k)).ToList();

                    if (missingKeywords.Count == 0)
                    {
                        txtResult.Text = "정답입니다! 모든 필수 요소가 포함되어 있습니다.";
                        txtResult.Foreground = Brushes.Green;
                    }
                    else
                    {
                        txtResult.Text = $"다시 확인해보세요. 누락된 요소: {string.Join(", ", missingKeywords)}";
                        txtResult.Foreground = Brushes.Red;
                    }
                }
            }
        }

        // XAML 실행 메서드
        private void ExecuteXaml(string xamlCode, StackPanel resultPanel, Border resultBorder)
        {
            resultPanel.Children.Clear();
            resultBorder.Visibility = Visibility.Visible;

            try
            {
                string fullXaml = xamlCode;

                // XAML 네임스페이스 추가
                if (!xamlCode.Contains("xmlns="))
                {
                    string trimmed = xamlCode.TrimStart();
                    if (trimmed.StartsWith("<DockPanel"))
                    {
                        fullXaml = xamlCode.Replace("<DockPanel",
                            "<DockPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<StackPanel"))
                    {
                        fullXaml = xamlCode.Replace("<StackPanel",
                            "<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<Grid"))
                    {
                        fullXaml = xamlCode.Replace("<Grid",
                            "<Grid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                }

                var element = XamlReader.Parse(fullXaml) as UIElement;
                if (element != null)
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

        #endregion

        #region 튜토리얼 이벤트 핸들러

        // 탭5: Dock 위치 변경
        private void DockTop_Click(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(bdMovable, Dock.Top);
            tbDockPos.Text = "현재: Top";
        }

        private void DockBottom_Click(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(bdMovable, Dock.Bottom);
            tbDockPos.Text = "현재: Bottom";
        }

        private void DockLeft_Click(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(bdMovable, Dock.Left);
            tbDockPos.Text = "현재: Left";
        }

        private void DockRight_Click(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(bdMovable, Dock.Right);
            tbDockPos.Text = "현재: Right";
        }

        // 탭5: 동적 자식 추가
        private void AddLeft_Click(object sender, RoutedEventArgs e)
        {
            AddDockChild(Dock.Left);
        }

        private void AddTop_Click(object sender, RoutedEventArgs e)
        {
            AddDockChild(Dock.Top);
        }

        private void AddRight_Click(object sender, RoutedEventArgs e)
        {
            AddDockChild(Dock.Right);
        }

        private void AddDockChild(Dock dock)
        {
            dynamicCount++;
            var btn = new Button
            {
                Content = dock + " " + dynamicCount,
                Padding = new Thickness(8, 4, 8, 4),
                Background = new SolidColorBrush(colors[dynamicCount % colors.Length])
            };
            DockPanel.SetDock(btn, dock);
            dpDynamic.Children.Insert(dpDynamic.Children.Count - 1, btn);
            tbDynamicInfo.Text = "자식 요소: " + (dpDynamic.Children.Count - 1) + "개 (Center 제외)";
        }

        private void ClearDock_Click(object sender, RoutedEventArgs e)
        {
            while (dpDynamic.Children.Count > 1)
            {
                dpDynamic.Children.RemoveAt(0);
            }
            dynamicCount = 0;
            tbDynamicInfo.Text = "자식 요소: 0개 (Center 제외)";
        }

        // 탭5: LastChildFill 토글
        private void SetLastChildTrue_Click(object sender, RoutedEventArgs e)
        {
            dpLastChild.LastChildFill = true;
            tbLastChild.Text = "LastChildFill = True";
        }

        private void SetLastChildFalse_Click(object sender, RoutedEventArgs e)
        {
            dpLastChild.LastChildFill = false;
            tbLastChild.Text = "LastChildFill = False";
        }

        #endregion
    }
}
