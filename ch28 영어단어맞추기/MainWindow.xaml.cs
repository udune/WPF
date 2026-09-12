using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace ch28_영어단어맞추기
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string wrongStatus = "";
        private string selEng = "";
        private string selKor = "";
        private string message = "";
        private Brush messageBackground = Brushes.Gray;
        private Brush messageForeground = Brushes.White;
        private List<char> btns = new List<char>();

        public string WrongStatus
        {
            get => wrongStatus;
            set { wrongStatus = value; OnPropertyChanged(nameof(WrongStatus)); }
        }

        public string SelEng
        {
            get => selEng;
            set { selEng = value; OnPropertyChanged(nameof(SelEng)); }
        }

        public string SelKor
        {
            get => selKor;
            set { selKor = value; OnPropertyChanged(nameof(SelKor)); }
        }

        public string Message
        {
            get => message;
            set { message = value; OnPropertyChanged(nameof(Message)); }
        }

        public Brush MessageBackground
        {
            get => messageBackground;
            set { messageBackground = value; OnPropertyChanged(nameof(MessageBackground)); }
        }

        public Brush MessageForeground
        {
            get => messageForeground;
            set { messageForeground = value; OnPropertyChanged(nameof(MessageForeground)); }
        }

        List<char> SelWord = new List<char>();
        List<string> words = new List<string>()
        {
            "boy,소년",
            "school,학교",
            "fish,물고기",
            "car,자동차",
            "book,책",
            "apple,사과",
            "computer,컴퓨터",
            "happiness,행복",
            "student,학생",
            "teacher,선생님"
        };
        int wrong = 0;
        int maxWrong = 3;
        string compareWord = string.Empty;

        // 데모용 점수
        private int demoScoreValue = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            btns.AddRange("abcdefghijklmnopqrstuvwxyz");
            alphabetButtons.ItemsSource = btns;
            StartNewGame();
        }

        private void StartNewGame()
        {
            wrong = 0;
            SelWord = new List<char>();
            RandomWord();
            ChangeWord(compareWord, SelWord);
            Message = "알파벳을 선택하여 주세요";
            MessageBackground = Brushes.Gray;
            MessageForeground = Brushes.White;
            Status();
        }

        // 선택된 글자를 표시하고 나머지는 *로 표시
        private void ChangeWord(string word, List<char> selWord)
        {
            char[] result = word.Select(x => (selWord.Contains(x) ? x : '*')).ToArray();
            SelEng = string.Join(' ', result);
        }

        // 랜덤 단어 선택
        private void RandomWord()
        {
            string[] selChar = words[new Random().Next(0, words.Count)].Split(",");
            compareWord = selChar[0].Trim();
            SelKor = selChar[1].Trim();
        }

        // 새 게임 시작
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
            EnableAllButtons();
        }

        // 알파벳 버튼 클릭
        private void Alphabet_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var result = btn.Content.ToString();
                if (!string.IsNullOrEmpty(result))
                {
                    CheckWord(result[0]);
                    btn.IsEnabled = false;
                }
            }
        }

        // 선택한 글자 확인
        private void CheckWord(char v)
        {
            if (!SelWord.Contains(v))
                SelWord.Add(v);

            if (compareWord.Contains(v))
            {
                ChangeWord(compareWord, SelWord);
                CheckWin();
            }
            else
            {
                wrong++;
                Status();
                CheckLost();
            }
        }

        private void Status()
        {
            WrongStatus = $"틀린 횟수: {wrong} / {maxWrong}";
        }

        // 승리 확인
        private void CheckWin()
        {
            if (compareWord == SelEng.Replace(" ", ""))
            {
                Message = "🎉 You Win!";
                MessageBackground = Brushes.Green;
                MessageForeground = Brushes.White;
                DisableAllButtons();
            }
        }

        // 패배 확인
        private void CheckLost()
        {
            if (wrong >= maxWrong)
            {
                Message = $"😢 You Lost! 정답: {compareWord}";
                MessageBackground = Brushes.Red;
                MessageForeground = Brushes.White;
                DisableAllButtons();
            }
        }

        // 모든 알파벳 버튼 비활성화
        private void DisableAllButtons()
        {
            foreach (var item in alphabetButtons.Items)
            {
                var container = alphabetButtons.ItemContainerGenerator.ContainerFromItem(item);
                if (container is ContentPresenter presenter)
                {
                    var button = FindVisualChild<Button>(presenter);
                    if (button != null)
                        button.IsEnabled = false;
                }
            }
        }

        // 모든 알파벳 버튼 활성화
        private void EnableAllButtons()
        {
            foreach (var item in alphabetButtons.Items)
            {
                var container = alphabetButtons.ItemContainerGenerator.ContainerFromItem(item);
                if (container is ContentPresenter presenter)
                {
                    var button = FindVisualChild<Button>(presenter);
                    if (button != null)
                        button.IsEnabled = true;
                }
            }
        }

        // 시각적 트리에서 특정 타입의 자식 찾기
        private T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                    return typedChild;

                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        // 데모 이벤트 핸들러들
        private void DemoIncrement_Click(object sender, RoutedEventArgs e)
        {
            demoScoreValue++;
            demoScore.Text = demoScoreValue.ToString();
        }

        private void DemoDecrement_Click(object sender, RoutedEventArgs e)
        {
            if (demoScoreValue > 0)
                demoScoreValue--;
            demoScore.Text = demoScoreValue.ToString();
        }

        private void DemoReset_Click(object sender, RoutedEventArgs e)
        {
            demoScoreValue = 0;
            demoScore.Text = demoScoreValue.ToString();
        }

        private void TestButtonState_Click(object sender, RoutedEventArgs e)
        {
            int enabledCount = 0;
            int disabledCount = 0;

            foreach (var item in alphabetButtons.Items)
            {
                var container = alphabetButtons.ItemContainerGenerator.ContainerFromItem(item);
                if (container is ContentPresenter presenter)
                {
                    var button = FindVisualChild<Button>(presenter);
                    if (button != null)
                    {
                        if (button.IsEnabled)
                            enabledCount++;
                        else
                            disabledCount++;
                    }
                }
            }

            buttonStateText.Text = $"활성화: {enabledCount}개, 비활성화: {disabledCount}개";
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "2_3", "public string Progress\n{\n    get { return _correct + \" / \" + _total; }\n}\n\nprivate void UpdateProgress()\n{\n    OnPropertyChanged(\"Progress\");\n}" },
            { "3_3", "<ItemsControl>\n    <ItemsControl.ItemsPanel>\n        <ItemsPanelTemplate>\n            <UniformGrid Rows=\"2\" Columns=\"2\"/>\n        </ItemsPanelTemplate>\n    </ItemsControl.ItemsPanel>\n</ItemsControl>" },
            { "4_3", "private void UseChance()\n{\n    chances = chances - 1;\n    if (chances <= 0)\n    {\n        GameOver();\n    }\n}" },
            { "5_3", "private void ResetInput()\n{\n    answerBox.Text = \"\";\n    answerBox.Focus();\n}" },
            { "2_1", "private int _score;\npublic int Score\n{\n    get { return _score; }\n    set\n    {\n        _score = value;\n        OnPropertyChanged(\"Score\");\n    }\n}" },
            { "2_2", "public event PropertyChangedEventHandler? PropertyChanged;\n\nprotected void OnPropertyChanged(string name)\n{\n    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));\n}" },
            { "3_1", "<ItemsControl>\n    <ItemsControl.ItemsPanel>\n        <ItemsPanelTemplate>\n            <WrapPanel/>\n        </ItemsPanelTemplate>\n    </ItemsControl.ItemsPanel>\n</ItemsControl>" },
            { "3_2", "<ItemsControl>\n    <ItemsControl.ItemTemplate>\n        <DataTemplate>\n            <Button Content=\"{Binding}\" Width=\"40\" Margin=\"3\"/>\n        </DataTemplate>\n    </ItemsControl.ItemTemplate>\n</ItemsControl>" },
            { "4_1", "private void Check(string input)\n{\n    if (input.ToLower() == answer.ToLower())\n    {\n        Score = Score + 10;\n    }\n}" },
            { "4_2", "private void Next()\n{\n    index = (index + 1) % words.Count;\n}" },
            { "5_1", "private void OnCorrect()\n{\n    nextButton.IsEnabled = true;\n    checkButton.IsEnabled = false;\n}" },
            { "5_2", "private void Reset()\n{\n    Score = 0;\n    index = 0;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "2_3", new[] { "OnPropertyChanged", "Progress" } },
            { "4_3", new[] { "chances", "if (chances <= 0)", "GameOver" } },
            { "5_3", new[] { "answerBox.Text", "Focus()" } },
            { "2_1", new[] { "_score = value", "OnPropertyChanged" } },
            { "2_2", new[] { "PropertyChanged?.Invoke", "PropertyChangedEventArgs" } },
            { "4_1", new[] { "ToLower", "==", "Score" } },
            { "4_2", new[] { "index", "%", "Count" } },
            { "5_1", new[] { "IsEnabled = true", "IsEnabled = false" } },
            { "5_2", new[] { "Score = 0", "index = 0" } },
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
