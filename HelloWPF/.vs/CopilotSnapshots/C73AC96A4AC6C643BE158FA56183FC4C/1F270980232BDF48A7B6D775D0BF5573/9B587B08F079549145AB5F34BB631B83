using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySqlConnector;

namespace ch35_MariaDB연동;

/// <summary>
/// MainWindow.xaml에 대한 상호 작용 논리
/// WPF에서 MariaDB 데이터베이스와 연동하여 CRUD 작업을 수행하는 예제입니다.
/// 
/// 주요 기능:
/// - 데이터베이스 연결 및 조회
/// - 데이터 삽입 (Insert)
/// - 데이터 수정 (Update)
/// - 데이터 삭제 (Delete)
/// - DataGrid와 TextBox 바인딩
/// 
/// 사용 기술:
/// - MySqlConnector: MariaDB/MySQL 연결 라이브러리
/// - ADO.NET: 데이터베이스 액세스 기술
/// - DataTable: 메모리 내 데이터 테이블
/// - DataGrid: 데이터 표시 및 선택
/// </summary>
public partial class MainWindow : Window
{
    /*
     * ========== MySqlConnector 개요 ==========
     * 
     * MySqlConnector:
     * - .NET용 MySQL/MariaDB 클라이언트 라이브러리
     * - NuGet 패키지: MySqlConnector
     * - 비동기 지원, 성능 최적화
     * - MIT 라이선스 (무료)
     * 
     * NuGet 설치:
     * Install-Package MySqlConnector
     * 
     * 또는 .csproj에 추가:
     * <PackageReference Include="MySqlConnector" Version="2.x.x" />
     * 
     * 다른 MySQL 라이브러리:
     * - MySql.Data (Oracle 공식, GPL 라이선스)
     * - Dapper (ORM, MySqlConnector와 함께 사용)
     * - Entity Framework Core (ORM)
     */
    
    /// <summary>
    /// MySqlConnection 객체
    /// MariaDB 데이터베이스와의 연결을 관리합니다.
    /// </summary>
    MySqlConnection connection = new MySqlConnection("Server=localhost;Uid=root;Database=study;port=3307;pwd=1234");
    /*
     * ========== 연결 문자열 (Connection String) ==========
     * 
     * "Server=localhost;Uid=root;Database=study;port=3307;pwd=1234"
     * 
     * 구성 요소:
     * - Server=localhost: 데이터베이스 서버 주소
     *   - localhost: 로컬 컴퓨터
     *   - 127.0.0.1: IP 주소
     *   - example.com: 원격 서버
     * 
     * - Uid=root: 사용자 ID
     *   - root: 관리자 계정
     *   - 실무: 별도 사용자 계정 생성 권장
     * 
     * - Database=study: 데이터베이스 이름
     *   - 연결할 데이터베이스
     *   - 사전 생성 필요
     * 
     * - port=3307: 포트 번호
     *   - MariaDB 기본 포트: 3306
     *   - 이 예제: 3307 (사용자 설정)
     *   - 생략 시 기본 3306 사용
     * 
     * - pwd=1234: 비밀번호
     *   - Password 또는 pwd
     *   - 보안상 암호화 권장
     * 
     * 다른 연결 문자열 옵션:
     * - CharSet=utf8mb4: 문자 인코딩
     * - SslMode=None/Preferred/Required: SSL 연결
     * - AllowUserVariables=True: 사용자 변수 허용
     * - ConnectionTimeout=30: 연결 타임아웃 (초)
     * - Pooling=true: 연결 풀링
     * - MinPoolSize=0, MaxPoolSize=100: 풀 크기
     * 
     * 안전한 연결 문자열 관리:
     * 
     * 1. appsettings.json:
     * {
     *   "ConnectionStrings": {
     *     "MariaDB": "Server=localhost;Uid=root;Database=study;port=3307;pwd=1234"
     *   }
     * }
     * 
     * 사용:
     * var configuration = new ConfigurationBuilder()
     *     .AddJsonFile("appsettings.json")
     *     .Build();
     * var connectionString = configuration.GetConnectionString("MariaDB");
     * 
     * 2. 환경 변수:
     * var connectionString = Environment.GetEnvironmentVariable("MARIADB_CONNECTION");
     * 
     * 3. User Secrets (개발):
     * dotnet user-secrets set "ConnectionStrings:MariaDB" "Server=..."
     * 
     * 4. Azure Key Vault (프로덕션):
     * var connectionString = keyVaultClient.GetSecretAsync("MariaDBConnection").Result.Value;
     * 
     * 보안 주의사항:
     * - 연결 문자열을 소스 코드에 하드코딩하지 말 것
     * - Git에 커밋하지 말 것 (.gitignore에 추가)
     * - 최소 권한 원칙 (root 대신 제한된 사용자)
     * - SSL/TLS 연결 사용
     */
    
