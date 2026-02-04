using System.Windows;
using System.Windows.Controls;
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

        public MainWindow()
        {
            InitializeComponent();
        }

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
    }
}
