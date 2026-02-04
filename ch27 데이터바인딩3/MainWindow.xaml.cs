using System.Collections.Generic;
using System.Windows;

namespace ch27_데이터바인딩3
{
    public class Person
    {
        public string 이름 { get; set; }
        public int 나이 { get; set; }
        public int 별점 { get; set; }
    }

    public partial class MainWindow : Window
    {
        List<Person> person = new List<Person>();

        public MainWindow()
        {
            InitializeComponent();

            // Person 데이터 추가
            person.Add(new Person() { 이름 = "홍길동", 나이 = 100, 별점 = 95 });
            person.Add(new Person() { 이름 = "임꺽정", 나이 = 90, 별점 = 89 });
            person.Add(new Person() { 이름 = "타요", 나이 = 10, 별점 = 85 });
            person.Add(new Person() { 이름 = "뽀로로", 나이 = 5, 별점 = 80 });
            person.Add(new Person() { 이름 = "타요", 나이 = 7, 별점 = 90 });

            // ItemsSource 바인딩
            lb.ItemsSource = person;
            lv.ItemsSource = person;
        }
    }
}