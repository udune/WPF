using System.Windows;
using System.Windows.Controls;

namespace ch15_랩패널
{
    public partial class MainWindow : Window
    {
        private int itemCount = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        // 탭5: 동적으로 버튼 추가
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            itemCount++;
            wpDynamic.Children.Add(new Button
            {
                Content = "항목 " + itemCount,
                Margin = new Thickness(4),
                Padding = new Thickness(12, 6, 12, 6)
            });
            tbDynamicCount.Text = "자식 요소: " + wpDynamic.Children.Count + "개";
        }

        // 탭5: 전체 삭제
        private void ClearItems_Click(object sender, RoutedEventArgs e)
        {
            wpDynamic.Children.Clear();
            itemCount = 0;
            tbDynamicCount.Text = "자식 요소: 0개";
        }

        // 탭5: Orientation을 Horizontal로 변경
        private void SetWrapHorizontal_Click(object sender, RoutedEventArgs e)
        {
            wpOrientation.Orientation = Orientation.Horizontal;
            tbWrapOrientation.Text = "현재: Horizontal";
        }

        // 탭5: Orientation을 Vertical로 변경
        private void SetWrapVertical_Click(object sender, RoutedEventArgs e)
        {
            wpOrientation.Orientation = Orientation.Vertical;
            tbWrapOrientation.Text = "현재: Vertical";
        }

        // 탭5: 슬라이더로 ItemWidth 변경
        private void sliderItemWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (wpItemWidth != null)
            {
                wpItemWidth.ItemWidth = sliderItemWidth.Value;
                tbItemWidth.Text = ((int)sliderItemWidth.Value).ToString();
            }
        }
    }
}