    /// <summary>
    /// MainWindow 생성자
    /// 윈도우를 초기화하고 데이터베이스에서 데이터를 로드합니다.
    /// </summary>
    public MainWindow()
    {
        /*
         * ========== 생성자 실행 순서 ==========
         * 
         * 1. InitializeComponent():
         *    - XAML 파싱 및 로드
         *    - UI 요소 생성 (DataGrid, TextBox, Button)
         *    - 바인딩 설정
         *    - 이벤트 핸들러 연결
         * 
         * 2. LoadData():
         *    - 데이터베이스 연결
         *    - 데이터 조회
         *    - DataGrid에 표시
         * 
         * 초기화 시점:
         * - Window가 생성될 때
         * - App.xaml의 StartupUri 또는 Show() 호출
         */
        InitializeComponent();
        /*
         * InitializeComponent():
         * - 자동 생성된 메서드
         * - MainWindow.g.cs 파일에 정의
         * - XAML을 C# 코드로 변환
         */
        
        LoadData();
        /*
         * LoadData():
         * - 데이터베이스에서 데이터 로드
         * - DataGrid에 표시
         * - 초기 화면 구성
         * 
         * 실행 타이밍:
         * - 생성자에서 호출 (즉시 로드)
         * - 또는 Loaded 이벤트에서 호출 (지연 로드)
         * 
         * Loaded 이벤트 사용 예:
         * public MainWindow()
         * {
         *     InitializeComponent();
         *     Loaded += MainWindow_Loaded;
         * }
         * 
         * private void MainWindow_Loaded(object sender, RoutedEventArgs e)
         * {
         *     LoadData();
         * }
         * 
         * 장점:
         * - UI가 완전히 로드된 후 데이터 로드
         * - 프로그레스바 표시 가능
         */
    }

