using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch27_데이터바인딩3
{
    public class Person
    {
        public string 이름 { get; set; } = string.Empty;
        public int 나이 { get; set; }
        public int 별점 { get; set; }
    }

    public partial class MainWindow : Window
    {
        // 샘플 데이터
        private List<Person> samplePeople = new List<Person>
        {
            new Person { 이름 = "홍길동", 나이 = 30, 별점 = 95 },
            new Person { 이름 = "김영희", 나이 = 25, 별점 = 88 },
            new Person { 이름 = "이철수", 나이 = 35, 별점 = 72 },
            new Person { 이름 = "박민수", 나이 = 28, 별점 = 90 },
            new Person { 이름 = "정수진", 나이 = 22, 별점 = 85 }
        };

        // ObservableCollection
        private ObservableCollection<Person> observablePeople = new ObservableCollection<Person>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeTab1();
            InitializeTab2();
            InitializeTab3();
            InitializeTab4();
            InitializeTab5();
        }

        // 탭 1: ItemsSource 기본
        private void InitializeTab1()
        {
            // 문자열 리스트
            var fruits = new List<string> { "사과", "바나나", "오렌지", "포도", "딸기", "키위", "망고" };
            stringListBox.ItemsSource = fruits;

            // 객체 리스트
            personListBox.ItemsSource = samplePeople;

            // SelectedItem 데모
            selectDemoListBox.ItemsSource = samplePeople;
            selectDemoListBox.SelectedValuePath = "나이";
            selectDemoListBox.SelectionChanged += SelectDemoListBox_SelectionChanged;
        }

        private void SelectDemoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectDemoListBox.SelectedItem is Person person)
            {
                selectedItemText.Text = $"{person.이름} ({person.나이}세)";
                selectedValueText.Text = selectDemoListBox.SelectedValue?.ToString() ?? "없음";
            }
        }

        // 탭 2: DataTemplate
        private void InitializeTab2()
        {
            basicTemplateListBox.ItemsSource = samplePeople;
            cardTemplateListBox.ItemsSource = samplePeople;
        }

        // 탭 3: ListView / GridView
        private void InitializeTab3()
        {
            basicGridView.ItemsSource = samplePeople;
            customGridView.ItemsSource = samplePeople;
        }

        // 탭 4: ComboBox
        private void InitializeTab4()
        {
            // 문자열 ComboBox
            stringComboBox.ItemsSource = new List<string> { "서울", "부산", "대구", "인천", "광주" };

            // 객체 ComboBox
            personComboBox.ItemsSource = samplePeople;

            // 선택 데모 ComboBox
            selectionComboBox.ItemsSource = samplePeople;

            // ItemTemplate ComboBox
            templateComboBox.ItemsSource = samplePeople;

            // 편집 가능한 ComboBox
            editableComboBox.ItemsSource = new List<string> { "옵션 1", "옵션 2", "옵션 3" };
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectionComboBox.SelectedItem is Person person)
            {
                comboSelectedItem.Text = $"SelectedItem: {person.이름}";
                comboSelectedValue.Text = $"SelectedValue (나이): {selectionComboBox.SelectedValue}";
            }
        }

        // 탭 5: ObservableCollection
        private void InitializeTab5()
        {
            // 초기 데이터 추가
            observablePeople.Add(new Person { 이름 = "홍길동", 나이 = 30, 별점 = 95 });
            observablePeople.Add(new Person { 이름 = "김영희", 나이 = 25, 별점 = 88 });

            observableListBox.ItemsSource = observablePeople;

            // CollectionChanged 이벤트 구독
            observablePeople.CollectionChanged += ObservablePeople_CollectionChanged;

            UpdateItemCount();
            AddLog("초기화 완료");
        }

        private void ObservablePeople_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is Person addedPerson)
                        AddLog($"[추가] {addedPerson.이름}");
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems?[0] is Person removedPerson)
                        AddLog($"[삭제] {removedPerson.이름}");
                    break;
                case NotifyCollectionChangedAction.Reset:
                    AddLog("[초기화] 모든 항목 삭제됨");
                    break;
            }
            UpdateItemCount();
        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            var name = string.IsNullOrWhiteSpace(newNameTextBox.Text) ? "새 사람" : newNameTextBox.Text;
            int.TryParse(newAgeTextBox.Text, out int age);

            observablePeople.Add(new Person { 이름 = name, 나이 = age, 별점 = 50 });

            newNameTextBox.Clear();
            newAgeTextBox.Clear();
        }

        private void RemovePerson_Click(object sender, RoutedEventArgs e)
        {
            if (observableListBox.SelectedItem is Person person)
            {
                observablePeople.Remove(person);
            }
            else
            {
                MessageBox.Show("삭제할 항목을 선택하세요.", "알림");
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            observablePeople.Clear();
        }

        private void UpdateItemCount()
        {
            itemCountText.Text = $"항목 수: {observablePeople.Count}";
        }

        private void AddLog(string message)
        {
            var time = System.DateTime.Now.ToString("HH:mm:ss");
            logTextBlock.Text = $"[{time}] {message}\n" + logTextBlock.Text;
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_4", "private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)\n{\n    var person = listBox.SelectedItem as Person;\n    var age = listBox.SelectedValue;\n}" },
            { "4_4", "<ComboBox Width=\"180\" IsEditable=\"True\" Text=\"직접 입력해 보세요\">\n    <ComboBoxItem Content=\"사과\"/>\n    <ComboBoxItem Content=\"바나나\"/>\n</ComboBox>" },
            { "1_3", "<ListBox Width=\"200\" Height=\"90\" SelectedIndex=\"2\">\n    <ListBoxItem Content=\"하나\"/>\n    <ListBoxItem Content=\"둘\"/>\n    <ListBoxItem Content=\"셋\"/>\n</ListBox>" },
            { "2_3", "<ListBox Width=\"240\" Height=\"110\">\n    <ListBox.ItemTemplate>\n        <DataTemplate>\n            <StackPanel Margin=\"0,3,0,3\">\n                <TextBlock Text=\"{Binding Name}\" FontWeight=\"Bold\"/>\n                <TextBlock Text=\"{Binding Description}\" FontSize=\"11\" Foreground=\"Gray\"/>\n            </StackPanel>\n        </DataTemplate>\n    </ListBox.ItemTemplate>\n</ListBox>" },
            { "3_3", "<ListView Width=\"320\" Height=\"110\">\n    <ListView.View>\n        <GridView>\n            <GridViewColumn Header=\"이름\" Width=\"120\" DisplayMemberBinding=\"{Binding Name}\"/>\n            <GridViewColumn Header=\"나이\" Width=\"60\" DisplayMemberBinding=\"{Binding Age}\"/>\n            <GridViewColumn Header=\"도시\" Width=\"100\" DisplayMemberBinding=\"{Binding City}\"/>\n        </GridView>\n    </ListView.View>\n</ListView>" },
            { "4_3", "<ComboBox Width=\"180\" IsEditable=\"True\">\n    <ComboBoxItem Content=\"서울\"/>\n    <ComboBoxItem Content=\"부산\"/>\n</ComboBox>" },
            { "5_3", "private void Clear_Click(object sender, RoutedEventArgs e)\n{\n    items.Clear();\n}" },
            { "1_1", "<ListBox Width=\"200\" Height=\"90\">\n    <ListBoxItem Content=\"사과\"/>\n    <ListBoxItem Content=\"바나나\"/>\n    <ListBoxItem Content=\"포도\"/>\n</ListBox>" },
            { "1_2", "private void Load_Click(object sender, RoutedEventArgs e)\n{\n    List<string> items = new List<string> { \"사과\", \"바나나\", \"포도\" };\n    list.ItemsSource = items;\n}" },
            { "2_1", "<ListBox Width=\"220\" Height=\"90\">\n    <ListBox.ItemTemplate>\n        <DataTemplate>\n            <TextBlock Text=\"{Binding Name}\" FontWeight=\"Bold\"/>\n        </DataTemplate>\n    </ListBox.ItemTemplate>\n</ListBox>" },
            { "2_2", "<ListBox Width=\"220\" Height=\"90\">\n    <ListBox.ItemTemplate>\n        <DataTemplate>\n            <StackPanel Orientation=\"Horizontal\">\n                <Rectangle Width=\"12\" Height=\"12\" Fill=\"SteelBlue\" Margin=\"0,0,6,0\"/>\n                <TextBlock Text=\"{Binding Name}\"/>\n            </StackPanel>\n        </DataTemplate>\n    </ListBox.ItemTemplate>\n</ListBox>" },
            { "3_1", "<ListView Width=\"260\" Height=\"100\">\n    <ListView.View>\n        <GridView>\n            <GridViewColumn Header=\"이름\" Width=\"120\" DisplayMemberBinding=\"{Binding Name}\"/>\n            <GridViewColumn Header=\"나이\" Width=\"80\" DisplayMemberBinding=\"{Binding Age}\"/>\n        </GridView>\n    </ListView.View>\n</ListView>" },
            { "3_2", "<ListView Width=\"260\" Height=\"100\">\n    <ListView.View>\n        <GridView>\n            <GridViewColumn Header=\"이름\" Width=\"200\">\n                <GridViewColumn.CellTemplate>\n                    <DataTemplate>\n                        <TextBlock Text=\"{Binding Name}\" Foreground=\"Blue\"/>\n                    </DataTemplate>\n                </GridViewColumn.CellTemplate>\n            </GridViewColumn>\n        </GridView>\n    </ListView.View>\n</ListView>" },
            { "4_1", "<ComboBox Width=\"160\" SelectedIndex=\"1\">\n    <ComboBoxItem Content=\"서울\"/>\n    <ComboBoxItem Content=\"부산\"/>\n    <ComboBoxItem Content=\"대구\"/>\n</ComboBox>" },
            { "4_2", "<ComboBox Width=\"160\" DisplayMemberPath=\"Name\" SelectedValuePath=\"Id\"/>" },
            { "5_1", "private void Add_Click(object sender, RoutedEventArgs e)\n{\n    items.Add(inputBox.Text);\n}" },
            { "5_2", "private void Remove_Click(object sender, RoutedEventArgs e)\n{\n    if (list.SelectedItem != null)\n    {\n        items.Remove((string)list.SelectedItem);\n    }\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "1_4", new[] { "SelectedItem", "SelectedValue" } },
            { "5_3", new[] { "items.Clear()" } },
            { "1_2", new[] { "List<string>", "ItemsSource" } },
            { "5_1", new[] { "items.Add" } },
            { "5_2", new[] { "SelectedItem", "items.Remove" } },
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
