using System.Windows;

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
    }
}
