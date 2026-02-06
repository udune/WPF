using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ch26_데이터바인딩2
{
    // 일반 Person 클래스 (INotifyPropertyChanged 미구현)
    public class Person
    {
        public string 이름 { get; set; } = string.Empty;
        public int 나이 { get; set; }
    }

    // INotifyPropertyChanged 구현 클래스
    public class NotifyPerson : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private int _age;
        public int Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(); }
        }

        private string _job = string.Empty;
        public string Job
        {
            get => _job;
            set { _job = value; OnPropertyChanged(); }
        }
    }

    // 회사 클래스 (중첩 속성 데모용)
    public class Company
    {
        public string CompanyName { get; set; } = string.Empty;
        public NotifyPerson CEO { get; set; } = new NotifyPerson();
        public int EmployeeCount { get; set; }
    }

    // 팀 클래스 (인덱서 데모용)
    public class Team
    {
        public System.Collections.Generic.List<NotifyPerson> Members { get; set; } = new();
    }

    // 카운터 클래스
    public class Counter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _count;
        public int Count
        {
            get => _count;
            set { _count = value; OnPropertyChanged(); }
        }
    }
}