    /// <summary>
    /// LoadData - 데이터베이스에서 데이터를 조회하여 DataGrid에 표시
    /// SELECT 쿼리를 실행하고 결과를 DataTable에 로드합니다.
    /// </summary>
    private void LoadData()
    {
        /*
         * ========== ADO.NET 데이터 조회 패턴 ==========
         * 
         * 단계:
         * 1. DataTable 생성 (메모리 내 테이블)
         * 2. MySqlCommand 생성 (SQL 명령)
         * 3. Connection 열기
         * 4. MySqlDataReader로 데이터 읽기
         * 5. DataTable에 로드
         * 6. Connection 닫기
         * 7. DataGrid에 바인딩
         */
        
        DataTable dataTable = new DataTable();
        /*
         * DataTable:
         * - 메모리 내 데이터베이스 테이블
         * - System.Data 네임스페이스
         * - 행(Rows)과 열(Columns) 컬렉션
         * 
         * 구조:
         * - Columns: 열 정의 (DataColumn)
         * - Rows: 데이터 행 (DataRow)
         * - PrimaryKey: 기본 키
         * - Constraints: 제약 조건
         * 
         * 장점:
         * - 데이터베이스 연결 없이 작업 가능
         * - DataGrid, ListView 등에 바인딩 용이
         * - 필터링, 정렬 지원
         * 
         * 단점:
         * - 메모리 사용량 (대용량 데이터)
         * - 타입 안전성 부족 (object 타입)
         * 
         * 대안:
         * - List<T>: 강타입 컬렉션
         * - ObservableCollection<T>: WPF 바인딩 최적화
         * - Entity Framework: ORM
         */
        
        MySqlCommand command = new MySqlCommand("SELECT * FROM person", connection);
        /*
         * MySqlCommand:
         * - SQL 명령을 실행하는 객체
         * - 쿼리 실행 및 매개변수화
         * 
         * 생성자 매개변수:
         * - "SELECT * FROM person": SQL 쿼리
         * - connection: MySqlConnection 객체
         * 
         * SELECT * FROM person:
         * - person 테이블의 모든 컬럼, 모든 행 조회
         * - *: 모든 컬럼
         * - 실무: 필요한 컬럼만 명시 권장
         *   - SELECT ID, Name, Age, Address FROM person
         * 
         * MySqlCommand 속성:
         * - CommandText: SQL 쿼리
         * - Connection: 데이터베이스 연결
         * - CommandType: Text, StoredProcedure, TableDirect
         * - CommandTimeout: 명령 타임아웃 (초)
         * - Parameters: 매개변수 컬렉션
         * 
         * 실행 메서드:
         * - ExecuteReader(): 데이터 읽기 (SELECT)
         * - ExecuteNonQuery(): 데이터 변경 (INSERT, UPDATE, DELETE)
         * - ExecuteScalar(): 단일 값 반환 (COUNT, MAX 등)
         * 
         * using 문 사용 권장:
         * using (var command = new MySqlCommand("SELECT * FROM person", connection))
         * {
         *     // 자동으로 Dispose (리소스 해제)
         * }
         */
        
        connection.Open();
        /*
         * connection.Open():
         * - 데이터베이스 연결 열기
         * - TCP/IP 소켓 생성
         * - 인증 수행
         * 
         * 연결 상태:
         * - Closed: 닫힘 (기본)
         * - Open: 열림
         * - Connecting: 연결 중
         * - Executing: 명령 실행 중
         * - Fetching: 데이터 가져오는 중
         * - Broken: 손상됨
         * 
         * connection.State 확인:
         * if (connection.State != ConnectionState.Open)
         * {
         *     connection.Open();
         * }
         * 
         * 예외:
         * - MySqlException: 연결 실패
         *   - 서버 연결 불가
         *   - 인증 실패
         *   - 데이터베이스 없음
         * 
         * 성능:
         * - 연결 열기는 비용이 큼
         * - 연결 풀링 사용 (기본 활성화)
         * - 가능한 빨리 닫기
         * 
         * 비동기:
         * await connection.OpenAsync();
         * - UI 블로킹 방지
         * - 권장 패턴
         */
        
        MySqlDataReader dataReader = command.ExecuteReader();
        /*
         * ExecuteReader():
         * - SELECT 쿼리 실행
         * - MySqlDataReader 반환
         * - 순방향, 읽기 전용 데이터 스트림
         * 
         * MySqlDataReader:
         * - 데이터베이스 결과를 순차적으로 읽기
         * - 메모리 효율적 (한 번에 하나의 행)
         * - 연결 유지 필요 (Connected)
         * 
         * 사용 패턴:
         * while (dataReader.Read())
         * {
         *     int id = dataReader.GetInt32(0);              // 인덱스
         *     string name = dataReader.GetString("Name");   // 열 이름
         *     int age = dataReader.GetInt32("Age");
         *     string address = dataReader.GetString("Address");
         *     
         *     Console.WriteLine($"{id}: {name}, {age}, {address}");
         * }
         * 
         * 타입 안전 메서드:
         * - GetInt32(), GetInt64()
         * - GetString()
         * - GetDateTime()
         * - GetBoolean()
         * - GetDecimal()
         * - GetDouble()
         * 
         * null 체크:
         * if (!dataReader.IsDBNull(1))
         * {
         *     string name = dataReader.GetString(1);
         * }
         * 
         * 또는:
         * string name = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);
         * 
         * 여러 결과 집합:
         * dataReader.NextResult(); // 다음 결과 집합으로 이동
         * 
         * 리소스 해제:
         * dataReader.Close(); // 명시적 닫기
         * 또는 using 사용
         */
        
        dataTable.Load(dataReader);
        /*
         * DataTable.Load(dataReader):
         * - DataReader의 모든 데이터를 DataTable에 로드
         * - 자동으로 스키마(열) 생성
         * - 모든 행 읽기
         * 
         * 과정:
         * 1. DataReader에서 스키마 읽기
         * 2. DataTable에 열(Columns) 생성
         * 3. DataReader의 모든 행을 DataTable.Rows에 추가
         * 
         * 결과:
         * - DataTable에 person 테이블의 모든 데이터
         * - 연결 해제 후에도 사용 가능 (Disconnected)
         * 
         * 수동 로드 (대안):
         * while (dataReader.Read())
         * {
         *     DataRow row = dataTable.NewRow();
         *     row["ID"] = dataReader["ID"];
         *     row["Name"] = dataReader["Name"];
         *     row["Age"] = dataReader["Age"];
         *     row["Address"] = dataReader["Address"];
         *     dataTable.Rows.Add(row);
         * }
         * 
         * 장점 (Load 메서드):
         * - 간결함
         * - 자동 스키마 생성
         * - 오류 가능성 낮음
         * 
         * DataTable 조작:
         * - dataTable.Select("Age > 30"): 필터링
         * - dataTable.DefaultView.Sort = "Name ASC": 정렬
         * - dataTable.Rows.Count: 행 개수
         * - dataTable.Columns.Count: 열 개수
         */
        
        connection.Close();
        /*
         * connection.Close():
         * - 데이터베이스 연결 닫기
         * - 리소스 해제
         * - 연결 풀에 반환
         * 
         * 중요성:
         * - 열린 연결은 서버 리소스 소비
         * - 연결 수 제한 (MaxPoolSize)
         * - 가능한 빨리 닫아야 함
         * 
         * using 문 패턴 (권장):
         * using (var connection = new MySqlConnection(connectionString))
         * {
         *     connection.Open();
         *     // 작업 수행
         * } // 자동으로 Close 호출
         * 
         * try-finally 패턴:
         * try
         * {
         *     connection.Open();
         *     // 작업 수행
         * }
         * finally
         * {
         *     if (connection.State == ConnectionState.Open)
         *     {
         *         connection.Close();
         *     }
         * }
         * 
         * 연결 풀링:
         * - Close()해도 물리적 연결은 유지
         * - 풀에 반환하여 재사용
         * - 성능 향상
         * 
         * 풀 비활성화:
         * "Server=localhost;...;Pooling=false"
         */
        
        Grid.ItemsSource = dataTable.DefaultView;
        /*
         * Grid.ItemsSource:
         * - DataGrid의 데이터 소스 설정
         * - IEnumerable 구현 객체
         * 
         * dataTable.DefaultView:
         * - DataTable의 기본 DataView
         * - 필터링, 정렬 가능
         * - DataGrid 바인딩에 최적화
         * 
         * DataView:
         * - DataTable의 뷰 (View)
         * - 원본 DataTable 수정 없이 필터/정렬
         * 
         * 설정 예:
         * dataTable.DefaultView.RowFilter = "Age > 30";
         * dataTable.DefaultView.Sort = "Name ASC";
         * 
         * 직접 DataTable 바인딩도 가능:
         * Grid.ItemsSource = dataTable.Rows;
         * // 또는
         * Grid.ItemsSource = dataTable.AsDataView();
         * 
         * 결과:
         * - DataGrid에 person 테이블의 모든 데이터 표시
         * - 자동으로 열 생성 (AutoGenerateColumns=True)
         * - 행 선택 시 TextBox에 바인딩 (XAML에서 설정)
         * 
         * XAML 바인딩:
         * <DataGrid Name="Grid" ItemsSource="{Binding}">
         * 
         * 코드에서:
         * this.DataContext = dataTable.DefaultView;
         * 
         * 강타입 바인딩 (대안):
         * List<Person> persons = new List<Person>();
         * // persons 채우기
         * Grid.ItemsSource = persons;
         * 
         * ObservableCollection (WPF 권장):
         * ObservableCollection<Person> persons = new ObservableCollection<Person>();
         * Grid.ItemsSource = persons;
         * persons.Add(new Person()); // 자동 UI 업데이트
         */
    }

