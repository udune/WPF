using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch22_툴바
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 볼륨 슬라이더 변경
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (volumeText != null)
            {
                volumeText.Text = $"{volumeSlider.Value:0}%";
            }
        }

        // ToolBarTray 잠금
        private void LockCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            toggleableTray.IsLocked = true;
        }

        // ToolBarTray 잠금 해제
        private void LockCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            toggleableTray.IsLocked = false;
        }

        // 브라우저 이동 버튼
        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"이동: {urlTextBox.Text}", "알림");
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_3", "<ToolBarTray>\n    <ToolBar Header=\"편집\">\n        <Button Content=\"복사\"/>\n        <Button Content=\"붙여넣기\"/>\n    </ToolBar>\n</ToolBarTray>" },
            { "2_3", "<ToolBar>\n    <Button>\n        <Ellipse Width=\"14\" Height=\"14\" Fill=\"Crimson\"/>\n    </Button>\n    <Button>\n        <Rectangle Width=\"14\" Height=\"14\" Fill=\"SeaGreen\"/>\n    </Button>\n</ToolBar>" },
            { "3_3", "<ToolBar Width=\"180\">\n    <Button Content=\"하나\"/>\n    <Button Content=\"둘\"/>\n    <Button Content=\"셋\"/>\n    <Button Content=\"넷\"/>\n    <Button Content=\"다섯\"/>\n</ToolBar>" },
            { "4_3", "<ToolBarTray>\n    <ToolBar Band=\"0\" BandIndex=\"0\">\n        <Button Content=\"앞\"/>\n    </ToolBar>\n    <ToolBar Band=\"0\" BandIndex=\"1\">\n        <Button Content=\"뒤\"/>\n    </ToolBar>\n</ToolBarTray>" },
            { "5_3", "<ToolBar>\n    <Button Content=\"재생\"/>\n    <Button Content=\"일시정지\"/>\n    <Button Content=\"정지\"/>\n    <Separator/>\n    <Slider Width=\"100\" Minimum=\"0\" Maximum=\"100\" Value=\"70\" VerticalAlignment=\"Center\"/>\n</ToolBar>" },
            { "1_1", "<ToolBar>\n    <Button Content=\"굵게\"/>\n    <Button Content=\"기울임\"/>\n    <Button Content=\"밑줄\"/>\n</ToolBar>" },
            { "1_2", "<ToolBarTray>\n    <ToolBar>\n        <Button Content=\"새로 만들기\"/>\n        <Separator/>\n        <Button Content=\"열기\"/>\n    </ToolBar>\n    <ToolBar>\n        <Button Content=\"저장\"/>\n    </ToolBar>\n</ToolBarTray>" },
            { "2_1", "<ToolBar>\n    <ComboBox Width=\"100\" SelectedIndex=\"0\">\n        <ComboBoxItem Content=\"맑은 고딕\"/>\n        <ComboBoxItem Content=\"굴림\"/>\n        <ComboBoxItem Content=\"바탕\"/>\n    </ComboBox>\n</ToolBar>" },
            { "2_2", "<ToolBar>\n    <ToggleButton Content=\"굵게\"/>\n    <Separator/>\n    <CheckBox Content=\"자동 줄바꿈\" VerticalAlignment=\"Center\"/>\n</ToolBar>" },
            { "3_1", "<ToolBar Width=\"200\">\n    <Button Content=\"자주 쓰는 명령\"/>\n    <Button Content=\"가끔 쓰는 명령\" ToolBar.OverflowMode=\"Always\"/>\n</ToolBar>" },
            { "3_2", "<ToolBar Width=\"150\">\n    <Button Content=\"저장\" ToolBar.OverflowMode=\"Never\"/>\n    <Button Content=\"인쇄\"/>\n    <Button Content=\"미리보기\"/>\n</ToolBar>" },
            { "4_1", "<ToolBarTray>\n    <ToolBar Band=\"0\" BandIndex=\"0\">\n        <Button Content=\"첫째 줄\"/>\n    </ToolBar>\n    <ToolBar Band=\"1\" BandIndex=\"0\">\n        <Button Content=\"둘째 줄\"/>\n    </ToolBar>\n</ToolBarTray>" },
            { "4_2", "<ToolBarTray Orientation=\"Vertical\">\n    <ToolBar>\n        <Button Content=\"위\"/>\n        <Button Content=\"가운데\"/>\n        <Button Content=\"아래\"/>\n    </ToolBar>\n</ToolBarTray>" },
            { "5_1", "<ToolBar>\n    <Button Content=\"새로 만들기\"/>\n    <Button Content=\"열기\"/>\n    <Button Content=\"저장\"/>\n    <Separator/>\n    <ToggleButton Content=\"굵게\"/>\n    <ToggleButton Content=\"기울임\"/>\n</ToolBar>" },
            { "5_2", "private void Lock_Click(object sender, RoutedEventArgs e)\n{\n    tray.IsLocked = true;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_2", new[] { "IsLocked", "true" } },
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
