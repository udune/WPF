using System.ComponentModel;

namespace ch34_MVVM.Models
{
    /// <summary>
    /// PersonModel - MVVM 패턴의 Model 계층
    /// 사람 데이터를 나타내는 데이터 모델 클래스입니다.
    /// INotifyPropertyChanged를 구현하여 데이터 변경 시 UI에 자동으로 반영됩니다.
    /// 
    /// MVVM의 Model:
    /// - 비즈니스 로직과 데이터를 담당
    /// - 데이터의 구조를 정의
    /// - View나 ViewModel을 알지 못함
    /// - 재사용 가능한 순수한 데이터 객체
    /// </summary>
    public class PersonModel : INotifyPropertyChanged
    {
        /*
         * ========== INotifyPropertyChanged 인터페이스 ==========
         * 
         * 목적:
         * - 속성 값이 변경되었을 때 UI에 알림
         * - 데이터 바인딩의 핵심 메커니즘
         * - WPF의 양방향 바인딩 지원
         * 
         * 작동 원리:
         * 1. 속성 값이 변경됨
         * 2. OnPropertyChanged 메서드 호출
         * 3. PropertyChanged 이벤트 발생
         * 4. 바인딩 시스템이 이벤트 감지
         * 5. UI 자동 업데이트
         * 
         * 필수 구현:
         * - PropertyChanged 이벤트
         * - 속성 setter에서 이벤트 발생
         * 
         * 장점:
         * - 코드 비하인드 없이 UI 업데이트
         * - 느슨한 결합 (Loose Coupling)
         * - 테스트 용이
         */
        
