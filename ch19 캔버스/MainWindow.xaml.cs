using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ch19_캔버스
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private UIElement? draggedElement;
        private Point clickPosition;

        public MainWindow()
        {
            InitializeComponent();
        }

        // ZIndex 변경 - 빨강을 위로
        private void BringRedToFront_Click(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(rectA, 1);
            Panel.SetZIndex(rectB, 0);
        }

        // ZIndex 변경 - 초록을 위로
        private void BringGreenToFront_Click(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(rectA, 0);
            Panel.SetZIndex(rectB, 1);
        }

        // 동적 요소 추가 - 캔버스 클릭 시 원 추가
        private void DynamicCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(dynamicCanvas);
            Ellipse ellipse = new Ellipse
            {
                Width = 30,
                Height = 30,
                Fill = new SolidColorBrush(Color.FromRgb(
                    (byte)random.Next(256),
                    (byte)random.Next(256),
                    (byte)random.Next(256)))
            };
            Canvas.SetLeft(ellipse, position.X - 15);
            Canvas.SetTop(ellipse, position.Y - 15);
            dynamicCanvas.Children.Add(ellipse);
        }

        // 동적 캔버스 초기화
        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            dynamicCanvas.Children.Clear();
        }

        // 드래그 시작
        private void Draggable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            draggedElement = sender as UIElement;
            if (draggedElement != null)
            {
                clickPosition = e.GetPosition(draggedElement);
                draggedElement.CaptureMouse();
            }
        }

        // 드래그 중
        private void Draggable_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedElement != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(dragCanvas);
                Canvas.SetLeft(draggedElement, position.X - clickPosition.X);
                Canvas.SetTop(draggedElement, position.Y - clickPosition.Y);
            }
        }

        // 드래그 종료
        private void Draggable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedElement != null)
            {
                draggedElement.ReleaseMouseCapture();
                draggedElement = null;
            }
        }
    }
}
