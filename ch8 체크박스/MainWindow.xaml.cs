using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace ch8_체크박스
{
    public partial class MainWindow : Window
    {
        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_4", "<StackPanel>\n    <CheckBox Content=\"체크 안 됨\" Margin=\"0,4\"/>\n    <CheckBox Content=\"체크됨\" IsChecked=\"True\" Margin=\"0,4\"/>\n    <CheckBox Content=\"세 가지 상태\" IsThreeState=\"True\" IsChecked=\"{x:Null}\" Margin=\"0,4\"/>\n</StackPanel>" },
            { "1_5", "<StackPanel>\n    <CheckBox Content=\"활성 체크박스\" Margin=\"0,4\"/>\n    <CheckBox Content=\"비활성 (체크 안 됨)\" IsEnabled=\"False\" Margin=\"0,4\"/>\n    <CheckBox Content=\"비활성 (체크됨)\" IsEnabled=\"False\" IsChecked=\"True\" Margin=\"0,4\"/>\n</StackPanel>" },
            { "1_6", "<StackPanel>\n    <CheckBox Content=\"Release\" ClickMode=\"Release\" Margin=\"0,4\"/>\n    <CheckBox Content=\"Press\" ClickMode=\"Press\" Margin=\"0,4\"/>\n    <CheckBox Content=\"Hover\" ClickMode=\"Hover\" Margin=\"0,4\"/>\n</StackPanel>" },
            { "2_4", "<StackPanel>\n    <CheckBox Content=\"LeftToRight\" FlowDirection=\"LeftToRight\" Margin=\"0,4\"/>\n    <CheckBox Content=\"RightToLeft\" FlowDirection=\"RightToLeft\" Margin=\"0,4\"/>\n</StackPanel>" },
            { "3_4", "<StackPanel>\n    <CheckBox Content=\"Arial\" FontFamily=\"Arial\" Margin=\"0,4\"/>\n    <CheckBox Content=\"Consolas\" FontFamily=\"Consolas\" Margin=\"0,4\"/>\n    <CheckBox Content=\"맑은 고딕\" FontFamily=\"맑은 고딕\" Margin=\"0,4\"/>\n</StackPanel>" },
            { "4_4", "<StackPanel>\n    <CheckBox Content=\"Padding 0\" Padding=\"0\" Margin=\"0,4\"/>\n    <CheckBox Content=\"Padding 10\" Padding=\"10\" Margin=\"0,4\"/>\n    <CheckBox Content=\"Padding 20\" Padding=\"20\" Margin=\"0,4\"/>\n</StackPanel>" },
            { "5_4", "private void ChkSelectAll_Checked(object sender, RoutedEventArgs e)\n{\n    chkItem1.IsChecked = true;\n    chkItem2.IsChecked = true;\n    chkItem3.IsChecked = true;\n}\n\nprivate void ChkSelectAll_Unchecked(object sender, RoutedEventArgs e)\n{\n    chkItem1.IsChecked = false;\n    chkItem2.IsChecked = false;\n    chkItem3.IsChecked = false;\n}" },
            { "2_3", "<StackPanel HorizontalAlignment=\"Center\">\n    <CheckBox Content=\"사과\"/>\n    <CheckBox Content=\"바나나\"/>\n    <CheckBox Content=\"포도\"/>\n</StackPanel>" },
            { "3_3", "<CheckBox FontSize=\"16\" FontWeight=\"Bold\">\n    <TextBlock Text=\"동의합니다\" TextDecorations=\"Underline\"/>\n</CheckBox>" },
            { "4_3", "<CheckBox Content=\"다크 모드\" Foreground=\"White\" Background=\"#333333\" Padding=\"8,4\"/>" },
            // 탭 1: 기본 사용법
            { "1_1", "<CheckBox Content=\"약관에 동의합니다\"/>" },
            { "1_2", "<CheckBox Content=\"자동 로그인\" IsChecked=\"True\"/>" },
            { "1_3", "<CheckBox Content=\"부분 선택\" IsThreeState=\"True\"/>" },

            // 탭 2: 정렬
            { "2_1", "<CheckBox Content=\"중앙 정렬\" HorizontalAlignment=\"Center\"/>" },
            { "2_2", "<CheckBox Content=\"오른쪽 체크\" FlowDirection=\"RightToLeft\"/>" },

            // 탭 3: 폰트와 스타일
            { "3_1", "<CheckBox Content=\"큰 글씨\" FontSize=\"18\"/>" },
            { "3_2", "<CheckBox Content=\"굵은 기울임\" FontWeight=\"Bold\" FontStyle=\"Italic\"/>" },

            // 탭 4: 색상과 외관
            { "4_1", "<CheckBox Content=\"녹색 테마\" Background=\"LightGreen\" Foreground=\"DarkGreen\"/>" },
            { "4_2", "<CheckBox Content=\"반투명\" IsChecked=\"True\" Opacity=\"0.5\"/>" },

            // 탭 5: 이벤트와 상호작용 (코드 비교)
            { "5_1", "private void Chk_Checked(object sender, RoutedEventArgs e)\n{\n    txtStatus.Text = \"체크됨\";\n}" },
            { "5_2", "private void CheckStatus()\n{\n    if (chkBox.IsChecked == true)\n        txtResult.Text = \"체크됨\";\n    else\n        txtResult.Text = \"해제됨\";\n}" },
            { "5_3", "private void ChkAll_Checked(object sender, RoutedEventArgs e)\n{\n    chkItem1.IsChecked = true;\n    chkItem2.IsChecked = true;\n    chkItem3.IsChecked = true;\n}" }
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "5_4", new[] { "chkItem1.IsChecked", "true", "false" } },
            { "5_1", new[] { "txtStatus", ".Text", "체크됨" } },
            { "5_2", new[] { "IsChecked", "== true", "if", "else" } },
            { "5_3", new[] { "IsChecked", "= true", "chkItem1", "chkItem2", "chkItem3" } }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        #region 직접 해보기 이벤트 핸들러

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

        #endregion

        #region 기존 이벤트 핸들러

        // Checked / Unchecked 이벤트
        private void ChkBasic_Checked(object sender, RoutedEventArgs e)
        {
            if (txtBasicStatus != null)
                txtBasicStatus.Text = "체크 상태: 체크됨 ✔";
        }

        private void ChkBasic_Unchecked(object sender, RoutedEventArgs e)
        {
            if (txtBasicStatus != null)
                txtBasicStatus.Text = "체크 상태: 해제됨";
        }

        // IsThreeState + Indeterminate 이벤트
        private void ChkThree_Checked(object sender, RoutedEventArgs e)
        {
            if (txtThreeStatus != null)
                txtThreeStatus.Text = "상태: 체크됨 (True)";
        }

        private void ChkThree_Unchecked(object sender, RoutedEventArgs e)
        {
            if (txtThreeStatus != null)
                txtThreeStatus.Text = "상태: 해제됨 (False)";
        }

        private void ChkThree_Indeterminate(object sender, RoutedEventArgs e)
        {
            if (txtThreeStatus != null)
                txtThreeStatus.Text = "상태: 불확정 (Null)";
        }

        // 여러 체크박스 조합
        private void ChkFruit_Changed(object sender, RoutedEventArgs e)
        {
            if (txtFruitResult == null) return;

            var fruits = new List<string>();
            if (chkApple.IsChecked == true) fruits.Add("사과");
            if (chkBanana.IsChecked == true) fruits.Add("바나나");
            if (chkGrape.IsChecked == true) fruits.Add("포도");
            if (chkOrange.IsChecked == true) fruits.Add("오렌지");

            txtFruitResult.Text = fruits.Count > 0
                ? $"선택된 과일: {string.Join(", ", fruits)}"
                : "선택된 과일: (없음)";
        }

        // 전체 선택 / 해제
        private void ChkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            if (chkItem1 == null) return;
            chkItem1.IsChecked = true;
            chkItem2.IsChecked = true;
            chkItem3.IsChecked = true;
        }

        private void ChkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkItem1 == null) return;
            chkItem1.IsChecked = false;
            chkItem2.IsChecked = false;
            chkItem3.IsChecked = false;
        }

        #endregion
    }
}
