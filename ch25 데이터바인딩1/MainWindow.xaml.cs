using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace ch25_데이터바인딩1
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        // 기본 사용법 - 배경색 변경
        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            var colors = new SolidColorBrush[]
            {
                Brushes.Salmon,
                Brushes.LightGreen,
                Brushes.LightBlue,
                Brushes.Gold,
                Brushes.LightCoral,
                Brushes.MediumPurple
            };

            colorSourceBtn.Background = colors[random.Next(colors.Length)];
        }

        // UpdateSourceTrigger - Explicit 업데이트
        private void UpdateExplicit_Click(object sender, RoutedEventArgs e)
        {
            var binding = explicitTextBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
            MessageBox.Show("소스가 업데이트되었습니다!", "알림");
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_4", "<StackPanel>\n    <CheckBox x:Name=\"enableCheckBox\" Content=\"버튼 활성화\" IsChecked=\"True\"/>\n    <Button Content=\"실행\" Margin=\"0,8,0,0\"\n            IsEnabled=\"{Binding ElementName=enableCheckBox, Path=IsChecked}\"/>\n</StackPanel>" },
            { "2_4", "<StackPanel>\n    <TextBox x:Name=\"oneTimeSource\" Text=\"처음 값\"/>\n    <TextBlock Margin=\"0,8,0,0\"\n               Text=\"{Binding ElementName=oneTimeSource, Path=Text, Mode=OneTime}\"/>\n</StackPanel>" },
            { "3_4", "private void UpdateExplicit_Click(object sender, RoutedEventArgs e)\n{\n    var binding = explicitTextBox.GetBindingExpression(TextBox.TextProperty);\n    binding?.UpdateSource();\n}" },
            { "4_4", "<TextBlock Text=\"{Binding NonExistent, FallbackValue=기본값}\"/>" },
            { "1_3", "<StackPanel>\n    <CheckBox x:Name=\"chk\" Content=\"동의\"/>\n    <TextBlock Text=\"{Binding ElementName=chk, Path=IsChecked}\" Margin=\"0,6,0,0\"/>\n</StackPanel>" },
            { "2_3", "<StackPanel>\n    <CheckBox x:Name=\"agree\" Content=\"약관에 동의합니다\"/>\n    <Button Content=\"다음\" Width=\"120\" HorizontalAlignment=\"Left\" Margin=\"0,6,0,0\" IsEnabled=\"{Binding ElementName=agree, Path=IsChecked}\"/>\n</StackPanel>" },
            { "3_3", "<StackPanel>\n    <Slider x:Name=\"s6\" Minimum=\"0\" Maximum=\"100\" Width=\"220\"/>\n    <TextBox Width=\"220\" Text=\"{Binding ElementName=s6, Path=Value, UpdateSourceTrigger=PropertyChanged}\"/>\n</StackPanel>" },
            { "4_3", "<StackPanel>\n    <Slider x:Name=\"s7\" Minimum=\"0\" Maximum=\"10\" Width=\"220\"/>\n    <TextBlock Text=\"{Binding ElementName=s7, Path=Value, StringFormat='{}{0:F2}'}\" Margin=\"0,6,0,0\"/>\n</StackPanel>" },
            { "5_3", "<StackPanel>\n    <Slider x:Name=\"size\" Minimum=\"10\" Maximum=\"40\" Value=\"16\" Width=\"220\"/>\n    <TextBlock Text=\"미리보기 글자\" FontSize=\"{Binding ElementName=size, Path=Value}\" Margin=\"0,8,0,0\"/>\n</StackPanel>" },
            { "1_1", "<StackPanel>\n    <Slider x:Name=\"sld\" Minimum=\"0\" Maximum=\"100\" Width=\"200\"/>\n    <TextBlock Text=\"{Binding ElementName=sld, Path=Value}\"/>\n</StackPanel>" },
            { "1_2", "<StackPanel>\n    <TextBox x:Name=\"input\" Width=\"200\"/>\n    <TextBlock Text=\"{Binding ElementName=input, Path=Text}\"/>\n</StackPanel>" },
            { "2_1", "<StackPanel>\n    <TextBox x:Name=\"a\" Width=\"200\" Text=\"처음 값\"/>\n    <TextBox Width=\"200\" Text=\"{Binding ElementName=a, Path=Text, Mode=TwoWay}\"/>\n</StackPanel>" },
            { "2_2", "<StackPanel>\n    <Slider x:Name=\"s2\" Minimum=\"0\" Maximum=\"100\" Width=\"200\"/>\n    <ProgressBar Height=\"20\" Minimum=\"0\" Maximum=\"100\" Value=\"{Binding ElementName=s2, Path=Value, Mode=OneWay}\"/>\n</StackPanel>" },
            { "3_1", "<StackPanel>\n    <TextBox x:Name=\"src\" Width=\"200\"/>\n    <TextBox Width=\"200\" Text=\"{Binding ElementName=src, Path=Text, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}\"/>\n</StackPanel>" },
            { "3_2", "<StackPanel>\n    <TextBox x:Name=\"src2\" Width=\"200\"/>\n    <TextBox Width=\"200\" Text=\"{Binding ElementName=src2, Path=Text, Mode=TwoWay, UpdateSourceTrigger=LostFocus}\"/>\n</StackPanel>" },
            { "4_1", "<StackPanel>\n    <Slider x:Name=\"s4\" Minimum=\"0\" Maximum=\"100\" Width=\"200\"/>\n    <TextBlock Text=\"{Binding ElementName=s4, Path=Value, StringFormat='현재 값: {0:F0} 점'}\"/>\n</StackPanel>" },
            { "4_2", "<StackPanel>\n    <TextBox x:Name=\"s5\" Width=\"200\"/>\n    <TextBlock Text=\"{Binding ElementName=없는이름, Path=Text, FallbackValue=(입력 없음)}\"/>\n</StackPanel>" },
            { "5_1", "<StackPanel>\n    <Slider x:Name=\"vol\" Minimum=\"0\" Maximum=\"100\" Width=\"220\"/>\n    <ProgressBar Height=\"18\" Minimum=\"0\" Maximum=\"100\" Value=\"{Binding ElementName=vol, Path=Value}\" Margin=\"0,6,0,0\"/>\n    <TextBlock Text=\"{Binding ElementName=vol, Path=Value, StringFormat='볼륨 {0:F0}%'}\" Margin=\"0,6,0,0\"/>\n</StackPanel>" },
            { "5_2", "private void Bind_Click(object sender, RoutedEventArgs e)\n{\n    Binding binding = new Binding(\"Text\");\n    binding.Source = src;\n    target.SetBinding(TextBlock.TextProperty, binding);\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "3_4", new[] { "GetBindingExpression", "UpdateSource" } },
            { "5_2", new[] { "new Binding", "Source", "SetBinding", "TextProperty" } },
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
