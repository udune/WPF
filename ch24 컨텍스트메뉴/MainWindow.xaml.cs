using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace ch24_컨텍스트메뉴
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        // 기본 사용법 - 메시지 표시
        private void ShowMessage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("메뉴 항목이 클릭되었습니다!", "알림");
        }

        // 기본 사용법 - 시간 표시
        private void ShowTime_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"현재 시간: {DateTime.Now:HH:mm:ss}", "시간");
        }

        // 계층 구조와 스타일 - 굵게
        private void Bold_Checked(object sender, RoutedEventArgs e)
        {
            checkableText.FontWeight = FontWeights.Bold;
        }

        private void Bold_Unchecked(object sender, RoutedEventArgs e)
        {
            checkableText.FontWeight = FontWeights.Normal;
        }

        // 계층 구조와 스타일 - 기울임
        private void Italic_Checked(object sender, RoutedEventArgs e)
        {
            checkableText.FontStyle = FontStyles.Italic;
        }

        private void Italic_Unchecked(object sender, RoutedEventArgs e)
        {
            checkableText.FontStyle = FontStyles.Normal;
        }

        // 계층 구조와 스타일 - 밑줄
        private void Underline_Checked(object sender, RoutedEventArgs e)
        {
            checkableText.TextDecorations = TextDecorations.Underline;
        }

        private void Underline_Unchecked(object sender, RoutedEventArgs e)
        {
            checkableText.TextDecorations = null;
        }

        // 다양한 컨트롤 - 항목 추가
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("항목 추가 기능", "알림");
        }

        // 다양한 컨트롤 - 항목 삭제
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("선택 항목 삭제 기능", "알림");
        }

        // 다양한 컨트롤 - Rectangle 색상 변경
        private void SetRed_Click(object sender, RoutedEventArgs e)
        {
            colorRect.Fill = Brushes.Red;
        }

        private void SetGreen_Click(object sender, RoutedEventArgs e)
        {
            colorRect.Fill = Brushes.Green;
        }

        private void SetBlue_Click(object sender, RoutedEventArgs e)
        {
            colorRect.Fill = Brushes.Blue;
        }

        private void SetRandom_Click(object sender, RoutedEventArgs e)
        {
            colorRect.Fill = new SolidColorBrush(Color.FromRgb(
                (byte)random.Next(256),
                (byte)random.Next(256),
                (byte)random.Next(256)));
        }

        // 다양한 컨트롤 - Ellipse 색상 변경
        private void SetEllipseRed_Click(object sender, RoutedEventArgs e)
        {
            colorEllipse.Fill = Brushes.Red;
        }

        private void SetEllipseGreen_Click(object sender, RoutedEventArgs e)
        {
            colorEllipse.Fill = Brushes.Green;
        }

        private void SetEllipseBlue_Click(object sender, RoutedEventArgs e)
        {
            colorEllipse.Fill = Brushes.Blue;
        }

        // 다양한 컨트롤 - 텍스트 지우기
        private void ClearText_Click(object sender, RoutedEventArgs e)
        {
            customContextTextBox.Text = "";
        }

        // 동적 ContextMenu - 코드에서 생성
        private void CreateContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var contextMenu = new ContextMenu();

            var item1 = new MenuItem { Header = "동적 항목 1" };
            item1.Click += (s, args) => MessageBox.Show("항목 1 클릭", "동적 메뉴");

            var item2 = new MenuItem { Header = "동적 항목 2" };
            item2.Click += (s, args) => MessageBox.Show("항목 2 클릭", "동적 메뉴");

            var item3 = new MenuItem { Header = "현재 시간" };
            item3.Click += (s, args) => MessageBox.Show($"시간: {DateTime.Now:HH:mm:ss}", "동적 메뉴");

            contextMenu.Items.Add(item1);
            contextMenu.Items.Add(item2);
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(item3);

            dynamicContextBorder.ContextMenu = contextMenu;
            MessageBox.Show("ContextMenu가 생성되었습니다. 영역을 우클릭하세요.", "알림");
        }

        // 동적 ContextMenu - ContextMenuOpening
        private void DynamicBorder_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // 기존 동적 항목 제거 (첫 번째 고정 항목 제외)
            while (dynamicMenu.Items.Count > 1)
            {
                dynamicMenu.Items.RemoveAt(1);
            }

            // 동적 항목 추가
            dynamicMenu.Items.Add(new Separator());
            dynamicMenu.Items.Add(new MenuItem { Header = $"현재 시간: {DateTime.Now:HH:mm:ss}" });
            dynamicMenu.Items.Add(new MenuItem { Header = $"랜덤 숫자: {random.Next(100)}" });
        }

        // 동적 ContextMenu - 조건부 메뉴
        private void ConditionalBorder_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            deleteMenuItem.IsEnabled = enableDeleteCheck.IsChecked == true;
        }

        // 동적 ContextMenu - 프로그래밍 방식으로 열기
        private void OpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (programmaticButton.ContextMenu != null)
            {
                programmaticButton.ContextMenu.PlacementTarget = programmaticButton;
                programmaticButton.ContextMenu.IsOpen = true;
            }
        }

        // 실용 예제 - 파일 열기
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (fileListBox.SelectedItem is ListBoxItem item)
            {
                MessageBox.Show($"파일 열기: {item.Content}", "열기");
            }
            else
            {
                MessageBox.Show("파일을 선택하세요.", "알림");
            }
        }

        // 실용 예제 - 파일 삭제
        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (fileListBox.SelectedItem is ListBoxItem item)
            {
                var result = MessageBox.Show($"'{item.Content}'을(를) 삭제하시겠습니까?", "삭제 확인",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    fileListBox.Items.Remove(item);
                }
            }
            else
            {
                MessageBox.Show("삭제할 파일을 선택하세요.", "알림");
            }
        }

        // 실용 예제 - 파일 속성
        private void FileProperties_Click(object sender, RoutedEventArgs e)
        {
            if (fileListBox.SelectedItem is ListBoxItem item)
            {
                MessageBox.Show($"파일: {item.Content}\n유형: 파일\n크기: 알 수 없음", "속성");
            }
            else
            {
                MessageBox.Show("파일을 선택하세요.", "알림");
            }
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_4", "private void ShowMessage_Click(object sender, RoutedEventArgs e)\n{\n    MessageBox.Show(\"메뉴 항목이 클릭되었습니다!\", \"알림\");\n}" },
            { "2_4", "<Button Content=\"마우스 오른쪽 클릭\" Padding=\"10\">\n    <Button.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"실행 취소\" IsEnabled=\"False\"/>\n            <Separator/>\n            <MenuItem Header=\"복사\"/>\n            <MenuItem Header=\"붙여넣기\" IsEnabled=\"False\"/>\n        </ContextMenu>\n    </Button.ContextMenu>\n</Button>" },
            { "3_4", "<TextBox Text=\"오른쪽 클릭해 보세요\" Width=\"220\">\n    <TextBox.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"잘라내기\" Command=\"ApplicationCommands.Cut\"/>\n            <MenuItem Header=\"복사\" Command=\"ApplicationCommands.Copy\"/>\n            <MenuItem Header=\"붙여넣기\" Command=\"ApplicationCommands.Paste\"/>\n            <Separator/>\n            <MenuItem Header=\"모두 선택\" Command=\"ApplicationCommands.SelectAll\"/>\n        </ContextMenu>\n    </TextBox.ContextMenu>\n</TextBox>" },
            { "4_4", "private void OpenContextMenu_Click(object sender, RoutedEventArgs e)\n{\n    programmaticButton.ContextMenu.PlacementTarget = programmaticButton;\n    programmaticButton.ContextMenu.IsOpen = true;\n}" },
            { "1_3", "<TextBlock Text=\"오른쪽 클릭\" Padding=\"30\" Background=\"#EEEEEE\">\n    <TextBlock.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"새로 고침\"/>\n        </ContextMenu>\n    </TextBlock.ContextMenu>\n</TextBlock>" },
            { "2_3", "<Button Content=\"오른쪽 클릭\" Padding=\"20,10\">\n    <Button.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"열기\">\n                <MenuItem.Icon>\n                    <Rectangle Width=\"12\" Height=\"12\" Fill=\"#2196F3\"/>\n                </MenuItem.Icon>\n            </MenuItem>\n            <Separator/>\n            <MenuItem Header=\"삭제\"/>\n        </ContextMenu>\n    </Button.ContextMenu>\n</Button>" },
            { "3_3", "<Border Width=\"160\" Height=\"90\" Background=\"#CFD8DC\">\n    <Border.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"확대\"/>\n            <MenuItem Header=\"축소\"/>\n        </ContextMenu>\n    </Border.ContextMenu>\n</Border>" },
            { "4_3", "private void Menu_Click(object sender, RoutedEventArgs e)\n{\n    MenuItem item = sender as MenuItem;\n    MessageBox.Show(item.Header.ToString());\n}" },
            { "5_3", "<Border Background=\"#ECEFF1\" Padding=\"24\">\n    <TextBlock Text=\"삭제된 파일\"/>\n    <Border.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"복원\"/>\n            <Separator/>\n            <MenuItem Header=\"완전 삭제\" Foreground=\"Red\"/>\n        </ContextMenu>\n    </Border.ContextMenu>\n</Border>" },
            { "1_1", "<Button Content=\"마우스 오른쪽 클릭\" Padding=\"20,10\">\n    <Button.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"복사\"/>\n            <MenuItem Header=\"붙여넣기\"/>\n        </ContextMenu>\n    </Button.ContextMenu>\n</Button>" },
            { "1_2", "<TextBox Text=\"여기서 오른쪽 클릭\" Width=\"200\">\n    <TextBox.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"잘라내기\"/>\n            <MenuItem Header=\"삭제\"/>\n        </ContextMenu>\n    </TextBox.ContextMenu>\n</TextBox>" },
            { "2_1", "<Border Background=\"#EEEEEE\" Padding=\"30\">\n    <TextBlock Text=\"오른쪽 클릭\"/>\n    <Border.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"정렬\">\n                <MenuItem Header=\"이름순\"/>\n                <MenuItem Header=\"날짜순\"/>\n            </MenuItem>\n        </ContextMenu>\n    </Border.ContextMenu>\n</Border>" },
            { "2_2", "<Button Content=\"오른쪽 클릭\" Padding=\"20,10\">\n    <Button.ContextMenu>\n        <ContextMenu Background=\"#FFFDE7\">\n            <MenuItem Header=\"기본 동작\" FontWeight=\"Bold\"/>\n            <MenuItem Header=\"그 외\"/>\n        </ContextMenu>\n    </Button.ContextMenu>\n</Button>" },
            { "3_1", "<ListBox Width=\"180\">\n    <ListBoxItem Content=\"첫째 항목\"/>\n    <ListBoxItem Content=\"둘째 항목\"/>\n    <ListBox.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"삭제\"/>\n        </ContextMenu>\n    </ListBox.ContextMenu>\n</ListBox>" },
            { "3_2", "<Border Background=\"#E3F2FD\" Padding=\"30\">\n    <TextBlock Text=\"오른쪽 클릭\"/>\n    <Border.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"즐겨찾기\" IsCheckable=\"True\"/>\n        </ContextMenu>\n    </Border.ContextMenu>\n</Border>" },
            { "4_1", "private void Build_Click(object sender, RoutedEventArgs e)\n{\n    ContextMenu menu = new ContextMenu();\n    menu.Items.Add(new MenuItem { Header = \"새 항목\" });\n    myButton.ContextMenu = menu;\n}" },
            { "4_2", "private void Target_ContextMenuOpening(object sender, ContextMenuEventArgs e)\n{\n    myButton.ContextMenu.Items.Clear();\n    myButton.ContextMenu.Items.Add(new MenuItem { Header = \"지금 만든 항목\" });\n}" },
            { "5_1", "<Border Background=\"#F5F5F5\" Padding=\"30\">\n    <TextBlock Text=\"파일\"/>\n    <Border.ContextMenu>\n        <ContextMenu>\n            <MenuItem Header=\"열기\"/>\n            <Separator/>\n            <MenuItem Header=\"복사\"/>\n            <MenuItem Header=\"잘라내기\"/>\n            <Separator/>\n            <MenuItem Header=\"삭제\"/>\n        </ContextMenu>\n    </Border.ContextMenu>\n</Border>" },
            { "5_2", "private void Delete_Click(object sender, RoutedEventArgs e)\n{\n    if (list.SelectedItem != null)\n    {\n        list.Items.Remove(list.SelectedItem);\n    }\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "1_4", new[] { "MessageBox.Show" } },
            { "4_4", new[] { "ContextMenu", "PlacementTarget", "IsOpen" } },
            { "4_3", new[] { "as MenuItem", "Header", "MessageBox.Show" } },
            { "4_1", new[] { "new ContextMenu", "Items.Add", "MenuItem", "ContextMenu =" } },
            { "4_2", new[] { "Items.Clear", "Items.Add", "MenuItem" } },
            { "5_2", new[] { "SelectedItem", "Items.Remove" } },
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
