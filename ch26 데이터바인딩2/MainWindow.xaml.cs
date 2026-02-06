using System.Windows;

namespace ch26_데이터바인딩2
{
    public partial class MainWindow : Window
    {
        public string 이름 { get; set; }
        public int 나이 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            이름 = "타요";
            나이 = 10;
            DataContext = this;
            Person person = new Person() { 이름 = "홍길동", 나이 = 100 };
            wp.DataContext = person;

            Person person2 = new Person() { 이름 = "임꺽정", 나이 = 90 };
            stp.DataContext = person2;
        }
    }
}