    /// <summary>
    /// BtnClear_OnClick - 클리어 버튼 클릭 이벤트
    /// 모든 입력 필드를 초기화합니다.
    /// </summary>
    private void BtnClear_OnClick(object sender, RoutedEventArgs e)
    {
        /*
         * ========== 클리어 버튼 ==========
         * 
         * 목적:
         * - 입력 필드 초기화
         * - 새 데이터 입력 준비
         * - 선택 해제
         * 
         * XAML:
         * <Button Name="BtnClear" Click="BtnClear_OnClick">클리어</Button>
         */
        
        TxtId.Clear();
        TxtName.Clear();
        TxtAge.Clear();
        TxtAddress.Clear();
        /*
         * TextBox.Clear():
         * - Text 속성을 빈 문자열로 설정
         * - TextBox.Text = ""; 과 동일
         * 
         * 다른 방법:
         * TxtId.Text = string.Empty;
         * TxtName.Text = "";
         * 
         * 추가 작업:
         * Grid.SelectedItem = null; // DataGrid 선택 해제
         * TxtName.Focus(); // 포커스 이동
         * 
         * 모든 TextBox 일괄 처리:
         * var textBoxes = new[] { TxtId, TxtName, TxtAge, TxtAddress };
         * foreach (var textBox in textBoxes)
         * {
         *     textBox.Clear();
         * }
         * 
         * 또는 LINQ:
         * this.FindVisualChildren<TextBox>().ToList().ForEach(tb => tb.Clear());
         * 
         * 바인딩된 경우:
         * - TextBox가 DataGrid.SelectedItem에 바인딩
         * - Clear()해도 다시 채워질 수 있음
         * - Grid.SelectedItem = null; 필요
         */
    }

