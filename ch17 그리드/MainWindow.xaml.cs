using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ch17_그리드
{
    public partial class MainWindow : Window
    {
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
    }
}
