using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace ch18_유니폼그리드
{
    public partial class MainWindow : Window
    {
        private int dynamicCount = 0;
        private readonly Color[] colors = {
            Color.FromRgb(0xBB, 0xDE, 0xFB),
            Color.FromRgb(0xC8, 0xE6, 0xC9),
            Color.FromRgb(0xFF, 0xE0, 0xB2),
            Color.FromRgb(0xE1, 0xBE, 0xE7),
            Color.FromRgb(0xFF, 0xCC, 0x80),
            Color.FromRgb(0x90, 0xCA, 0xF9)
        };

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "2_4", "<UniformGrid Columns=\"4\" FirstColumn=\"2\">\n    <Button Content=\"1\"/>\n    <Button Content=\"2\"/>\n    <Button Content=\"3\"/>\n</UniformGrid>" },
            { "3_4", "<UniformGrid Columns=\"7\">\n    <TextBlock Text=\"일\" Foreground=\"Red\" HorizontalAlignment=\"Center\"/>\n    <TextBlock Text=\"월\" HorizontalAlignment=\"Center\"/>\n    <TextBlock Text=\"화\" HorizontalAlignment=\"Center\"/>\n    <TextBlock Text=\"수\" HorizontalAlignment=\"Center\"/>\n    <TextBlock Text=\"목\" HorizontalAlignment=\"Center\"/>\n    <TextBlock Text=\"금\" HorizontalAlignment=\"Center\"/>\n    <TextBlock Text=\"토\" Foreground=\"Blue\" HorizontalAlignment=\"Center\"/>\n</UniformGrid>" },
            { "4_4", "<UniformGrid Columns=\"3\" Height=\"60\">\n    <Border Margin=\"8\" Background=\"#FFCDD2\"/>\n    <Border Margin=\"8\" Background=\"#C8E6C9\"/>\n    <Border Margin=\"8\" Background=\"#BBDEFB\"/>\n</UniformGrid>" },
            { "1_3", "<UniformGrid Rows=\"2\" Columns=\"2\" Height=\"120\">\n    <Button Content=\"1\" Margin=\"3\"/>\n    <Button Content=\"2\" Margin=\"3\"/>\n    <Button Content=\"3\" Margin=\"3\"/>\n    <Button Content=\"4\" Margin=\"3\"/>\n</UniformGrid>" },
            { "2_3", "<UniformGrid Columns=\"3\" Height=\"120\">\n    <Button Content=\"1\" Margin=\"3\"/>\n    <Button Content=\"2\" Margin=\"3\"/>\n    <Button Content=\"3\" Margin=\"3\"/>\n    <Button Content=\"4\" Margin=\"3\"/>\n    <Button Content=\"5\" Margin=\"3\"/>\n</UniformGrid>" },
            { "3_3", "<UniformGrid Rows=\"3\" Columns=\"3\" Width=\"180\" Height=\"180\">\n    <Button Content=\"7\" Margin=\"2\"/>\n    <Button Content=\"8\" Margin=\"2\"/>\n    <Button Content=\"9\" Margin=\"2\"/>\n    <Button Content=\"4\" Margin=\"2\"/>\n    <Button Content=\"5\" Margin=\"2\"/>\n    <Button Content=\"6\" Margin=\"2\"/>\n    <Button Content=\"1\" Margin=\"2\"/>\n    <Button Content=\"2\" Margin=\"2\"/>\n    <Button Content=\"3\" Margin=\"2\"/>\n</UniformGrid>" },
            { "4_3", "<UniformGrid Rows=\"2\" Columns=\"3\" FirstColumn=\"1\" Height=\"100\">\n    <Button Content=\"A\" Margin=\"3\"/>\n    <Button Content=\"B\" Margin=\"3\"/>\n</UniformGrid>" },
            // 탭1: 기본 사용법
            { "1_1", "<UniformGrid Rows=\"3\" Columns=\"2\">\n    <Button Content=\"A\"/>\n    <Button Content=\"B\"/>\n    <Button Content=\"C\"/>\n    <Button Content=\"D\"/>\n    <Button Content=\"E\"/>\n    <Button Content=\"F\"/>\n</UniformGrid>" },
            { "1_2", "<UniformGrid Rows=\"2\" Columns=\"4\">\n    <Border Background=\"Red\" Margin=\"2\"/>\n    <Border Background=\"Orange\" Margin=\"2\"/>\n    <Border Background=\"Yellow\" Margin=\"2\"/>\n    <Border Background=\"Green\" Margin=\"2\"/>\n    <Border Background=\"Blue\" Margin=\"2\"/>\n    <Border Background=\"Navy\" Margin=\"2\"/>\n    <Border Background=\"Purple\" Margin=\"2\"/>\n    <Border Background=\"Pink\" Margin=\"2\"/>\n</UniformGrid>" },

            // 탭2: 행/열 자동 계산
            { "2_1", "<UniformGrid Columns=\"3\">\n    <Button Content=\"1\"/>\n    <Button Content=\"2\"/>\n    <Button Content=\"3\"/>\n    <Button Content=\"4\"/>\n    <Button Content=\"5\"/>\n    <Button Content=\"6\"/>\n    <Button Content=\"7\"/>\n</UniformGrid>" },
            { "2_2", "<UniformGrid Columns=\"5\" FirstColumn=\"2\">\n    <Button Content=\"1\"/>\n    <Button Content=\"2\"/>\n    <Button Content=\"3\"/>\n    <Button Content=\"4\"/>\n    <Button Content=\"5\"/>\n    <Button Content=\"6\"/>\n</UniformGrid>" },

            // 탭3: 실용 예제
            { "3_1", "<UniformGrid Columns=\"3\" Rows=\"4\">\n    <Button Content=\"7\"/>\n    <Button Content=\"8\"/>\n    <Button Content=\"9\"/>\n    <Button Content=\"4\"/>\n    <Button Content=\"5\"/>\n    <Button Content=\"6\"/>\n    <Button Content=\"1\"/>\n    <Button Content=\"2\"/>\n    <Button Content=\"3\"/>\n    <Button Content=\"0\"/>\n    <Button Content=\"+\"/>\n    <Button Content=\"-\"/>\n</UniformGrid>" },
            { "3_2", "<UniformGrid Columns=\"4\" Rows=\"2\">\n    <Border Background=\"Red\" Margin=\"2\"/>\n    <Border Background=\"Orange\" Margin=\"2\"/>\n    <Border Background=\"Yellow\" Margin=\"2\"/>\n    <Border Background=\"Green\" Margin=\"2\"/>\n    <Border Background=\"Blue\" Margin=\"2\"/>\n    <Border Background=\"Navy\" Margin=\"2\"/>\n    <Border Background=\"Purple\" Margin=\"2\"/>\n    <Border Background=\"Pink\" Margin=\"2\"/>\n</UniformGrid>" },

            // 탭4: 스타일과 외관
            { "4_1", "<UniformGrid Rows=\"2\" Columns=\"3\" Background=\"#E3F2FD\">\n    <Button Content=\"1\" Margin=\"2\"/>\n    <Button Content=\"2\" Margin=\"2\"/>\n    <Button Content=\"3\" Margin=\"2\"/>\n    <Button Content=\"4\" Margin=\"2\"/>\n    <Button Content=\"5\" Margin=\"2\"/>\n    <Button Content=\"6\" Margin=\"2\"/>\n</UniformGrid>" },
            { "4_2", "<UniformGrid Columns=\"3\">\n    <Border Background=\"LightBlue\" Margin=\"10\"/>\n    <Border Background=\"LightGreen\" Margin=\"5\"/>\n    <Border Background=\"LightCoral\" Margin=\"2\"/>\n</UniformGrid>" },

            // 탭5: 코드비하인드
            { "5_1", "private void AddItem_Click(object sender, RoutedEventArgs e)\n{\n    _count++;\n    var btn = new Button { Content = \"Item\" + _count };\n    ugGrid.Children.Add(btn);\n}" },
            { "5_2", "private void SetLayout_Click(object sender, RoutedEventArgs e)\n{\n    ugGrid.Rows = 4;\n    ugGrid.Columns = 2;\n}" },
            { "5_3", "private void SetOffset_Click(object sender, RoutedEventArgs e)\n{\n    ugGrid.FirstColumn = 3;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { "new Button", "Content", "Item", "Children.Add" } },
            { "5_2", new[] { "Rows", "Columns", "4", "2" } },
            { "5_3", new[] { "FirstColumn", "3" } },
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializeChessBoard();
        }

        // 체스판 초기화
        private void InitializeChessBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var border = new Border
                    {
                        Background = (r + c) % 2 == 0
                            ? Brushes.White
                            : new SolidColorBrush(Color.FromRgb(0x76, 0x9a, 0x56))
                    };
                    ugChessBoard.Children.Add(border);
                }
            }
        }

        // 탭5: 동적 자식 추가/삭제
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            dynamicCount++;
            var btn = new Button
            {
                Content = "버튼" + dynamicCount,
                Margin = new Thickness(2),
                Background = new SolidColorBrush(colors[dynamicCount % colors.Length])
            };
            ugDynamic.Children.Add(btn);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ugDynamic.Children.Count > 0)
            {
                ugDynamic.Children.RemoveAt(ugDynamic.Children.Count - 1);
            }
        }

        private void ClearButtons_Click(object sender, RoutedEventArgs e)
        {
            ugDynamic.Children.Clear();
            dynamicCount = 0;
        }

        // 탭5: Rows/Columns 동적 변경
        private void SetCol2_Click(object sender, RoutedEventArgs e)
        {
            ugResize.Rows = 0;
            ugResize.Columns = 2;
            tbGridInfo.Text = "현재: 2열 (행 자동)";
        }

        private void SetCol3_Click(object sender, RoutedEventArgs e)
        {
            ugResize.Rows = 0;
            ugResize.Columns = 3;
            tbGridInfo.Text = "현재: 3열 (행 자동)";
        }

        private void SetCol6_Click(object sender, RoutedEventArgs e)
        {
            ugResize.Rows = 0;
            ugResize.Columns = 6;
            tbGridInfo.Text = "현재: 6열 (행 자동)";
        }

        private void Set2x3_Click(object sender, RoutedEventArgs e)
        {
            ugResize.Rows = 2;
            ugResize.Columns = 3;
            tbGridInfo.Text = "현재: 2행 × 3열";
        }

        private void SetAuto_Click(object sender, RoutedEventArgs e)
        {
            ugResize.Rows = 0;
            ugResize.Columns = 0;
            tbGridInfo.Text = "현재: 자동";
        }

        // 탭5: FirstColumn 동적 변경
        private void SetFirstCol0_Click(object sender, RoutedEventArgs e)
        {
            ugFirstCol.FirstColumn = 0;
            tbFirstColInfo.Text = "현재 FirstColumn: 0";
        }

        private void SetFirstCol1_Click(object sender, RoutedEventArgs e)
        {
            ugFirstCol.FirstColumn = 1;
            tbFirstColInfo.Text = "현재 FirstColumn: 1";
        }

        private void SetFirstCol2_Click(object sender, RoutedEventArgs e)
        {
            ugFirstCol.FirstColumn = 2;
            tbFirstColInfo.Text = "현재 FirstColumn: 2";
        }

        private void SetFirstCol3_Click(object sender, RoutedEventArgs e)
        {
            ugFirstCol.FirstColumn = 3;
            tbFirstColInfo.Text = "현재 FirstColumn: 3";
        }

        // ========== 직접 해보기 기능 ==========

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

                var element = XamlReader.Parse(fullXaml) as UIElement;
                if (element != null)
                {
                    // UniformGrid에 기본 높이 설정
                    if (element is UniformGrid ug && double.IsNaN(ug.Height))
                    {
                        ug.Height = 120;
                    }

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
    }
}