    /// <summary>
    /// BtnInsert_OnClick - 삽입 버튼 클릭 이벤트
    /// 새로운 데이터를 데이터베이스에 추가합니다.
    /// </summary>
    private void BtnInsert_OnClick(object sender, RoutedEventArgs e)
    {
        /*
         * ========== INSERT 작업 ==========
         * 
         * 단계:
         * 1. 입력 검증
         * 2. MySqlCommand 생성 (매개변수화된 쿼리)
         * 3. 매개변수 추가
         * 4. Connection 열기
         * 5. ExecuteNonQuery() 실행
         * 6. Connection 닫기
         * 7. 데이터 새로고침
         */
        
        /*
         * ========== 입력 검증 ==========
         */
        if (TxtName.Text.Length == 0)
        {
            MessageBox.Show("이름을 입력하여 주세요.");
            return;
            /*
             * return:
             * - 메서드 종료
             * - 이후 코드 실행 안 됨
             * 
             * MessageBox.Show:
             * - 경고 메시지 표시
             * - 사용자에게 알림
             * 
             * 개선:
             * MessageBox.Show("이름을 입력하여 주세요.", "입력 오류", 
             *                 MessageBoxButton.OK, MessageBoxImage.Warning);
             * TxtName.Focus(); // 포커스 이동
             * 
             * 더 나은 검증:
             * if (string.IsNullOrWhiteSpace(TxtName.Text))
             * {
             *     // 공백만 있어도 감지
             * }
             */
        }

        if (TxtAge.Text.Length == 0)
        {
            MessageBox.Show("나이를 입력하여 주세요.");
            return;
            /*
             * 추가 검증:
             * if (!int.TryParse(TxtAge.Text, out int age))
             * {
             *     MessageBox.Show("나이는 숫자여야 합니다.");
             *     return;
             * }
             * 
             * if (age < 0 || age > 150)
             * {
             *     MessageBox.Show("올바른 나이를 입력하세요.");
             *     return;
             * }
             */
        }

        if (TxtAddress.Text.Length == 0)
        {
            MessageBox.Show("주소를 입력하여 주세요.");
            return;
        }

        /*
         * ========== INSERT 쿼리 실행 ==========
         */
        try
        {
            /*
             * try-catch-finally:
             * - 예외 처리
             * - try: 실행할 코드
             * - catch: 예외 발생 시 처리
             * - finally: 항상 실행 (정리 작업)
             */
            
            MySqlCommand command =
                new MySqlCommand("INSERT INTO person(Name, Age, Address) VALUES (@name, @age, @address)", connection);
            /*
             * ========== 매개변수화된 쿼리 (Parameterized Query) ==========
             * 
             * "INSERT INTO person(Name, Age, Address) VALUES (@name, @age, @address)"
             * 
             * 장점:
             * - SQL Injection 방지 (보안)
             * - 타입 안전성
             * - 성능 (쿼리 재사용)
             * 
             * @name, @age, @address:
             * - 매개변수 플레이스홀더
             * - 실제 값으로 치환됨
             * 
             * SQL Injection 위험 (나쁜 예):
             * string sql = $"INSERT INTO person(Name) VALUES ('{TxtName.Text}')";
             * 
             * 공격:
             * TxtName.Text = "'; DROP TABLE person; --"
             * 결과: DROP TABLE person 실행 (데이터 삭제!)
             * 
             * 매개변수화하면:
             * @name = "'; DROP TABLE person; --"
             * 단순 문자열로 처리 (안전)
             * 
             * INSERT 쿼리 구조:
             * INSERT INTO 테이블명(열1, 열2, ...) VALUES (값1, 값2, ...)
             * 
             * ID 컬럼 생략:
             * - AUTO_INCREMENT 속성
             * - 자동으로 증가하는 값 할당
             * 
             * 테이블 생성 예:
             * CREATE TABLE person (
             *     ID INT AUTO_INCREMENT PRIMARY KEY,
             *     Name VARCHAR(100),
             *     Age INT,
             *     Address VARCHAR(200)
             * );
             */
            
            command.CommandType = CommandType.Text;
            /*
             * CommandType:
             * - Text: SQL 텍스트 (기본)
             * - StoredProcedure: 저장 프로시저
             * - TableDirect: 테이블 직접 액세스 (OLE DB만)
             * 
             * Text (기본값):
             * - SQL 쿼리 문자열
             * - 대부분의 경우 사용
             * 
             * StoredProcedure:
             * command.CommandType = CommandType.StoredProcedure;
             * command.CommandText = "sp_InsertPerson";
             * command.Parameters.AddWithValue("@name", TxtName.Text);
             * 
             * 저장 프로시저 예:
             * DELIMITER //
             * CREATE PROCEDURE sp_InsertPerson(
             *     IN p_name VARCHAR(100),
             *     IN p_age INT,
             *     IN p_address VARCHAR(200)
             * )
             * BEGIN
             *     INSERT INTO person(Name, Age, Address) VALUES (p_name, p_age, p_address);
             * END //
             * DELIMITER ;
             * 
             * 생략 가능 (기본값이 Text):
             * // command.CommandType = CommandType.Text; 없어도 됨
             */
            
            command.Parameters.AddWithValue("@name", TxtName.Text);
            command.Parameters.AddWithValue("@age", TxtAge.Text);
            command.Parameters.AddWithValue("@address", TxtAddress.Text);
            /*
             * AddWithValue:
             * - 매개변수 추가 (간편한 방법)
             * - 자동 타입 추론
             * 
             * 매개변수:
             * - "@name": 매개변수 이름 (SQL 쿼리의 @name)
             * - TxtName.Text: 매개변수 값
             * 
             * 명시적 타입 지정 (권장):
             * command.Parameters.Add("@name", MySqlDbType.VarChar).Value = TxtName.Text;
             * command.Parameters.Add("@age", MySqlDbType.Int32).Value = int.Parse(TxtAge.Text);
             * command.Parameters.Add("@address", MySqlDbType.VarChar).Value = TxtAddress.Text;
             * 
             * MySqlDbType:
             * - VarChar: 가변 길이 문자열
             * - Int32: 정수
             * - DateTime: 날짜/시간
             * - Decimal: 고정 소수점
             * - Bit: 불린
             * - Blob: 이진 데이터
             * 
             * 장점 (명시적 타입):
             * - 타입 안전성
             * - 성능 최적화
             * - 명확성
             * 
             * null 값 처리:
             * command.Parameters.AddWithValue("@name", 
             *     string.IsNullOrEmpty(TxtName.Text) ? DBNull.Value : TxtName.Text);
             * 
             * 매개변수 재사용:
             * command.Parameters.Clear(); // 기존 매개변수 제거
             * command.Parameters.AddWithValue("@name", "새 이름");
             */
            
            connection.Open();
            /*
             * Connection 열기:
             * - 데이터베이스 연결
             * - INSERT 실행 준비
             * 
             * 예외 가능성:
             * - 서버 연결 불가
             * - 네트워크 문제
             * - 인증 실패
             */
            
            command.ExecuteNonQuery();
            /*
             * ExecuteNonQuery():
             * - 데이터 변경 쿼리 실행 (INSERT, UPDATE, DELETE)
             * - 영향받은 행 수 반환 (int)
             * 
             * 반환값:
             * int rowsAffected = command.ExecuteNonQuery();
             * 
             * 결과 확인:
             * if (rowsAffected > 0)
             * {
             *     MessageBox.Show($"{rowsAffected}개 행이 삽입되었습니다.");
             * }
             * else
             * {
             *     MessageBox.Show("삽입 실패");
             * }
             * 
             * 다른 Execute 메서드:
             * - ExecuteReader(): SELECT (데이터 읽기)
             * - ExecuteScalar(): 단일 값 반환
             *   - SELECT COUNT(*) FROM person
             *   - int count = (int)command.ExecuteScalar();
             * 
             * 예외:
             * - MySqlException: 쿼리 오류
             *   - 구문 오류
             *   - 제약 조건 위반 (UNIQUE, FOREIGN KEY)
             *   - 권한 부족
             * 
             * 비동기:
             * await command.ExecuteNonQueryAsync();
             * - UI 블로킹 방지
             * - 권장 패턴 (WPF)
             */

            MessageBox.Show("입력되었습니다.");
            /*
             * 성공 메시지:
             * - 사용자에게 알림
             * - UX 개선
             * 
             * 개선:
             * MessageBox.Show("데이터가 성공적으로 입력되었습니다.", "성공", 
             *                 MessageBoxButton.OK, MessageBoxImage.Information);
             * 
             * 토스트 알림 (대안):
             * // NotificationManager.Show("입력 완료");
             */
        }
        catch (Exception exception)
        {
            /*
             * 예외 처리:
             * - 오류 발생 시 실행
             * - 사용자에게 오류 메시지 표시
             * 
             * Exception 타입:
             * - MySqlException: 데이터베이스 오류
             * - InvalidOperationException: 연결 상태 오류
             * - FormatException: 데이터 형식 오류
             */
            MessageBox.Show(exception.Message);
            /*
             * exception.Message:
             * - 오류 메시지
             * - 디버깅 정보
             * 
             * 더 나은 오류 처리:
             * catch (MySqlException ex)
             * {
             *     switch (ex.Number)
             *     {
             *         case 1062: // Duplicate entry
             *             MessageBox.Show("중복된 데이터입니다.");
             *             break;
             *         case 1406: // Data too long
             *             MessageBox.Show("입력 데이터가 너무 깁니다.");
             *             break;
             *         default:
             *             MessageBox.Show($"오류: {ex.Message}");
             *             break;
             *     }
             * }
             * catch (Exception ex)
             * {
             *     MessageBox.Show($"예상치 못한 오류: {ex.Message}");
             *     // 로그 기록
             *     Logger.Error(ex);
             * }
             * 
             * 로깅:
             * - 오류를 파일/데이터베이스에 기록
             * - 디버깅 및 모니터링
             * - NLog, Serilog, log4net
             */
        }
        finally
        {
            /*
             * finally 블록:
             * - 항상 실행 (성공/실패 무관)
             * - 리소스 정리
             * - Connection 닫기
             */
            connection.Close();
            /*
             * Connection 닫기:
             * - 필수
             * - 리소스 해제
             * - 연결 풀에 반환
             * 
             * 이미 닫혀 있어도 안전:
             * if (connection.State == ConnectionState.Open)
             * {
             *     connection.Close();
             * }
             */
            
            LoadData();
            /*
             * 데이터 새로링:
             * - 새로 삽입된 데이터 표시
             * - DataGrid 업데이트
             * 
             * 성능 고려:
             * - 전체 데이터 다시 로드
             * - 대용량 데이터: 새 행만 추가
             * 
             * ObservableCollection 사용 시:
             * persons.Add(new Person { Name = TxtName.Text, ... });
             * // 자동으로 DataGrid 업데이트 (LoadData 불필요)
             */
        }
    }

