using System.Windows;

namespace ch35_MariaDB연동;

/// <summary>
/// ch35 MariaDB 연동 튜토리얼
///
/// 이 튜토리얼은 WPF에서 MariaDB/MySQL 데이터베이스와 연동하는 방법을 설명합니다.
/// 실제 CRUD 동작을 위해서는 MariaDB 서버가 실행 중이어야 합니다.
///
/// 주요 내용:
/// - MySqlConnector 패키지 사용
/// - 연결 문자열 (Connection String)
/// - SELECT, INSERT, UPDATE, DELETE 쿼리
/// - 매개변수화된 쿼리 (SQL Injection 방지)
/// - DataGrid 바인딩
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
