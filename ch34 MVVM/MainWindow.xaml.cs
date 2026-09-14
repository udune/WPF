using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch34_MVVM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "2_4", "public class PersonModel : INotifyPropertyChanged\n{\n    public event PropertyChangedEventHandler? PropertyChanged;\n\n    private void OnPropertyChanged(string name)\n    {\n        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));\n    }\n\n    private string name = string.Empty;\n    public string Name\n    {\n        get => name;\n        set { name = value; OnPropertyChanged(\"Name\"); }\n    }\n}" },
            { "3_4", "private void Message(string? txt)\n{\n    MessageBox.Show(txt);\n}\n\nprivate bool CheckMessage(string? txt)\n{\n    return !string.IsNullOrEmpty(txt);\n}" },
            { "3_5", "public class PersonViewModel\n{\n    public PersonCommand PersonCommand { get; set; }\n    public List<PersonModel> PersonList { get; set; }\n\n    public PersonViewModel()\n    {\n        PersonList = new List<PersonModel>\n        {\n            new PersonModel { Name = \"홍길동\", Age = 100 },\n            new PersonModel { Name = \"임꺽정\", Age = 90 }\n        };\n\n        PersonCommand = new PersonCommand(Message, CheckMessage);\n    }\n\n    private void Message(string? txt) => MessageBox.Show(txt);\n    private bool CheckMessage(string? txt) => txt?.Length > 0;\n}" },
            { "4_4", "public void Execute(object? parameter)\n{\n    execute.Invoke(parameter as string);\n}" },
            { "4_5", "public class RelayCommand : ICommand\n{\n    private readonly Action<object?> _execute;\n    private readonly Predicate<object?>? _canExecute;\n\n    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)\n    {\n        _execute = execute;\n        _canExecute = canExecute;\n    }\n\n    public event EventHandler? CanExecuteChanged\n    {\n        add => CommandManager.RequerySuggested += value;\n        remove => CommandManager.RequerySuggested -= value;\n    }\n\n    public bool CanExecute(object? parameter)\n        => _canExecute == null || _canExecute(parameter);\n\n    public void Execute(object? parameter) => _execute(parameter);\n}" },
            { "1_3", "// View 와 ViewModel 의 연결\n// DataContext 에 ViewModel 을 넣고 View 는 Binding 으로만 접근한다\n// 따라서 View 의 code-behind 에는 화면 로직이 남지 않는다" },
            { "2_3", "public class PersonModel\n{\n    public string Name { get; set; } = \"\";\n    public int Age { get; set; }\n    public string Display { get { return Name + \" (\" + Age + \")\"; } }\n}" },
            { "3_3", "private PersonModel? _selected;\npublic PersonModel? Selected\n{\n    get { return _selected; }\n    set\n    {\n        _selected = value;\n        OnPropertyChanged(\"Selected\");\n    }\n}" },
            { "4_3", "public void RaiseCanExecuteChanged()\n{\n    CanExecuteChanged?.Invoke(this, EventArgs.Empty);\n}" },
            { "5_3", "<ListView ItemsSource=\"{Binding People}\" SelectedItem=\"{Binding Selected, Mode=TwoWay}\" Height=\"110\"/>" },
            { "1_1", "// MVVM 역할 정리\n// Model: 순수한 데이터와 업무 규칙\n// View: 화면을 그리는 XAML\n// ViewModel: Model 을 View 가 쓰기 좋은 형태로 노출하고 Command 를 제공" },
            { "1_2", "public MainWindow()\n{\n    InitializeComponent();\n    this.DataContext = new PersonViewModel();\n}" },
            { "2_1", "public class PersonModel\n{\n    public string Name { get; set; } = \"\";\n    public int Age { get; set; }\n}" },
            { "2_2", "public List<PersonModel> People { get; set; }\n\npublic void Load()\n{\n    People = new List<PersonModel>\n    {\n        new PersonModel { Name = \"홍길동\", Age = 30 },\n        new PersonModel { Name = \"임꺽정\", Age = 40 },\n        new PersonModel { Name = \"장길산\", Age = 50 }\n    };\n}" },
            { "3_1", "public class PersonViewModel : INotifyPropertyChanged\n{\n    public event PropertyChangedEventHandler? PropertyChanged;\n\n    protected void OnPropertyChanged(string name)\n    {\n        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));\n    }\n}" },
            { "3_2", "public ObservableCollection<PersonModel> People { get; set; } = new ObservableCollection<PersonModel>();" },
            { "4_1", "public class RelayCommand : ICommand\n{\n    private readonly Action<object?> _execute;\n\n    public RelayCommand(Action<object?> execute)\n    {\n        _execute = execute;\n    }\n\n    public event EventHandler? CanExecuteChanged;\n\n    public bool CanExecute(object? parameter) => true;\n\n    public void Execute(object? parameter) => _execute(parameter);\n}" },
            { "4_2", "public bool CanExecute(object? parameter)\n{\n    return !string.IsNullOrEmpty(parameter as string);\n}" },
            { "5_1", "<StackPanel>\n    <TextBox x:Name=\"input\" Width=\"200\"/>\n    <Button Content=\"실행\" Width=\"100\" Margin=\"0,6,0,0\"\n            Command=\"{Binding PersonCommand}\"\n            CommandParameter=\"{Binding ElementName=input, Path=Text}\"/>\n</StackPanel>" },
            { "5_2", "<ListView ItemsSource=\"{Binding People}\" Height=\"110\">\n    <ListView.View>\n        <GridView>\n            <GridViewColumn Header=\"이름\" Width=\"120\" DisplayMemberBinding=\"{Binding Name}\"/>\n            <GridViewColumn Header=\"나이\" Width=\"80\" DisplayMemberBinding=\"{Binding Age}\"/>\n        </GridView>\n    </ListView.View>\n</ListView>" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "2_4", new[] { "get", "set", "OnPropertyChanged" } },
            { "3_4", new[] { "MessageBox.Show", "return" } },
            { "3_5", new[] { "PersonList", "new PersonModel", "new PersonCommand" } },
            { "4_4", new[] { "execute", "parameter as string" } },
            { "4_5", new[] { "CanExecuteChanged", "CommandManager.RequerySuggested", "CanExecute" } },
            { "1_3", new[] { "DataContext", "Binding" } },
            { "2_3", new[] { "get", "Display", "Name" } },
            { "3_3", new[] { "_selected = value", "OnPropertyChanged" } },
            { "4_3", new[] { "CanExecuteChanged?.Invoke", "EventArgs.Empty" } },
            { "1_1", new[] { "Model", "View", "ViewModel" } },
            { "1_2", new[] { "DataContext", "new PersonViewModel" } },
            { "2_1", new[] { "public string Name", "public int Age" } },
            { "2_2", new[] { "new List<PersonModel>", "new PersonModel", "Name =", "Age =" } },
            { "3_1", new[] { "INotifyPropertyChanged", "PropertyChangedEventHandler", "OnPropertyChanged" } },
            { "3_2", new[] { "ObservableCollection<PersonModel>", "People" } },
            { "4_1", new[] { "ICommand", "CanExecute", "Execute", "CanExecuteChanged" } },
            { "4_2", new[] { "IsNullOrEmpty", "return" } },
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
