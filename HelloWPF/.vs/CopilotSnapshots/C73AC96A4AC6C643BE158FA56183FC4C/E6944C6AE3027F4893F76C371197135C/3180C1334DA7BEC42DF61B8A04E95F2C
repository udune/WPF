using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ch26_데이터바인딩2
{
    /// <summary>
    /// Person 클래스 - 데이터 바인딩을 위한 간단한 모델 클래스
    /// 이름과 나이 정보를 저장하는 데이터 객체입니다.
    /// WrapPanel과 StackPanel의 DataContext로 사용됩니다.
    /// </summary>
    internal class Person
    {
        // ========== 속성 정의 ==========
        
        public string 이름 { get; set; }
        /*
         * 이름 속성 (자동 속성):
         * - 타입: string (문자열)
         * - 접근자: public get, public set
         * 
         * 역할:
         * - Person 인스턴스의 이름 저장
         * - XAML에서 {Binding 이름}으로 바인딩
         * 
         * 자동 속성:
         * - 컴파일러가 자동으로 백킹 필드 생성
         * - private string _이름; 을 내부적으로 생성
         * 
         * 사용 예:
         * Person person = new Person { 이름 = "홍길동" };
         * string name = person.이름; // "홍길동"
         */
        
        public int 나이 { get; set; }
        /*
         * 나이 속성 (자동 속성):
         * - 타입: int (정수)
         * - 접근자: public get, public set
         * 
         * 역할:
         * - Person 인스턴스의 나이 저장
         * - XAML에서 {Binding 나이}로 바인딩
         * 
         * 자동 형변환:
         * - WPF가 자동으로 int를 문자열로 변환하여 표시
         * - Label.Content는 object 타입이므로 모든 타입 허용
         * 
         * 사용 예:
         * Person person = new Person { 나이 = 100 };
         * int age = person.나이; // 100
         */
        
        /*
         * ========== Person 클래스의 한계와 개선점 ==========
         * 
         * 현재 구조:
         * - 단순한 데이터 저장 객체 (DTO: Data Transfer Object)
         * - 속성 변경 알림 없음
         * - 초기값 설정 후 UI 업데이트 안 됨
         * 
         * 문제점:
         * 1. 속성 변경 시 UI 자동 업데이트 안 됨
         *    person.이름 = "김철수"; // UI에 반영 안 됨
         * 
         * 2. 양방향 바인딩 불가
         *    <TextBox Text="{Binding 이름, Mode=TwoWay}"/>
         *    // TextBox 수정 시 Person 업데이트는 되지만
         *    // Person 변경 시 TextBox 업데이트 안 됨
         * 
         * ========== INotifyPropertyChanged 구현 (권장) ==========
         * 
         * using System.ComponentModel;
         * 
         * public class Person : INotifyPropertyChanged
         * {
         *     // 백킹 필드
         *     private string _이름;
         *     private int _나이;
         *     
         *     // 속성 (변경 알림 포함)
         *     public string 이름
         *     {
         *         get => _이름;
         *         set
         *         {
         *             if (_이름 != value)
         *             {
         *                 _이름 = value;
         *                 OnPropertyChanged(nameof(이름)); // 변경 알림
         *             }
         *         }
         *     }
         *     
         *     public int 나이
         *     {
         *         get => _나이;
         *         set
         *         {
         *             if (_나이 != value)
         *             {
         *                 _나이 = value;
         *                 OnPropertyChanged(nameof(나이)); // 변경 알림
         *             }
         *         }
         *     }
         *     
         *     // INotifyPropertyChanged 구현
         *     public event PropertyChangedEventHandler PropertyChanged;
         *     
         *     protected virtual void OnPropertyChanged(string propertyName)
         *     {
         *         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         *     }
         * }
         * 
         * 장점:
         * - 속성 변경 시 UI 자동 업데이트
         * - 양방향 바인딩 완전 지원
         * - MVVM 패턴과 완벽한 호환
         * 
         * 사용:
         * Person person = new Person { 이름 = "홍길동", 나이 = 100 };
         * wp.DataContext = person;
         * 
         * // 코드에서 변경 → UI 자동 업데이트
         * person.이름 = "김철수"; // UI에 즉시 반영
         * person.나이 = 80;       // UI에 즉시 반영
         * 
         * ========== 초기값 설정 방법 ==========
         * 
         * 1. 생성자에서:
         * public class Person
         * {
         *     public string 이름 { get; set; }
         *     public int 나이 { get; set; }
         *     
         *     public Person()
         *     {
         *         이름 = "홍길동";
         *         나이 = 100;
         *     }
         * }
         * 
         * 사용:
         * Person person = new Person(); // 기본값 "홍길동", 100
         * 
         * 2. 매개변수 생성자:
         * public class Person
         * {
         *     public string 이름 { get; set; }
         *     public int 나이 { get; set; }
         *     
         *     public Person(string name, int age)
         *     {
         *         이름 = name;
         *         나이 = age;
         *     }
         * }
         * 
         * 사용:
         * Person person = new Person("홍길동", 100);
         * 
         * 3. 객체 초기화 구문 (현재 사용):
         * Person person = new Person() { 이름 = "홍길동", 나이 = 100 };
         * 
         * 4. 속성 초기화 (C# 6.0+):
         * public class Person
         * {
         *     public string 이름 { get; set; } = "홍길동";
         *     public int 나이 { get; set; } = 100;
         * }
         * 
         * ========== 추가 속성 예제 ==========
         * 
         * public class Person
         * {
         *     // 기본 정보
         *     public string 이름 { get; set; }
         *     public int 나이 { get; set; }
         *     public string 성별 { get; set; }
         *     public DateTime 생년월일 { get; set; }
         *     
         *     // 연락처
         *     public string 전화번호 { get; set; }
         *     public string 이메일 { get; set; }
         *     public string 주소 { get; set; }
         *     
         *     // 계산된 속성 (읽기 전용)
         *     public string 전체이름 => $"{성} {이름}";
         *     public int 출생년도 => DateTime.Now.Year - 나이;
         *     
         *     // 검증 속성
         *     public bool IsAdult => 나이 >= 18;
         *     public bool HasEmail => !string.IsNullOrEmpty(이메일);
         * }
         * 
         * ========== 유효성 검사 추가 ==========
         * 
         * using System.ComponentModel.DataAnnotations;
         * 
         * public class Person
         * {
         *     [Required(ErrorMessage = "이름은 필수입니다.")]
         *     [StringLength(50, ErrorMessage = "이름은 50자 이내여야 합니다.")]
         *     public string 이름 { get; set; }
         *     
         *     [Range(0, 150, ErrorMessage = "나이는 0~150 사이여야 합니다.")]
         *     public int 나이 { get; set; }
         *     
         *     [EmailAddress(ErrorMessage = "올바른 이메일 형식이 아닙니다.")]
         *     public string 이메일 { get; set; }
         * }
         * 
         * ========== 비교 및 동등성 ==========
         * 
         * public class Person : IEquatable<Person>
         * {
         *     public string 이름 { get; set; }
         *     public int 나이 { get; set; }
         *     
         *     public bool Equals(Person other)
         *     {
         *         if (other == null) return false;
         *         return 이름 == other.이름 && 나이 == other.나이;
         *     }
         *     
         *     public override bool Equals(object obj)
         *     {
         *         return Equals(obj as Person);
         *     }
         *     
         *     public override int GetHashCode()
         *     {
         *         return HashCode.Combine(이름, 나이);
         *     }
         * }
         * 
         * ========== ToString 재정의 ==========
         * 
         * public class Person
         * {
         *     public string 이름 { get; set; }
         *     public int 나이 { get; set; }
         *     
         *     public override string ToString()
         *     {
         *         return $"{이름} ({나이}세)";
         *     }
         * }
         * 
         * 사용:
         * Person person = new Person { 이름 = "홍길동", 나이 = 100 };
         * Console.WriteLine(person); // "홍길동 (100세)"
         * 
         * ========== 컬렉션에서 사용 ==========
         * 
         * using System.Collections.ObjectModel;
         * 
         * public class MainViewModel
         * {
         *     public ObservableCollection<Person> People { get; set; }
         *     
         *     public MainViewModel()
         *     {
         *         People = new ObservableCollection<Person>
         *         {
         *             new Person { 이름 = "홍길동", 나이 = 100 },
         *             new Person { 이름 = "임꺽정", 나이 = 90 },
         *             new Person { 이름 = "김유신", 나이 = 80 }
         *         };
         *     }
         * }
         * 
         * XAML:
         * <ListBox ItemsSource="{Binding People}">
         *     <ListBox.ItemTemplate>
         *         <DataTemplate>
         *             <StackPanel>
         *                 <TextBlock Text="{Binding 이름}" FontWeight="Bold"/>
         *                 <TextBlock Text="{Binding 나이}"/>
         *             </StackPanel>
         *         </DataTemplate>
         *     </ListBox.ItemTemplate>
         * </ListBox>
         * 
         * ========== 실전 활용 패턴 ==========
         * 
         * 1. DTO (Data Transfer Object):
         *    - 현재 구조 (단순 데이터 전달)
         *    - 데이터베이스 ↔ UI
         * 
         * 2. ViewModel:
         *    - INotifyPropertyChanged 구현
         *    - Command 추가
         *    - 비즈니스 로직 포함
         * 
         * 3. Model:
         *    - 도메인 로직
         *    - 유효성 검사
         *    - 비즈니스 규칙
         * 
         * ========== 주의사항 ==========
         * 
         * 1. public 접근자:
         *    - 바인딩을 위해 public get 필수
         *    - private, protected는 바인딩 안 됨
         * 
         * 2. 속성 이름:
         *    - XAML 바인딩과 정확히 일치
         *    - 대소문자 구분 (이름 ≠ 名前)
         * 
         * 3. internal 클래스:
         *    - 같은 어셈블리 내에서만 접근
         *    - XAML 바인딩은 정상 동작
         * 
         * 4. 초기화:
         *    - 속성은 null이 기본값
         *    - 초기화하지 않으면 null 표시
         */
    }
}