        /// <summary>
        /// PropertyChanged 이벤트
        /// 속성 값이 변경될 때 발생하는 이벤트입니다.
        /// WPF 바인딩 시스템이 이 이벤트를 구독하여 UI를 업데이트합니다.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        /*
         * PropertyChangedEventHandler:
         * - 이벤트 핸들러 델리게이트
         * - 시그니처: void(object sender, PropertyChangedEventArgs e)
         * 
         * PropertyChangedEventArgs:
         * - PropertyName: 변경된 속성 이름
         * 
         * ?:
         * - Nullable 참조 타입 (C# 8.0+)
         * - null일 수 있음을 명시
         * 
         * 이벤트 발생 예:
         * PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
         */

        /// <summary>
        /// OnPropertyChanged - 속성 변경 알림 메서드
        /// 속성이 변경되었을 때 PropertyChanged 이벤트를 발생시킵니다.
        /// </summary>
        /// <param name="name">변경된 속성의 이름</param>
        private void OnPropertyChanged(string name)
        {
            /*
             * PropertyChanged 이벤트 발생:
             * 
             * 1. 이벤트 핸들러 참조 가져오기
             * PropertyChangedEventHandler? handler = PropertyChanged;
             * 
             * 이유:
             * - Thread-Safety
             * - 멀티스레드 환경에서 안전
             * - 이벤트 핸들러가 중간에 null이 되는 것 방지
             * 
             * 2. null 체크
             * if (handler != null)
             * 
             * 이유:
             * - 구독자가 없으면 이벤트 발생 불가
             * - NullReferenceException 방지
             * 
             * 3. 이벤트 발생
             * handler(this, new PropertyChangedEventArgs(name));
             * 
             * 매개변수:
             * - this: 이벤트를 발생시킨 객체 (PersonModel 인스턴스)
             * - PropertyChangedEventArgs: 변경된 속성 이름
             * 
             * 간단한 방법 (C# 6.0+):
             * PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
             * 
             * ?. (Null-conditional 연산자):
             * - PropertyChanged가 null이 아니면 Invoke 호출
             * - null이면 아무 일도 하지 않음
             */
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
        /*
         * OnPropertyChanged 호출 패턴:
         * 
         * 속성 setter에서 호출:
         * public string Name
         * {
         *     set
         *     {
         *         name = value;
         *         OnPropertyChanged("Name");  // 또는 OnPropertyChanged(nameof(Name));
         *     }
         * }
         * 
         * nameof 연산자 사용 권장:
         * - OnPropertyChanged(nameof(Name));
         * - 컴파일 타임 타입 안전성
         * - 리팩토링 시 자동 변경
         * - 오타 방지
         * 
         * CallerMemberName 사용 (더 간편):
         * private void OnPropertyChanged([CallerMemberName] string name = "")
         * {
         *     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
         * }
         * 
         * 호출:
         * OnPropertyChanged(); // 속성 이름 자동 전달
         */

        /*
         * ========== 백킹 필드 (Backing Fields) ==========
         * 
         * private string name;
         * private int age;
         * 
         * 목적:
         * - 실제 데이터를 저장하는 필드
         * - 속성(Property)의 데이터 저장소
         * 
         * 명명 규칙:
         * - 소문자로 시작 (camelCase)
         * - 속성 이름과 연관
         * - private 접근 제한자
         * 
         * 왜 필요한가?
         * - 속성은 메서드 (getter/setter)
         * - 실제 데이터는 필드에 저장
         * - setter에서 변경 알림 로직 추가
         */
        private string name = string.Empty;
        /*
         * string name;
         * - 기본값: null
         * - Nullable 경고 가능 (C# 8.0+)
         * 
         * string name = string.Empty;
         * - 기본값: "" (빈 문자열)
         * - Nullable 경고 없음
         * - 권장 패턴
         */
        private int age;
        /*
         * int age;
         * - 기본값: 0
         * - 값 타입 (Value Type)
         * - null 불가
         */

        /// <summary>
        /// Name 속성 - 사람의 이름
        /// 값이 변경되면 PropertyChanged 이벤트를 발생시켜 UI를 업데이트합니다.
        /// </summary>
        public string Name
        {
            /*
             * getter (읽기):
             * - 필드 값을 반환
             * - Expression-bodied member 사용
             * - get => name; (간단한 구문)
             * 
             * 일반 구문:
             * get { return name; }
             */
            get => name;
            
            /*
             * setter (쓰기):
             * - 필드 값을 설정
             * - OnPropertyChanged 호출
             * - UI 업데이트 트리거
             * 
             * value:
             * - setter의 암시적 매개변수
             * - 할당하려는 새 값
             * 
             * 실행 순서:
             * 1. name = value; (필드 업데이트)
             * 2. OnPropertyChanged("Name"); (이벤트 발생)
             * 3. UI 자동 업데이트
             * 
             * 개선 버전 (값 변경 체크):
             * set
             * {
             *     if (name != value)
             *     {
             *         name = value;
             *         OnPropertyChanged(nameof(Name));
             *     }
             * }
             * 
             * 장점:
             * - 불필요한 이벤트 방지
             * - 성능 향상
             * - 무한 루프 방지
             */
            set
            {
                name = value;
                OnPropertyChanged("Name");
                /*
                 * OnPropertyChanged("Name");
                 * - 문자열 리터럴 사용
                 * - 오타 가능성
                 * 
                 * 권장:
                 * OnPropertyChanged(nameof(Name));
                 * - 컴파일 타임 체크
                 * - 리팩토링 안전
                 */
            }
        }

        /// <summary>
        /// Age 속성 - 사람의 나이
        /// 값이 변경되면 PropertyChanged 이벤트를 발생시켜 UI를 업데이트합니다.
        /// </summary>
        public int Age
        {
            /*
             * int 타입 속성:
             * - 값 타입 (Value Type)
             * - 0 ~ 2,147,483,647
             * - null 불가
             * 
             * Nullable int:
             * public int? Age { get; set; }
             * - null 허용
             * - int? = Nullable<int>
             */
            get => age;
            set
            {
                age = value;
                OnPropertyChanged("Age");
            }
        }
        
        /*
         * ========== PersonModel 사용 예제 ==========
         * 
         * 1. 인스턴스 생성:
         * var person = new PersonModel
         * {
         *     Name = "홍길동",
         *     Age = 100
         * };
         * 
         * 2. 속성 변경 (자동 알림):
         * person.Name = "임꺽정";  // PropertyChanged 이벤트 자동 발생
         * person.Age = 90;         // UI 자동 업데이트
         * 
         * 3. 이벤트 구독:
         * person.PropertyChanged += (sender, e) =>
         * {
         *     Console.WriteLine($"속성 '{e.PropertyName}'이(가) 변경됨");
         * };
         * 
         * 4. XAML 바인딩:
         * <TextBox Text="{Binding Name, Mode=TwoWay}"/>
         * <TextBox Text="{Binding Age, Mode=TwoWay}"/>
         * 
         * Mode=TwoWay:
         * - UI → Model: TextBox 변경 시 Name 속성 업데이트
         * - Model → UI: Name 속성 변경 시 TextBox 업데이트
         * 
         * ========== MVVM에서 Model의 역할 ==========
         * 
         * 1. 데이터 구조 정의:
         *    - 엔티티 속성
         *    - 비즈니스 규칙
         * 
         * 2. 데이터 검증:
         *    public string Name
         *    {
         *        set
         *        {
         *            if (string.IsNullOrWhiteSpace(value))
         *                throw new ArgumentException("이름은 필수입니다.");
         *            name = value;
         *            OnPropertyChanged(nameof(Name));
         *        }
         *    }
         * 
         * 3. 계산된 속성:
         *    public string DisplayName => $"{Name} ({Age}세)";
         * 
         * 4. 비즈니스 로직:
         *    public bool IsAdult => Age >= 18;
         * 
         * ========== 고급 패턴 ==========
         * 
         * 1. Base 클래스:
         * public class ObservableObject : INotifyPropertyChanged
         * {
         *     public event PropertyChangedEventHandler PropertyChanged;
         *     
         *     protected void OnPropertyChanged([CallerMemberName] string name = "")
         *     {
         *         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
         *     }
         *     
         *     protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = "")
         *     {
         *         if (EqualityComparer<T>.Default.Equals(field, value))
         *             return false;
         *         
         *         field = value;
         *         OnPropertyChanged(name);
         *         return true;
         *     }
         * }
         * 
         * public class PersonModel : ObservableObject
         * {
         *     private string name;
         *     public string Name
         *     {
         *         get => name;
         *         set => SetProperty(ref name, value);
         *     }
         * }
         * 
         * 2. MVVM Toolkit (CommunityToolkit.Mvvm):
         * [ObservableProperty]
         * private string name;
         * 
         * // 자동 생성됨:
         * // public string Name { get; set; }
         * // OnPropertyChanged 자동 호출
         * 
         * 3. ReactiveUI:
         * public class PersonModel : ReactiveObject
         * {
         *     private string name;
         *     public string Name
         *     {
         *         get => name;
         *         set => this.RaiseAndSetIfChanged(ref name, value);
         *     }
         * }
         * 
         * ========== 주의사항 ==========
         * 
         * 1. 순환 참조 방지:
         *    - Model이 ViewModel 참조 금지
         *    - Model은 독립적이어야 함
         * 
         * 2. 스레드 안전성:
         *    - UI 스레드에서만 PropertyChanged 발생
         *    - Dispatcher 사용
         * 
         * 3. 성능:
         *    - 불필요한 이벤트 방지 (값 변경 체크)
         *    - 많은 속성 변경 시 일괄 처리
         * 
         * 4. 메모리 누수:
         *    - 이벤트 핸들러 해제
         *    - WeakEventManager 사용 고려
         * 
         * 5. 데이터 검증:
         *    - IDataErrorInfo 구현
         *    - ValidationAttribute 사용
         */
    }
}
