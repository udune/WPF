using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

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
    }
}
