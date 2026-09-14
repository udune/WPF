using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch32_스타일
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
            { "1_4", "<StackPanel>\n    <StackPanel.Resources>\n        <Style x:Key=\"SampleButtonStyle\" TargetType=\"Button\">\n            <Setter Property=\"Background\" Value=\"SteelBlue\"/>\n            <Setter Property=\"Foreground\" Value=\"White\"/>\n            <Setter Property=\"Margin\" Value=\"0,4\"/>\n        </Style>\n    </StackPanel.Resources>\n    <Button Style=\"{StaticResource SampleButtonStyle}\" Content=\"기본\"/>\n    <Button Style=\"{StaticResource SampleButtonStyle}\" Content=\"배경 변경\" Background=\"Purple\"/>\n    <Button Style=\"{StaticResource SampleButtonStyle}\" Content=\"기본\"/>\n</StackPanel>" },
            { "2_4", "<StackPanel>\n    <StackPanel.Resources>\n        <Style x:Key=\"버튼스타일\" TargetType=\"Button\">\n            <Setter Property=\"Background\" Value=\"#4CAF50\"/>\n            <Setter Property=\"Foreground\" Value=\"White\"/>\n            <Setter Property=\"Margin\" Value=\"0,4\"/>\n        </Style>\n    </StackPanel.Resources>\n    <Button Style=\"{StaticResource 버튼스타일}\" Content=\"StaticResource\"/>\n    <Button Style=\"{DynamicResource 버튼스타일}\" Content=\"DynamicResource\"/>\n</StackPanel>" },
            { "3_4", "<Button Content=\"마우스를 올려보세요\" Width=\"180\" Height=\"40\">\n    <Button.Template>\n        <ControlTemplate TargetType=\"Button\">\n            <Border x:Name=\"border\" CornerRadius=\"10\" Background=\"SteelBlue\" Padding=\"15,10\">\n                <ContentPresenter HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n            </Border>\n            <ControlTemplate.Triggers>\n                <Trigger Property=\"IsMouseOver\" Value=\"True\">\n                    <Setter TargetName=\"border\" Property=\"Background\" Value=\"DarkOrange\"/>\n                </Trigger>\n            </ControlTemplate.Triggers>\n        </ControlTemplate>\n    </Button.Template>\n</Button>" },
            { "4_4", "<StackPanel>\n    <StackPanel.Resources>\n        <Style x:Key=\"BaseButtonStyle\" TargetType=\"Button\">\n            <Setter Property=\"Padding\" Value=\"12,6\"/>\n            <Setter Property=\"Margin\" Value=\"0,4\"/>\n            <Setter Property=\"FontWeight\" Value=\"Bold\"/>\n        </Style>\n        <Style x:Key=\"SuccessButtonStyle\" TargetType=\"Button\" BasedOn=\"{StaticResource BaseButtonStyle}\">\n            <Setter Property=\"Background\" Value=\"#28A745\"/>\n            <Setter Property=\"Foreground\" Value=\"White\"/>\n        </Style>\n        <Style x:Key=\"DangerButtonStyle\" TargetType=\"Button\" BasedOn=\"{StaticResource BaseButtonStyle}\">\n            <Setter Property=\"Background\" Value=\"#DC3545\"/>\n            <Setter Property=\"Foreground\" Value=\"White\"/>\n        </Style>\n    </StackPanel.Resources>\n    <Button Style=\"{StaticResource SuccessButtonStyle}\" Content=\"성공\"/>\n    <Button Style=\"{StaticResource DangerButtonStyle}\" Content=\"위험\"/>\n</StackPanel>" },
            { "5_4", "<StackPanel>\n    <StackPanel.Resources>\n        <Style TargetType=\"CheckBox\">\n            <Style.Triggers>\n                <Trigger Property=\"IsChecked\" Value=\"True\">\n                    <Setter Property=\"Foreground\" Value=\"Green\"/>\n                    <Setter Property=\"FontWeight\" Value=\"Bold\"/>\n                </Trigger>\n            </Style.Triggers>\n        </Style>\n    </StackPanel.Resources>\n    <CheckBox Content=\"체크해 보세요\" Margin=\"0,4\"/>\n    <CheckBox Content=\"이것도 체크해 보세요\" Margin=\"0,4\"/>\n</StackPanel>" },
            { "1_3", "<TextBlock Text=\"스타일 연습\">\n    <TextBlock.Style>\n        <Style TargetType=\"TextBlock\">\n            <Setter Property=\"Foreground\" Value=\"DarkGreen\"/>\n            <Setter Property=\"Margin\" Value=\"6\"/>\n            <Setter Property=\"FontSize\" Value=\"15\"/>\n        </Style>\n    </TextBlock.Style>\n</TextBlock>" },
            { "2_3", "<StackPanel>\n    <StackPanel.Resources>\n        <SolidColorBrush x:Key=\"Accent\" Color=\"#7B1FA2\"/>\n    </StackPanel.Resources>\n    <Button Content=\"버튼\" Width=\"140\" Background=\"{StaticResource Accent}\" Foreground=\"White\"/>\n    <Border Height=\"30\" Margin=\"0,6,0,0\" Background=\"{StaticResource Accent}\"/>\n</StackPanel>" },
            { "3_3", "<Button Content=\"납작 버튼\" Width=\"140\" Height=\"34\">\n    <Button.Template>\n        <ControlTemplate TargetType=\"Button\">\n            <Border Background=\"Transparent\" BorderBrush=\"#455A64\" BorderThickness=\"1\">\n                <ContentPresenter HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n            </Border>\n        </ControlTemplate>\n    </Button.Template>\n</Button>" },
            { "4_3", "<StackPanel>\n    <StackPanel.Resources>\n        <Style x:Key=\"BaseText\" TargetType=\"TextBlock\">\n            <Setter Property=\"FontSize\" Value=\"14\"/>\n            <Setter Property=\"Margin\" Value=\"0,3,0,3\"/>\n        </Style>\n        <Style x:Key=\"WarnText\" TargetType=\"TextBlock\" BasedOn=\"{StaticResource BaseText}\">\n            <Setter Property=\"Foreground\" Value=\"Red\"/>\n            <Setter Property=\"FontWeight\" Value=\"Bold\"/>\n        </Style>\n    </StackPanel.Resources>\n    <TextBlock Text=\"보통 안내\" Style=\"{StaticResource BaseText}\"/>\n    <TextBlock Text=\"경고 문구\" Style=\"{StaticResource WarnText}\"/>\n</StackPanel>" },
            { "5_3", "<CheckBox Content=\"완료\">\n    <CheckBox.Style>\n        <Style TargetType=\"CheckBox\">\n            <Style.Triggers>\n                <Trigger Property=\"IsChecked\" Value=\"True\">\n                    <Setter Property=\"Foreground\" Value=\"Green\"/>\n                    <Setter Property=\"FontWeight\" Value=\"Bold\"/>\n                </Trigger>\n            </Style.Triggers>\n        </Style>\n    </CheckBox.Style>\n</CheckBox>" },
            { "1_1", "<Button Content=\"스타일 적용\" Width=\"140\">\n    <Button.Style>\n        <Style TargetType=\"Button\">\n            <Setter Property=\"FontSize\" Value=\"16\"/>\n            <Setter Property=\"FontWeight\" Value=\"Bold\"/>\n        </Style>\n    </Button.Style>\n</Button>" },
            { "1_2", "<StackPanel>\n    <StackPanel.Resources>\n        <Style x:Key=\"BigButton\" TargetType=\"Button\">\n            <Setter Property=\"FontSize\" Value=\"16\"/>\n            <Setter Property=\"Foreground\" Value=\"DarkBlue\"/>\n        </Style>\n    </StackPanel.Resources>\n    <Button Content=\"적용됨\" Width=\"140\" Style=\"{StaticResource BigButton}\"/>\n    <Button Content=\"적용 안 됨\" Width=\"140\" Margin=\"0,6,0,0\"/>\n</StackPanel>" },
            { "2_1", "<StackPanel>\n    <StackPanel.Resources>\n        <Style TargetType=\"Button\">\n            <Setter Property=\"Background\" Value=\"#2196F3\"/>\n            <Setter Property=\"Foreground\" Value=\"White\"/>\n            <Setter Property=\"Padding\" Value=\"8,4\"/>\n        </Style>\n    </StackPanel.Resources>\n    <Button Content=\"첫째\" Width=\"140\"/>\n    <Button Content=\"둘째\" Width=\"140\" Margin=\"0,6,0,0\"/>\n</StackPanel>" },
            { "2_2", "<StackPanel>\n    <StackPanel.Resources>\n        <Style TargetType=\"TextBlock\">\n            <Setter Property=\"FontSize\" Value=\"14\"/>\n            <Setter Property=\"Margin\" Value=\"0,3,0,3\"/>\n        </Style>\n    </StackPanel.Resources>\n    <TextBlock Text=\"첫째 줄\"/>\n    <TextBlock Text=\"둘째 줄\"/>\n</StackPanel>" },
            { "3_1", "<Button Content=\"둥근 버튼\" Width=\"140\" Height=\"36\">\n    <Button.Template>\n        <ControlTemplate TargetType=\"Button\">\n            <Border Background=\"#4CAF50\" CornerRadius=\"18\">\n                <ContentPresenter HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n            </Border>\n        </ControlTemplate>\n    </Button.Template>\n</Button>" },
            { "3_2", "<Button Content=\"가운데\" Width=\"140\" Height=\"36\">\n    <Button.Template>\n        <ControlTemplate TargetType=\"Button\">\n            <Border Background=\"#FF9800\">\n                <ContentPresenter HorizontalAlignment=\"Center\" VerticalAlignment=\"Center\"/>\n            </Border>\n        </ControlTemplate>\n    </Button.Template>\n</Button>" },
            { "4_1", "<StackPanel>\n    <StackPanel.Resources>\n        <Style x:Key=\"Base\" TargetType=\"Button\">\n            <Setter Property=\"FontSize\" Value=\"14\"/>\n            <Setter Property=\"Padding\" Value=\"8,4\"/>\n        </Style>\n        <Style x:Key=\"Accent\" TargetType=\"Button\" BasedOn=\"{StaticResource Base}\">\n            <Setter Property=\"Background\" Value=\"#E91E63\"/>\n            <Setter Property=\"Foreground\" Value=\"White\"/>\n        </Style>\n    </StackPanel.Resources>\n    <Button Content=\"기본\" Style=\"{StaticResource Base}\" Width=\"140\"/>\n    <Button Content=\"강조\" Style=\"{StaticResource Accent}\" Width=\"140\" Margin=\"0,6,0,0\"/>\n</StackPanel>" },
            { "4_2", "<StackPanel>\n    <StackPanel.Resources>\n        <Style x:Key=\"Base2\" TargetType=\"TextBlock\">\n            <Setter Property=\"FontSize\" Value=\"12\"/>\n            <Setter Property=\"Foreground\" Value=\"Gray\"/>\n        </Style>\n        <Style x:Key=\"Big\" TargetType=\"TextBlock\" BasedOn=\"{StaticResource Base2}\">\n            <Setter Property=\"FontSize\" Value=\"20\"/>\n        </Style>\n    </StackPanel.Resources>\n    <TextBlock Text=\"큰 글씨\" Style=\"{StaticResource Big}\"/>\n</StackPanel>" },
            { "5_1", "<Button Content=\"마우스를 올려보세요\" Width=\"180\" Height=\"34\">\n    <Button.Style>\n        <Style TargetType=\"Button\">\n            <Style.Triggers>\n                <Trigger Property=\"IsMouseOver\" Value=\"True\">\n                    <Setter Property=\"Background\" Value=\"Orange\"/>\n                </Trigger>\n            </Style.Triggers>\n        </Style>\n    </Button.Style>\n</Button>" },
            { "5_2", "<Button Content=\"멀티 트리거\" Width=\"180\" Height=\"34\">\n    <Button.Style>\n        <Style TargetType=\"Button\">\n            <Style.Triggers>\n                <MultiTrigger>\n                    <MultiTrigger.Conditions>\n                        <Condition Property=\"IsMouseOver\" Value=\"True\"/>\n                        <Condition Property=\"IsEnabled\" Value=\"True\"/>\n                    </MultiTrigger.Conditions>\n                    <Setter Property=\"Background\" Value=\"#8BC34A\"/>\n                </MultiTrigger>\n            </Style.Triggers>\n        </Style>\n    </Button.Style>\n</Button>" },
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
