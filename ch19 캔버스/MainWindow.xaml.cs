using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace ch19_캔버스
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private UIElement? draggedElement;
        private Point clickPosition;

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "2_4", "<Canvas Height=\"100\" Background=\"#FFFDE7\">\n    <Button Content=\"위치 미지정\"/>\n    <Button Content=\"120,40\" Canvas.Left=\"120\" Canvas.Top=\"40\"/>\n</Canvas>" },
            { "3_4", "private void BringRedToFront_Click(object sender, RoutedEventArgs e)\n{\n    Panel.SetZIndex(rectA, 1);\n    Panel.SetZIndex(rectB, 0);\n}" },
            { "4_4", "<Path Stroke=\"Crimson\" StrokeThickness=\"2\" Fill=\"#FFEBEE\"\n      Data=\"M 0,50 L 25,0 L 50,50 L 25,100 Z\"/>" },
            { "5_4", "<Canvas Width=\"150\" Height=\"100\" Background=\"#E8F5E9\" ClipToBounds=\"True\">\n    <Ellipse Fill=\"MediumSeaGreen\" Width=\"100\" Height=\"100\"\n             Canvas.Left=\"100\" Canvas.Top=\"40\"/>\n</Canvas>" },
            { "1_3", "<Canvas Height=\"140\" Background=\"#FAFAFA\">\n    <Rectangle Canvas.Left=\"20\" Canvas.Top=\"20\" Width=\"80\" Height=\"50\" Fill=\"SteelBlue\"/>\n</Canvas>" },
            { "2_3", "<Canvas Height=\"140\" Background=\"#FAFAFA\">\n    <Ellipse Canvas.Right=\"15\" Canvas.Bottom=\"15\" Width=\"50\" Height=\"50\" Fill=\"Tomato\"/>\n</Canvas>" },
            { "3_3", "<Canvas Height=\"140\" Background=\"#FAFAFA\">\n    <Rectangle Canvas.Left=\"20\" Canvas.Top=\"20\" Width=\"80\" Height=\"60\" Fill=\"Red\" Panel.ZIndex=\"1\"/>\n    <Rectangle Canvas.Left=\"50\" Canvas.Top=\"40\" Width=\"80\" Height=\"60\" Fill=\"Blue\" Panel.ZIndex=\"2\"/>\n    <Rectangle Canvas.Left=\"80\" Canvas.Top=\"30\" Width=\"80\" Height=\"60\" Fill=\"Green\" Panel.ZIndex=\"3\"/>\n</Canvas>" },
            { "5_3", "<Canvas Height=\"140\" Background=\"#FAFAFA\">\n    <Rectangle Canvas.Left=\"30\" Canvas.Bottom=\"0\" Width=\"40\" Height=\"60\" Fill=\"#42A5F5\"/>\n    <Rectangle Canvas.Left=\"90\" Canvas.Bottom=\"0\" Width=\"40\" Height=\"100\" Fill=\"#66BB6A\"/>\n    <Rectangle Canvas.Left=\"150\" Canvas.Bottom=\"0\" Width=\"40\" Height=\"80\" Fill=\"#FFA726\"/>\n</Canvas>" },
            // 탭 1: 기본 사용법
            { "1_1", "<Canvas Height=\"120\" Background=\"#EEEEEE\">\n    <Rectangle Fill=\"Orange\" Width=\"100\" Height=\"60\" Canvas.Left=\"50\" Canvas.Top=\"30\"/>\n</Canvas>" },
            { "1_2", "<Canvas Height=\"120\" Background=\"#FFF8E1\">\n    <Ellipse Fill=\"LightBlue\" Width=\"80\" Height=\"80\" Canvas.Left=\"100\" Canvas.Top=\"20\"/>\n    <TextBlock Text=\"원 위에 텍스트\" Canvas.Left=\"110\" Canvas.Top=\"50\"/>\n</Canvas>" },

            // 탭 2: 위치 지정
            { "2_1", "<Canvas Height=\"120\" Background=\"#E8EAF6\">\n    <Rectangle Fill=\"Purple\" Width=\"80\" Height=\"50\" Canvas.Right=\"20\" Canvas.Bottom=\"20\"/>\n</Canvas>" },
            { "2_2", "<Canvas Height=\"120\" Background=\"#ECEFF1\">\n    <Ellipse Fill=\"Red\" Width=\"40\" Height=\"40\" Canvas.Left=\"10\" Canvas.Top=\"10\"/>\n    <Ellipse Fill=\"Blue\" Width=\"40\" Height=\"40\" Canvas.Right=\"10\" Canvas.Top=\"10\"/>\n    <Ellipse Fill=\"Green\" Width=\"40\" Height=\"40\" Canvas.Left=\"10\" Canvas.Bottom=\"10\"/>\n    <Ellipse Fill=\"Yellow\" Width=\"40\" Height=\"40\" Canvas.Right=\"10\" Canvas.Bottom=\"10\"/>\n</Canvas>" },

            // 탭 3: ZIndex
            { "3_1", "<Canvas Height=\"130\" Background=\"#FAFAFA\">\n    <Ellipse Fill=\"Blue\" Width=\"80\" Height=\"80\" Canvas.Left=\"50\" Canvas.Top=\"25\" Panel.ZIndex=\"1\"/>\n    <Ellipse Fill=\"Red\" Width=\"80\" Height=\"80\" Canvas.Left=\"90\" Canvas.Top=\"35\" Panel.ZIndex=\"2\"/>\n    <Ellipse Fill=\"Yellow\" Width=\"80\" Height=\"80\" Canvas.Left=\"130\" Canvas.Top=\"25\" Panel.ZIndex=\"3\"/>\n</Canvas>" },
            { "3_2", "Panel.SetZIndex(element, 10);" },

            // 탭 4: 도형 그리기
            { "4_1", "<Canvas Height=\"120\" Background=\"#F5F5F5\">\n    <Rectangle Fill=\"LightGreen\" Stroke=\"DarkGreen\" StrokeThickness=\"2\" Width=\"100\" Height=\"60\" RadiusX=\"10\" RadiusY=\"10\" Canvas.Left=\"50\" Canvas.Top=\"30\"/>\n</Canvas>" },
            { "4_2", "<Canvas Height=\"100\" Background=\"#EDE7F6\">\n    <Line X1=\"30\" Y1=\"20\" X2=\"200\" Y2=\"80\" Stroke=\"Purple\" StrokeThickness=\"3\"/>\n</Canvas>" },
            { "4_3", "<Canvas Height=\"120\" Background=\"#FFF8E1\">\n    <Polygon Points=\"100,20 150,100 50,100\" Fill=\"LightCoral\" Stroke=\"DarkRed\" StrokeThickness=\"2\"/>\n</Canvas>" },

            // 탭 5: 실용 예제
            { "5_1", "Rectangle rect = new Rectangle\n{\n    Width = 50,\n    Height = 50,\n    Fill = Brushes.Blue\n};\nCanvas.SetLeft(rect, 100);\nCanvas.SetTop(rect, 50);\nmyCanvas.Children.Add(rect);" },
            { "5_2", "private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)\n{\n    UIElement element = sender as UIElement;\n    element.CaptureMouse();\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "3_4", new[] { "Panel.SetZIndex", "rectA", "rectB" } },
            { "3_2", new[] { "Panel.SetZIndex", "element", "10" } },
            { "5_1", new[] { "Rectangle", "Width", "Height", "Fill", "Canvas.SetLeft", "Canvas.SetTop", "Children.Add" } },
            { "5_2", new[] { "UIElement", "sender", "CaptureMouse" } },
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        // XAML 실행
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
