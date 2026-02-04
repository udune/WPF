using System.Windows;
using System.Windows.Controls;

namespace ch14_스택패널
{
    public partial class MainWindow : Window
    {
        private int count = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        // 탭5: 동적으로 버튼 추가
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            count++;
            spDynamic.Children.Add(new Button
            {
                Content = "동적 버튼 " + count,
                Margin = new Thickness(4),
                Padding = new Thickness(12, 4, 12, 4)
            });
        }

        // 탭5: 동적으로 텍스트 추가
        private void AddText_Click(object sender, RoutedEventArgs e)
        {
            count++;
            spDynamic.Children.Add(new TextBlock
            {
                Text = "동적 텍스트 " + count,
                Margin = new Thickness(4)
            });
        }

        // 탭5: 전체 삭제
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            spDynamic.Children.Clear();
            count = 0;
        }

        // 탭5: Orientation을 Vertical로 변경
        private void SetVertical_Click(object sender, RoutedEventArgs e)
        {
            spOrientation.Orientation = Orientation.Vertical;
            tbOrientation.Text = "현재: Vertical";
        }

        // 탭5: Orientation을 Horizontal로 변경
        private void SetHorizontal_Click(object sender, RoutedEventArgs e)
        {
            spOrientation.Orientation = Orientation.Horizontal;
            tbOrientation.Text = "현재: Horizontal";
        }

        // 탭5: 자식 요소 개수 확인
        private void CheckCount_Click(object sender, RoutedEventArgs e)
        {
            tbCount.Text = "자식 요소 수: " + spCount.Children.Count;
        }
    }
}
