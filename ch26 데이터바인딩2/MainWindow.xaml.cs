using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch26_데이터바인딩2
{
    public partial class MainWindow : Window
    {
        // Window DataContext용 속성
        public string WindowTitle => "데이터바인딩 튜토리얼";
        public string WindowSize => $"{Width} x {Height}";

        // 일반 Person (INotifyPropertyChanged 미구현)
        private Person normalPerson = new Person { 이름 = "일반 사람", 나이 = 20 };

        // NotifyPerson (INotifyPropertyChanged 구현)
        private NotifyPerson notifyPerson = new NotifyPerson { Name = "알림 사람", Age = 20 };

        // 카운터
        private Counter counter = new Counter { Count = 0 };

        // 프로필 카드용
        private NotifyPerson profilePerson = new NotifyPerson { Name = "홍길동", Age = 25, Job = "개발자" };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // 탭 1: 객체를 DataContext로 설정
            Person person1 = new Person { 이름 = "홍길동", 나이 = 100 };
            personBorder1.DataContext = person1;

            // 탭 2: DataContext 상속 시각화
            Person personA = new Person { 이름 = "홍길동", 나이 = 30 };
            wpDemo.DataContext = personA;

            Person personB = new Person { 이름 = "임꺽정", 나이 = 40 };
            stpDemo.DataContext = personB;

            // 동적 DataContext 변경용
            dynamicContextBorder.DataContext = new Person { 이름 = "김철수", 나이 = 25 };

            // 탭 3: INotifyPropertyChanged 비교
            normalPersonBorder.DataContext = normalPerson;
            notifyPersonBorder.DataContext = notifyPerson;

            // TwoWay 바인딩 데모
            twoWayDisplayBorder.DataContext = notifyPerson;

            // 탭 4: 바인딩 경로
            // 중첩 속성
            var company = new Company
            {
                CompanyName = "ABC 테크",
                CEO = new NotifyPerson { Name = "박사장", Age = 55 },
                EmployeeCount = 150
            };
            nestedBorder.DataContext = company;

            // 인덱서
            var team = new Team
            {
                Members = new System.Collections.Generic.List<NotifyPerson>
                {
                    new NotifyPerson { Name = "김팀원" },
                    new NotifyPerson { Name = "이팀원" },
                    new NotifyPerson { Name = "박팀원" }
                }
            };
            indexerBorder.DataContext = team;

            // 빈 Path
            emptyPathBorder.DataContext = "안녕하세요";

            // 탭 5: 실용 예제
            profileCard.DataContext = profilePerson;
            counterBorder.DataContext = counter;
        }

        // 탭 2: 동적 DataContext 변경
        private void SetPersonA_Click(object sender, RoutedEventArgs e)
        {
            dynamicContextBorder.DataContext = new Person { 이름 = "김철수", 나이 = 25 };
        }

        private void SetPersonB_Click(object sender, RoutedEventArgs e)
        {
            dynamicContextBorder.DataContext = new Person { 이름 = "이영희", 나이 = 30 };
        }

        private void SetPersonC_Click(object sender, RoutedEventArgs e)
        {
            dynamicContextBorder.DataContext = new Person { 이름 = "박민수", 나이 = 35 };
        }

        // 탭 3: INotifyPropertyChanged 비교
        private void IncrementNormalAge_Click(object sender, RoutedEventArgs e)
        {
            normalPerson.나이++;
            // UI가 업데이트되지 않음 (INotifyPropertyChanged 미구현)
        }

        private void IncrementNotifyAge_Click(object sender, RoutedEventArgs e)
        {
            notifyPerson.Age++;
            // UI가 자동으로 업데이트됨 (INotifyPropertyChanged 구현)
        }

        // 탭 5: 카운터
        private void IncrementCount_Click(object sender, RoutedEventArgs e)
        {
            counter.Count++;
        }

        private void DecrementCount_Click(object sender, RoutedEventArgs e)
        {
            counter.Count--;
        }

        private void ResetCount_Click(object sender, RoutedEventArgs e)
        {
            counter.Count = 0;
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_1", "<StackPanel>\n    <TextBlock Text=\"{Binding Name}\" FontSize=\"16\"/>\n</StackPanel>" },
            { "1_2", "public MainWindow()\n{\n    InitializeComponent();\n    this.DataContext = this;\n}" },
            { "2_1", "<StackPanel>\n    <TextBlock Text=\"{Binding Name}\"/>\n    <TextBlock Text=\"{Binding Age}\"/>\n</StackPanel>" },
            { "2_2", "<StackPanel>\n    <TextBlock Text=\"{Binding Name}\"/>\n    <Border BorderBrush=\"Gray\" BorderThickness=\"1\" Padding=\"8\" DataContext=\"{Binding Address}\">\n        <TextBlock Text=\"{Binding City}\"/>\n    </Border>\n</StackPanel>" },
            { "3_1", "public class Person : INotifyPropertyChanged\n{\n    public event PropertyChangedEventHandler? PropertyChanged;\n\n    protected void OnPropertyChanged(string name)\n    {\n        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));\n    }\n}" },
            { "3_2", "private string _name = \"\";\npublic string Name\n{\n    get { return _name; }\n    set\n    {\n        _name = value;\n        OnPropertyChanged(\"Name\");\n    }\n}" },
            { "4_1", "<TextBlock Text=\"{Binding Address.City}\" FontSize=\"14\"/>" },
            { "4_2", "<TextBlock Text=\"{Binding Items[0]}\" FontSize=\"14\"/>" },
            { "5_1", "<Border BorderBrush=\"#CCCCCC\" BorderThickness=\"1\" Padding=\"12\" Width=\"240\">\n    <StackPanel>\n        <TextBlock Text=\"{Binding Name}\" FontSize=\"16\" FontWeight=\"Bold\"/>\n        <TextBlock Text=\"{Binding Age, StringFormat='나이: {0}세'}\" Margin=\"0,4,0,0\"/>\n    </StackPanel>\n</Border>" },
            { "5_2", "protected void OnPropertyChanged(string name)\n{\n    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "1_2", new[] { "DataContext", "this" } },
            { "3_1", new[] { "PropertyChangedEventHandler", "PropertyChanged", "OnPropertyChanged", "PropertyChangedEventArgs" } },
            { "3_2", new[] { "_name = value", "OnPropertyChanged" } },
            { "5_2", new[] { "PropertyChanged?.Invoke", "PropertyChangedEventArgs" } },
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
