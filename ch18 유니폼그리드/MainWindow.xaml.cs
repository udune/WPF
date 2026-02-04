using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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
    }
}
