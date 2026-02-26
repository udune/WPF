using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch17_그리드
{
    public partial class MainWindow : Window
    {
        // 직접 해보기 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭1: 기본 사용법
            { "1_1", "<Grid ShowGridLines=\"True\" Height=\"120\">\n    <Grid.RowDefinitions>\n        <RowDefinition/>\n        <RowDefinition/>\n    </Grid.RowDefinitions>\n    <Grid.ColumnDefinitions>\n        <ColumnDefinition/>\n        <ColumnDefinition/>\n    </Grid.ColumnDefinitions>\n    <TextBlock Text=\"(0,0)\" Grid.Row=\"0\" Grid.Column=\"0\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n    <TextBlock Text=\"(0,1)\" Grid.Row=\"0\" Grid.Column=\"1\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n    <TextBlock Text=\"(1,0)\" Grid.Row=\"1\" Grid.Column=\"0\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n    <TextBlock Text=\"(1,1)\" Grid.Row=\"1\" Grid.Column=\"1\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n</Grid>" },
            { "1_2", "<Grid Height=\"50\">\n    <Grid.ColumnDefinitions>\n        <ColumnDefinition/>\n        <ColumnDefinition/>\n        <ColumnDefinition/>\n    </Grid.ColumnDefinitions>\n    <Button Content=\"버튼1\" Grid.Column=\"0\" Margin=\"2\"/>\n    <Button Content=\"버튼2\" Grid.Column=\"1\" Margin=\"2\"/>\n    <Button Content=\"버튼3\" Grid.Column=\"2\" Margin=\"2\"/>\n</Grid>" },

            // 탭2: 크기 지정
            { "2_1", "<Grid Height=\"80\">\n    <Grid.ColumnDefinitions>\n        <ColumnDefinition Width=\"1*\"/>\n        <ColumnDefinition Width=\"2*\"/>\n        <ColumnDefinition Width=\"1*\"/>\n    </Grid.ColumnDefinitions>\n    <Border Grid.Column=\"0\" Background=\"#BBDEFB\" Margin=\"2\">\n        <TextBlock Text=\"1*\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n    </Border>\n    <Border Grid.Column=\"1\" Background=\"#90CAF9\" Margin=\"2\">\n        <TextBlock Text=\"2*\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n    </Border>\n    <Border Grid.Column=\"2\" Background=\"#64B5F6\" Margin=\"2\">\n        <TextBlock Text=\"1*\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n    </Border>\n</Grid>" },
            { "2_2", "<Grid Height=\"60\">\n    <Grid.ColumnDefinitions>\n        <ColumnDefinition Width=\"Auto\"/>\n        <ColumnDefinition Width=\"100\"/>\n        <ColumnDefinition Width=\"*\"/>\n    </Grid.ColumnDefinitions>\n    <Button Content=\"Auto\" Grid.Column=\"0\" Margin=\"2\"/>\n    <Button Content=\"100px\" Grid.Column=\"1\" Margin=\"2\"/>\n    <Button Content=\"나머지\" Grid.Column=\"2\" Margin=\"2\"/>\n</Grid>" },

            // 탭3: Span
            { "3_1", "<Grid Height=\"120\" ShowGridLines=\"True\">\n    <Grid.RowDefinitions>\n        <RowDefinition Height=\"Auto\"/>\n        <RowDefinition/>\n    </Grid.RowDefinitions>\n    <Grid.ColumnDefinitions>\n        <ColumnDefinition/>\n        <ColumnDefinition/>\n        <ColumnDefinition/>\n    </Grid.ColumnDefinitions>\n    <Border Grid.Row=\"0\" Grid.Column=\"0\" Grid.ColumnSpan=\"3\" Background=\"#1976D2\" Padding=\"8\">\n        <TextBlock Text=\"헤더\" Foreground=\"White\" HorizontalAlignment=\"Center\"/>\n    </Border>\n</Grid>" },
            { "3_2", "<Grid Height=\"150\" ShowGridLines=\"True\">\n    <Grid.RowDefinitions>\n        <RowDefinition/>\n        <RowDefinition/>\n        <RowDefinition/>\n    </Grid.RowDefinitions>\n    <Grid.ColumnDefinitions>\n        <ColumnDefinition Width=\"80\"/>\n        <ColumnDefinition/>\n    </Grid.ColumnDefinitions>\n    <Border Grid.Row=\"0\" Grid.Column=\"0\" Grid.RowSpan=\"3\" Background=\"#E3F2FD\" Padding=\"8\">\n        <TextBlock Text=\"사이드바\" VerticalAlignment=\"Center\"/>\n    </Border>\n</Grid>" },

            // 탭4: GridSplitter
            { "4_1", "<Grid Height=\"120\">\n    <Grid.ColumnDefinitions>\n        <ColumnDefinition Width=\"1*\"/>\n        <ColumnDefinition Width=\"Auto\"/>\n        <ColumnDefinition Width=\"1*\"/>\n    </Grid.ColumnDefinitions>\n    <Border Grid.Column=\"0\" Background=\"#BBDEFB\">\n        <TextBlock Text=\"왼쪽\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n    </Border>\n    <GridSplitter Grid.Column=\"1\" Width=\"5\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Stretch\" Background=\"#666666\"/>\n    <Border Grid.Column=\"2\" Background=\"#C8E6C9\">\n        <TextBlock Text=\"오른쪽\" HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n    </Border>\n</Grid>" },

            // 탭5: 코드비하인드
            { "5_1", "// myGrid에 새 행 추가\nmyGrid.RowDefinitions.Add(new RowDefinition());" },
            { "5_2", "// myElement를 2행 3열로 이동\nGrid.SetRow(myElement, 2);\nGrid.SetColumn(myElement, 3);" },
            { "5_3", "// col1을 2*, col2를 1*로 설정\ncol1.Width = new GridLength(2, GridUnitType.Star);\ncol2.Width = new GridLength(1, GridUnitType.Star);" }
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_1", new[] { "RowDefinitions.Add", "new RowDefinition()" } },
            { "5_2", new[] { "Grid.SetRow", "Grid.SetColumn", "2", "3" } },
            { "5_3", new[] { "GridLength", "GridUnitType.Star", "2", "1" } }
        };

        private readonly Color[] colors = {
            Color.FromRgb(0xBB, 0xDE, 0xFB),
            Color.FromRgb(0xC8, 0xE6, 0xC9),
            Color.FromRgb(0xFF, 0xE0, 0xB2),
            Color.FromRgb(0xE1, 0xBE, 0xE7),
            Color.FromRgb(0xFF, 0xCC, 0x80),
            Color.FromRgb(0x90, 0xCA, 0xF9)
        };
        private int cellCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        // 탭5: 동적 행/열 추가
        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            gdDynamic.RowDefinitions.Add(new RowDefinition());
            FillNewCells();
            UpdateDynamicInfo();
        }

        private void AddCol_Click(object sender, RoutedEventArgs e)
        {
            gdDynamic.ColumnDefinitions.Add(new ColumnDefinition());
            FillNewCells();
            UpdateDynamicInfo();
        }

        private void FillNewCells()
        {
            int rows = gdDynamic.RowDefinitions.Count;
            int cols = gdDynamic.ColumnDefinitions.Count;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    bool exists = false;
                    foreach (UIElement child in gdDynamic.Children)
                    {
                        if (Grid.GetRow(child) == r && Grid.GetColumn(child) == c)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        cellCount++;
                        var border = new Border
                        {
                            Background = new SolidColorBrush(colors[cellCount % colors.Length]),
                            Margin = new Thickness(2)
                        };
                        var tb = new TextBlock
                        {
                            Text = $"({r}, {c})",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 11
                        };
                        border.Child = tb;
                        Grid.SetRow(border, r);
                        Grid.SetColumn(border, c);
                        gdDynamic.Children.Add(border);
                    }
                }
            }
        }

        private void ResetGrid_Click(object sender, RoutedEventArgs e)
        {
            gdDynamic.Children.Clear();
            gdDynamic.RowDefinitions.Clear();
            gdDynamic.ColumnDefinitions.Clear();
            gdDynamic.RowDefinitions.Add(new RowDefinition());
            gdDynamic.ColumnDefinitions.Add(new ColumnDefinition());
            cellCount = 0;

            tbDynamicInfo = new TextBlock
            {
                Text = "1행 × 1열",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            gdDynamic.Children.Add(tbDynamicInfo);
        }

        private void UpdateDynamicInfo()
        {
            tbDynamicInfo.Text = $"{gdDynamic.RowDefinitions.Count}행 × {gdDynamic.ColumnDefinitions.Count}열";
        }

        // 탭5: 요소 위치 동적 변경
        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            int row = Grid.GetRow(bdMovable);
            if (row > 0)
            {
                Grid.SetRow(bdMovable, row - 1);
                UpdatePosition();
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            int row = Grid.GetRow(bdMovable);
            if (row < 2)
            {
                Grid.SetRow(bdMovable, row + 1);
                UpdatePosition();
            }
        }

        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            int col = Grid.GetColumn(bdMovable);
            if (col > 0)
            {
                Grid.SetColumn(bdMovable, col - 1);
                UpdatePosition();
            }
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            int col = Grid.GetColumn(bdMovable);
            if (col < 2)
            {
                Grid.SetColumn(bdMovable, col + 1);
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            tbPosition.Text = $"({Grid.GetRow(bdMovable)}, {Grid.GetColumn(bdMovable)})";
        }

        // 탭5: 행/열 크기 동적 변경
        private void ExpandLeft_Click(object sender, RoutedEventArgs e)
        {
            colLeft.Width = new GridLength(3, GridUnitType.Star);
            colRight.Width = new GridLength(1, GridUnitType.Star);
            tbLeftSize.Text = "3*";
            tbRightSize.Text = "1*";
        }

        private void ExpandRight_Click(object sender, RoutedEventArgs e)
        {
            colLeft.Width = new GridLength(1, GridUnitType.Star);
            colRight.Width = new GridLength(3, GridUnitType.Star);
            tbLeftSize.Text = "1*";
            tbRightSize.Text = "3*";
        }

        private void EqualSize_Click(object sender, RoutedEventArgs e)
        {
            colLeft.Width = new GridLength(1, GridUnitType.Star);
            colRight.Width = new GridLength(1, GridUnitType.Star);
            tbLeftSize.Text = "1*";
            tbRightSize.Text = "1*";
        }

        private void FixedLeft_Click(object sender, RoutedEventArgs e)
        {
            colLeft.Width = new GridLength(200);
            colRight.Width = new GridLength(1, GridUnitType.Star);
            tbLeftSize.Text = "200px";
            tbRightSize.Text = "1*";
        }

        // ===== 직접 해보기 이벤트 핸들러 =====

        // XAML 실행
        private void ExecuteXaml(string xamlCode, StackPanel resultPanel, Border resultBorder)
        {
            resultPanel.Children.Clear();
            resultBorder.Visibility = Visibility.Visible;

            try
            {
                string fullXaml = xamlCode;
                if (!xamlCode.Contains("xmlns="))
                {
                    // Grid 컨트롤에 네임스페이스 추가
                    var trimmed = xamlCode.TrimStart();
                    if (trimmed.StartsWith("<Grid"))
                    {
                        fullXaml = xamlCode.Replace("<Grid",
                            "<Grid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<StackPanel"))
                    {
                        fullXaml = xamlCode.Replace("<StackPanel",
                            "<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else
                    {
                        // 컨테이너로 감싸기
                        fullXaml = $"<Grid xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{xamlCode}</Grid>";
                    }
                }

                var element = XamlReader.Parse(fullXaml) as UIElement;
                if (element != null)
                {
                    resultPanel.Children.Add(element);
                    resultPanel.Children.Add(new TextBlock
                    {
                        Text = "✓ 성공적으로 실행되었습니다!",
                        Foreground = Brushes.Green,
                        Margin = new Thickness(0, 10, 0, 0)
                    });
                }
            }
            catch (Exception ex)
            {
                resultPanel.Children.Add(new TextBlock
                {
                    Text = $"✗ 오류: {ex.Message}",
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

                    var missingKeywords = keywords.Where(k => !userCode.Contains(k)).ToList();

                    if (missingKeywords.Count == 0)
                    {
                        txtResult.Text = "✓ 정답입니다! 모든 필수 요소가 포함되어 있습니다.";
                        txtResult.Foreground = Brushes.Green;
                    }
                    else
                    {
                        txtResult.Text = $"✗ 다시 확인해보세요. 누락된 요소: {string.Join(", ", missingKeywords)}";
                        txtResult.Foreground = Brushes.Red;
                    }
                }
            }
        }
    }
}
