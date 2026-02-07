using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
    }
}
