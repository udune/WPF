using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch33_애니메이션
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "2_4", "<StackPanel>\n    <Border x:Name=\"targetBorder\" Width=\"80\" Height=\"40\" Background=\"SteelBlue\" HorizontalAlignment=\"Left\"/>\n    <Button Content=\"애니메이션 시작\" Width=\"140\" Margin=\"0,8,0,0\" HorizontalAlignment=\"Left\">\n        <Button.Triggers>\n            <EventTrigger RoutedEvent=\"Button.Click\">\n                <BeginStoryboard>\n                    <Storyboard>\n                        <DoubleAnimation Storyboard.TargetName=\"targetBorder\"\n                                         Storyboard.TargetProperty=\"Width\"\n                                         To=\"200\" Duration=\"0:0:0.5\"/>\n                    </Storyboard>\n                </BeginStoryboard>\n            </EventTrigger>\n        </Button.Triggers>\n    </Button>\n</StackPanel>" },
            { "4_4", "<Button Content=\"변형 대상\" Width=\"120\" Height=\"40\" RenderTransformOrigin=\"0.5,0.5\">\n    <Button.RenderTransform>\n        <TransformGroup>\n            <RotateTransform x:Name=\"rotate\" Angle=\"15\"/>\n            <ScaleTransform x:Name=\"scale\" ScaleX=\"1.2\" ScaleY=\"1.2\"/>\n            <TranslateTransform x:Name=\"translate\"/>\n        </TransformGroup>\n    </Button.RenderTransform>\n</Button>" },
            { "5_4", "<StackPanel>\n    <Rectangle x:Name=\"bar\" Width=\"80\" Height=\"30\" Fill=\"MediumSeaGreen\" HorizontalAlignment=\"Left\"/>\n    <Button Content=\"왕복 실행\" Width=\"120\" Margin=\"0,8,0,0\" HorizontalAlignment=\"Left\">\n        <Button.Triggers>\n            <EventTrigger RoutedEvent=\"Button.Click\">\n                <BeginStoryboard>\n                    <Storyboard>\n                        <DoubleAnimation Storyboard.TargetName=\"bar\"\n                                         Storyboard.TargetProperty=\"Width\"\n                                         To=\"220\" Duration=\"0:0:1\" AutoReverse=\"True\"/>\n                    </Storyboard>\n                </BeginStoryboard>\n            </EventTrigger>\n        </Button.Triggers>\n    </Button>\n</StackPanel>" },
            { "1_3", "<Rectangle Width=\"120\" Height=\"30\" Fill=\"#8E24AA\" HorizontalAlignment=\"Left\">\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Height\" To=\"100\" Duration=\"0:0:0.4\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "2_3", "<Rectangle Width=\"100\" Height=\"40\" Fill=\"#00897B\" HorizontalAlignment=\"Left\">\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"Loaded\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Width\" To=\"220\" Duration=\"0:0:0.6\" BeginTime=\"0:0:0.5\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "3_3", "<Rectangle Width=\"100\" Height=\"40\" Fill=\"#5D4037\" HorizontalAlignment=\"Left\">\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Width\" To=\"240\" Duration=\"0:0:0.8\">\n                        <DoubleAnimation.EasingFunction>\n                            <QuadraticEase EasingMode=\"EaseInOut\"/>\n                        </DoubleAnimation.EasingFunction>\n                    </DoubleAnimation>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "4_3", "<Rectangle Width=\"60\" Height=\"40\" Fill=\"#1E88E5\" HorizontalAlignment=\"Left\">\n    <Rectangle.RenderTransform>\n        <TranslateTransform/>\n    </Rectangle.RenderTransform>\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"RenderTransform.X\" To=\"180\" Duration=\"0:0:0.6\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "5_3", "<Border Width=\"160\" Height=\"50\" BorderThickness=\"3\">\n    <Border.BorderBrush>\n        <SolidColorBrush Color=\"Gray\"/>\n    </Border.BorderBrush>\n    <Border.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <ColorAnimation Storyboard.TargetProperty=\"BorderBrush.Color\" To=\"DeepPink\" Duration=\"0:0:0.5\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Border.Triggers>\n</Border>" },
            { "1_1", "<Rectangle Width=\"100\" Height=\"40\" Fill=\"SteelBlue\" HorizontalAlignment=\"Left\">\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Width\" To=\"250\" Duration=\"0:0:0.4\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "1_2", "<Rectangle Width=\"120\" Height=\"40\" Fill=\"Tomato\" HorizontalAlignment=\"Left\">\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Opacity\" From=\"1\" To=\"0.2\" Duration=\"0:0:0.6\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "2_1", "<Rectangle Width=\"80\" Height=\"40\" Fill=\"MediumPurple\" HorizontalAlignment=\"Left\">\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Width\" To=\"200\" Duration=\"0:0:0.5\"/>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Height\" To=\"90\" Duration=\"0:0:0.5\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "2_2", "<Rectangle Width=\"100\" Height=\"40\" Fill=\"Teal\" HorizontalAlignment=\"Left\">\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Width\" To=\"220\" Duration=\"0:0:0.4\" AutoReverse=\"True\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "3_1", "<Rectangle Width=\"100\" Height=\"40\" Fill=\"#3F51B5\" HorizontalAlignment=\"Left\">\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Width\" To=\"240\" Duration=\"0:0:0.8\">\n                        <DoubleAnimation.EasingFunction>\n                            <BounceEase Bounces=\"3\" EasingMode=\"EaseOut\"/>\n                        </DoubleAnimation.EasingFunction>\n                    </DoubleAnimation>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "3_2", "<Rectangle Width=\"100\" Height=\"40\" Fill=\"#009688\" HorizontalAlignment=\"Left\">\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"Width\" To=\"240\" Duration=\"0:0:1\">\n                        <DoubleAnimation.EasingFunction>\n                            <ElasticEase Oscillations=\"3\" EasingMode=\"EaseOut\"/>\n                        </DoubleAnimation.EasingFunction>\n                    </DoubleAnimation>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "4_1", "<Rectangle Width=\"60\" Height=\"60\" Fill=\"#FF5722\" HorizontalAlignment=\"Left\" RenderTransformOrigin=\"0.5,0.5\">\n    <Rectangle.RenderTransform>\n        <RotateTransform/>\n    </Rectangle.RenderTransform>\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"RenderTransform.Angle\" To=\"360\" Duration=\"0:0:1\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "4_2", "<Rectangle Width=\"60\" Height=\"60\" Fill=\"#795548\" HorizontalAlignment=\"Left\" RenderTransformOrigin=\"0.5,0.5\">\n    <Rectangle.RenderTransform>\n        <ScaleTransform/>\n    </Rectangle.RenderTransform>\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <DoubleAnimation Storyboard.TargetProperty=\"RenderTransform.ScaleX\" To=\"1.5\" Duration=\"0:0:0.4\"/>\n                    <DoubleAnimation Storyboard.TargetProperty=\"RenderTransform.ScaleY\" To=\"1.5\" Duration=\"0:0:0.4\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "5_1", "<Rectangle Width=\"140\" Height=\"50\" HorizontalAlignment=\"Left\">\n    <Rectangle.Fill>\n        <SolidColorBrush Color=\"Blue\"/>\n    </Rectangle.Fill>\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"MouseEnter\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <ColorAnimation Storyboard.TargetProperty=\"Fill.Color\" To=\"Red\" Duration=\"0:0:0.6\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
            { "5_2", "<Rectangle Width=\"140\" Height=\"50\" HorizontalAlignment=\"Left\">\n    <Rectangle.Fill>\n        <SolidColorBrush Color=\"Green\"/>\n    </Rectangle.Fill>\n    <Rectangle.Triggers>\n        <EventTrigger RoutedEvent=\"Loaded\">\n            <BeginStoryboard>\n                <Storyboard>\n                    <ColorAnimation Storyboard.TargetProperty=\"Fill.Color\" To=\"Yellow\" Duration=\"0:0:1\" AutoReverse=\"True\" RepeatBehavior=\"Forever\"/>\n                </Storyboard>\n            </BeginStoryboard>\n        </EventTrigger>\n    </Rectangle.Triggers>\n</Rectangle>" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {

        };

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

                if (XamlReader.Parse(fullXaml) is UIElement element)
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

                    var missing = keywords.Where(k => !userCode.Contains(k)).ToList();

                    if (missing.Count == 0)
                    {
                        txtResult.Text = "정답입니다! 모든 필수 요소가 포함되어 있습니다.";
                        txtResult.Foreground = Brushes.Green;
                    }
                    else
                    {
                        txtResult.Text = $"다시 확인해보세요. 누락된 요소: {string.Join(", ", missing)}";
                        txtResult.Foreground = Brushes.Red;
                    }
                }
            }
        }

    }
}