    /// <summary>
    /// BtnUpdate_OnClick - 수정 버튼 클릭 이벤트
    /// 선택된 데이터를 수정합니다.
    /// </summary>
    private void BtnUpdate_OnClick(object sender, RoutedEventArgs e)
    {
        /*
         * ========== UPDATE 작업 ==========
         * 
         * 단계:
         * 1. ID 확인 (선택된 항목)
         * 2. UPDATE 쿼리 실행
         * 3. 데이터 새로고침
         * 
         * 주의:
         * - SQL Injection 취약점 (이 코드)
         * - 매개변수화 필요
         */
        try
        {
            MySqlCommand command =
                new MySqlCommand(
                    $"UPDATE person SET Name='{TxtName.Text}', Age='{TxtAge.Text}', Address='{TxtAddress.Text}' WHERE ID = '{TxtId.Text}'",
                    connection);
            /*
             * ⚠️ 보안 경고: SQL Injection 취약점!
             * 
             * 문제:
             * - 문자열 보간 ($"...{변수}...")
             * - 사용자 입력을 직접 쿼리에 삽입
             * - SQL Injection 공격 가능
             * 
             * 공격 예:
             * TxtId.Text = "1; DROP TABLE person; --"
             * 결과: UPDATE ... WHERE ID = '1; DROP TABLE person; --'
             * → person 테이블 삭제!
             * 
             * ✅ 안전한 코드 (매개변수화):
             * MySqlCommand command = new MySqlCommand(
             *     "UPDATE person SET Name=@name, Age=@age, Address=@address WHERE ID=@id", 
             *     connection);
             * command.Parameters.AddWithValue("@name", TxtName.Text);
             * command.Parameters.AddWithValue("@age", TxtAge.Text);
             * command.Parameters.AddWithValue("@address", TxtAddress.Text);
             * command.Parameters.AddWithValue("@id", TxtId.Text);
             * 
             * UPDATE 쿼리 구조:
             * UPDATE 테이블명 SET 열1=값1, 열2=값2, ... WHERE 조건
             * 
             * WHERE 절 중요:
             * - WHERE 없으면 모든 행 업데이트!
             * - 특정 행만 수정
             * 
             * 여러 조건:
             * WHERE ID=@id AND Name=@name
             * 
             * 트랜잭션 사용 (여러 UPDATE):
             * using (var transaction = connection.BeginTransaction())
             * {
             *     try
             *     {
             *         command1.Transaction = transaction;
             *         command1.ExecuteNonQuery();
             *         
             *         command2.Transaction = transaction;
             *         command2.ExecuteNonQuery();
             *         
             *         transaction.Commit();
             *     }
             *     catch
             *     {
             *         transaction.Rollback();
             *         throw;
             *     }
             * }
             */
            
            connection.Open();
            command.ExecuteNonQuery();
            /*
             * ExecuteNonQuery():
             * - UPDATE 쿼리 실행
             * - 영향받은 행 수 반환
             * 
             * 결과 확인:
             * int rowsAffected = command.ExecuteNonQuery();
             * if (rowsAffected == 0)
             * {
             *     MessageBox.Show("해당 ID가 존재하지 않습니다.");
             * }
             * else if (rowsAffected == 1)
             * {
             *     MessageBox.Show("수정되었습니다.");
             * }
             * else
             * {
             *     MessageBox.Show($"{rowsAffected}개 행이 수정되었습니다.");
             * }
             * 
             * 낙관적 동시성 제어:
             * UPDATE person 
             * SET Name=@name, Age=@age, Address=@address, Version=Version+1
             * WHERE ID=@id AND Version=@originalVersion
             * 
             * 다른 사용자가 먼저 수정하면 rowsAffected = 0
             */
            
            MessageBox.Show("수정하였습니다.");
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
        finally
        {
            connection.Close();
            LoadData();
        }
    }

    /// <summary>
    /// BtnDelete_OnClick - 삭제 버튼 클릭 이벤트
    /// 선택된 데이터를 삭제합니다.
    /// </summary>
    private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
    {
        /*
         * ========== DELETE 작업 ==========
         * 
         * 단계:
         * 1. ID 확인
         * 2. 삭제 확인 (선택 사항)
         * 3. DELETE 쿼리 실행
         * 4. 데이터 새로고침
         * 
         * 주의:
         * - 복구 불가능
         * - 사용자 확인 필요
         * - SQL Injection 취약점 (이 코드)
         */
        try
        {
            /*
             * 삭제 확인 (권장):
             * var result = MessageBox.Show(
             *     $"ID {TxtId.Text}의 데이터를 삭제하시겠습니까?",
             *     "삭제 확인",
             *     MessageBoxButton.YesNo,
             *     MessageBoxImage.Question);
             * 
             * if (result != MessageBoxResult.Yes)
             * {
             *     return; // 취소
             * }
             */
            
            MySqlCommand command =
                new MySqlCommand(
                    $"DELETE FROM person WHERE ID = '{TxtId.Text}'",
                    connection);
            /*
             * ⚠️ 보안 경고: SQL Injection 취약점!
             * 
             * ✅ 안전한 코드:
             * MySqlCommand command = new MySqlCommand(
             *     "DELETE FROM person WHERE ID=@id", 
             *     connection);
             * command.Parameters.AddWithValue("@id", TxtId.Text);
             * 
             * DELETE 쿼리 구조:
             * DELETE FROM 테이블명 WHERE 조건
             * 
             * ⚠️ WHERE 절 필수:
             * DELETE FROM person
             * → 모든 행 삭제! (매우 위험)
             * 
             * 안전장치:
             * if (string.IsNullOrWhiteSpace(TxtId.Text))
             * {
             *     MessageBox.Show("삭제할 ID를 선택하세요.");
             *     return;
             * }
             * 
             * 소프트 삭제 (Soft Delete):
             * UPDATE person SET IsDeleted=1, DeletedAt=NOW() WHERE ID=@id
             * 
             * 장점:
             * - 실제 삭제 안 함
             * - 복구 가능
             * - 감사 추적
             * 
             * 조회 시 제외:
             * SELECT * FROM person WHERE IsDeleted=0
             * 
             * 외래 키 제약:
             * - 참조하는 행이 있으면 삭제 실패
             * - ON DELETE CASCADE: 자동 삭제
             * - ON DELETE SET NULL: NULL로 설정
             * - ON DELETE RESTRICT: 삭제 금지 (기본)
             * 
             * 여러 행 삭제:
             * DELETE FROM person WHERE Age > 100
             * 
             * 모든 행 삭제 (빠름):
             * TRUNCATE TABLE person
             * - WHERE 절 없음
             * - AUTO_INCREMENT 초기화
             * - 트랜잭션 로그 없음
             * - 복구 불가
             */
            
            connection.Open();
            command.ExecuteNonQuery();
            /*
             * 삭제 결과 확인:
             * int rowsAffected = command.ExecuteNonQuery();
             * if (rowsAffected == 0)
             * {
             *     MessageBox.Show("삭제할 데이터가 없습니다.");
             * }
             * else
             * {
             *     MessageBox.Show($"{rowsAffected}개 행이 삭제되었습니다.");
             *     BtnClear_OnClick(null, null); // 입력 필드 초기화
             * }
             */
            
            MessageBox.Show("삭제하였습니다.");
        }
        catch (Exception exception)
        {
            /*
             * 외래 키 제약 위반:
             * catch (MySqlException ex)
             * {
             *     if (ex.Number == 1451) // Cannot delete or update a parent row
             *     {
             *         MessageBox.Show("다른 테이블에서 참조 중인 데이터입니다. 먼저 참조를 제거하세요.");
             *     }
             *     else
             *     {
             *         MessageBox.Show($"오류: {ex.Message}");
             *     }
             * }
             */
            MessageBox.Show(exception.Message);
        }
        finally
        {
            connection.Close();
            LoadData();
            /*
             * 삭제 후 처리:
             * - 입력 필드 초기화
             * - 선택 해제
             * 
             * BtnClear_OnClick(null, null);
             * Grid.SelectedItem = null;
             */
        }
    }
    
    /*
     * ========== 개선 사항 및 모범 사례 ==========
     * 
     * 1. 매개변수화된 쿼리 사용 (필수):
     *    - SQL Injection 방지
     *    - UPDATE, DELETE에도 적용
     * 
     * 2. using 문 사용:
     *    using (var connection = new MySqlConnection(connectionString))
     *    using (var command = new MySqlCommand(sql, connection))
     *    {
     *        connection.Open();
     *        command.ExecuteNonQuery();
     *    } // 자동으로 리소스 해제
     * 
     * 3. 비동기 메서드:
     *    private async Task LoadDataAsync()
     *    {
     *        await connection.OpenAsync();
     *        var reader = await command.ExecuteReaderAsync();
     *        // ...
     *    }
     * 
     * 4. 강타입 모델:
     *    public class Person
     *    {
     *        public int ID { get; set; }
     *        public string Name { get; set; }
     *        public int Age { get; set; }
     *        public string Address { get; set; }
     *    }
     *    
     *    ObservableCollection<Person> persons = new ObservableCollection<Person>();
     * 
     * 5. Repository 패턴:
     *    public interface IPersonRepository
     *    {
     *        Task<List<Person>> GetAllAsync();
     *        Task<Person> GetByIdAsync(int id);
     *        Task InsertAsync(Person person);
     *        Task UpdateAsync(Person person);
     *        Task DeleteAsync(int id);
     *    }
     * 
     * 6. Dependency Injection:
     *    public MainWindow(IPersonRepository repository)
     *    {
     *        _repository = repository;
     *        InitializeComponent();
     *    }
     * 
     * 7. MVVM 패턴:
     *    - ViewModel에서 데이터베이스 로직
     *    - Command 사용
     *    - 코드 비하인드 최소화
     * 
     * 8. Entity Framework Core (ORM):
     *    public class AppDbContext : DbContext
     *    {
     *        public DbSet<Person> Persons { get; set; }
     *    }
     *    
     *    var persons = await dbContext.Persons.ToListAsync();
     *    dbContext.Persons.Add(new Person { ... });
     *    await dbContext.SaveChangesAsync();
     * 
     * 9. Dapper (Micro ORM):
     *    var persons = await connection.QueryAsync<Person>("SELECT * FROM person");
     *    await connection.ExecuteAsync("INSERT INTO person...", new { name, age, address });
     * 
     * 10. 연결 문자열 보안:
     *     - appsettings.json
     *     - 환경 변수
     *     - User Secrets
     *     - Azure Key Vault
     * 
     * 11. 트랜잭션:
     *     using (var transaction = connection.BeginTransaction())
     *     {
     *         try
     *         {
     *             // 여러 작업
     *             transaction.Commit();
     *         }
     *         catch
     *         {
     *             transaction.Rollback();
     *             throw;
     *         }
     *     }
     * 
     * 12. 연결 풀링:
     *     - 기본 활성화
     *     - MinPoolSize=0, MaxPoolSize=100
     *     - 성능 향상
     * 
     * 13. 로깅:
     *     - NLog, Serilog
     *     - 오류 추적
     *     - 성능 모니터링
     * 
     * 14. 단위 테스트:
     *     [Fact]
     *     public async Task InsertPerson_ValidData_ReturnsSuccess()
     *     {
     *         // Arrange
     *         var person = new Person { Name = "Test", Age = 30 };
     *         
     *         // Act
     *         await _repository.InsertAsync(person);
     *         
     *         // Assert
     *         Assert.True(person.ID > 0);
     *     }
     * 
     * 15. 에러 처리:
     *     - 구체적인 예외 타입
     *     - 사용자 친화적 메시지
     *     - 로그 기록
     */
}