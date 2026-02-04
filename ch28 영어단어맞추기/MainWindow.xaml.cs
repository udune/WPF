using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ch28_영어단어맞추기
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string wrongStatus;
        private string selEng;
        private string selKor;
        private string message;
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

        List<char> SelWord = new List<char>();
        List<String> words = new List<String>()
        {
            "boy,소년",
            "school, 학교",
            "fish, 물고기",
            "car, 자동차",
            "book, 책"
        };
        int wrong = 0;
        int maxWrong = 3;
        string compareWord = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            btns.AddRange("abcdefghijklmnopqrstuvwxyz");
            ic.ItemsSource = btns;
            RandomWord();
            ChangeWord(SelEng, SelWord);
            WrongStatus = $"틀린 횟수: {wrong} of {maxWrong}";
        }

        // 선택된 글자를 표시하고 나머지는 *로 표시
        private void ChangeWord(string selEng, List<char> selWord)
        {
            char[] result = selEng.Select(x => (selWord.IndexOf(x) >= 0 ? x : '*')).ToArray();
            SelEng = string.Join(' ', result);
        }

        // 랜덤 단어 선택
        private void RandomWord()
        {
            string[] selChar = words[new Random().Next(0, words.Count)].Split(",");
            SelEng = selChar[0];
            SelKor = selChar[1];
            compareWord = selEng;
        }

        // 새 게임 시작
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            wrong = 0;
            SelWord = new List<char>();
            RandomWord();
            ChangeWord(SelEng, SelWord);
            Message = "알파벳을 선택하여 주세요";
            Status();
            foreach (var a in ic.Items)
            {
                var btn = (UIElement)ic.ItemContainerGenerator.ContainerFromItem(a);
                if (btn != null)
                    btn.IsEnabled = true;
            }
        }

        // 알파벳 버튼 클릭
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var result = btn.Content.ToString();
                CheckWord(result[0]);
            }
        }

        // 선택한 글자 확인
        private void CheckWord(char v)
        {
            if (SelWord.IndexOf(v) == -1)
                SelWord.Add(v);

            if (compareWord.IndexOf(v) >= 0)
            {
                ChangeWord(compareWord, SelWord);
                CheckWin();
            }
            else if (compareWord.IndexOf(v) == -1)
            {
                wrong++;
                Status();
                CheckLost();
            }
        }

        private void Status()
        {
            WrongStatus = $"틀린 횟수: {wrong} of {maxWrong}";
        }

        // 승리 확인
        private void CheckWin()
        {
            if (compareWord == SelEng.Replace(" ", ""))
            {
                Message = "You Win!";
                DisableAllButtons();
            }
        }

        // 패배 확인
        private void CheckLost()
        {
            if (wrong == maxWrong)
            {
                Message = "You Lost!";
                DisableAllButtons();
            }
        }

        // 모든 알파벳 버튼 비활성화
        private void DisableAllButtons()
        {
            foreach (var a in ic.Items)
            {
                var btn = (UIElement)ic.ItemContainerGenerator.ContainerFromItem(a);
                if (btn != null)
                    btn.IsEnabled = false;
            }
        }
    }
}
