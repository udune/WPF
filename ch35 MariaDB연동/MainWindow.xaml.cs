using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

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

        // ===== 직접 해보기 =====

        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            { "1_4", "string? connectionString = Environment.GetEnvironmentVariable(\"MARIADB_CONNECTION\");\n\nif (string.IsNullOrEmpty(connectionString))\n{\n    MessageBox.Show(\"연결 문자열이 설정되지 않았습니다.\");\n    return;\n}" },
            { "2_4", "private void LoadData()\n{\n    DataTable dataTable = new DataTable();\n\n    using (var connection = new MySqlConnection(connectionString))\n    using (var command = new MySqlCommand(\"SELECT * FROM person\", connection))\n    {\n        connection.Open();\n        using (var reader = command.ExecuteReader())\n        {\n            dataTable.Load(reader);\n        }\n    }\n\n    Grid.ItemsSource = dataTable.DefaultView;\n}" },
            { "3_4", "command.Parameters.Add(\"@name\", MySqlDbType.VarChar, 100).Value = TxtName.Text;\ncommand.Parameters.Add(\"@age\", MySqlDbType.Int32).Value = int.Parse(TxtAge.Text);" },
            { "4_4", "UPDATE person\nSET IsDeleted = 1, DeletedAt = NOW()\nWHERE ID = @id;\n\nSELECT * FROM person WHERE IsDeleted = 0;" },
            { "1_3", "private const string ConnectionString =\n    \"Server=localhost;Port=3306;Database=testdb;Uid=root;Pwd=1234;CharSet=utf8mb4;\";" },
            { "2_3", "private int CountMembers(MySqlConnection conn)\n{\n    using (var cmd = new MySqlCommand(\"SELECT COUNT(*) FROM member\", conn))\n    {\n        return Convert.ToInt32(cmd.ExecuteScalar());\n    }\n}" },
            { "3_3", "string sql = \"SELECT id, name FROM member WHERE name LIKE @keyword\";" },
            { "4_3", "private void RunBatch(MySqlConnection conn)\n{\n    var tx = conn.BeginTransaction();\n    try\n    {\n        // 여러 명령 실행\n        tx.Commit();\n    }\n    catch\n    {\n        tx.Rollback();\n    }\n}" },
            { "5_3", "private void Fill(MySqlDataReader reader)\n{\n    var rows = new List<string>();\n    while (reader.Read())\n    {\n        rows.Add(reader[\"name\"].ToString());\n    }\n    list.ItemsSource = rows;\n}" },
            { "1_1", "private const string ConnectionString =\n    \"Server=localhost;Port=3306;Database=testdb;Uid=root;Pwd=1234;\";" },
            { "1_2", "private void Connect_Click(object sender, RoutedEventArgs e)\n{\n    using (var conn = new MySqlConnection(ConnectionString))\n    {\n        conn.Open();\n        MessageBox.Show(\"연결 성공\");\n    }\n}" },
            { "2_1", "string sql = \"SELECT id, name, age FROM member ORDER BY id\";" },
            { "2_2", "using (var cmd = new MySqlCommand(sql, conn))\nusing (var reader = cmd.ExecuteReader())\n{\n    while (reader.Read())\n    {\n        string name = reader[\"name\"].ToString();\n    }\n}" },
            { "3_1", "string sql = \"INSERT INTO member (name, age) VALUES (@name, @age)\";\nusing (var cmd = new MySqlCommand(sql, conn))\n{\n    cmd.Parameters.AddWithValue(\"@name\", nameBox.Text);\n    cmd.Parameters.AddWithValue(\"@age\", int.Parse(ageBox.Text));\n    cmd.ExecuteNonQuery();\n}" },
            { "3_2", "int affected = cmd.ExecuteNonQuery();\nif (affected > 0)\n{\n    MessageBox.Show(\"추가되었습니다\");\n}" },
            { "4_1", "string sql = \"UPDATE member SET age = @age WHERE id = @id\";" },
            { "4_2", "string sql = \"DELETE FROM member WHERE id = @id\";" },
            { "5_1", "private void Run()\n{\n    using (var conn = new MySqlConnection(ConnectionString))\n    {\n        conn.Open();\n        using (var cmd = new MySqlCommand(\"SELECT COUNT(*) FROM member\", conn))\n        {\n            object result = cmd.ExecuteScalar();\n        }\n    }\n}" },
            { "5_2", "private void Safe()\n{\n    try\n    {\n        using (var conn = new MySqlConnection(ConnectionString))\n        {\n            conn.Open();\n        }\n    }\n    catch (Exception ex)\n    {\n        MessageBox.Show(\"오류: \" + ex.Message);\n    }\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "1_4", new[] { "Environment.GetEnvironmentVariable" } },
            { "2_4", new[] { "using", "MySqlConnection", "ExecuteReader" } },
            { "3_4", new[] { "Parameters.Add", "MySqlDbType" } },
            { "4_4", new[] { "UPDATE", "IsDeleted", "WHERE" } },
            { "1_3", new[] { "CharSet", "utf8mb4" } },
            { "2_3", new[] { "ExecuteScalar", "COUNT(*)", "Convert.ToInt32" } },
            { "3_3", new[] { "SELECT", "WHERE", "LIKE", "@keyword" } },
            { "4_3", new[] { "BeginTransaction", "Commit", "Rollback" } },
            { "5_3", new[] { "while (reader.Read())", "rows.Add", "ItemsSource" } },
            { "1_1", new[] { "Server=", "Port=", "Database=", "Uid=", "Pwd=" } },
            { "1_2", new[] { "using", "new MySqlConnection", "conn.Open()" } },
            { "2_1", new[] { "SELECT", "FROM", "member" } },
            { "2_2", new[] { "ExecuteReader", "reader.Read()", "reader[" } },
            { "3_1", new[] { "Parameters.AddWithValue", "@name", "@age", "ExecuteNonQuery" } },
            { "3_2", new[] { "ExecuteNonQuery", "affected" } },
            { "4_1", new[] { "UPDATE", "SET", "WHERE", "@id" } },
            { "4_2", new[] { "DELETE", "FROM", "WHERE", "@id" } },
            { "5_1", new[] { "using", "MySqlConnection", "MySqlCommand", "Open()" } },
            { "5_2", new[] { "try", "catch", "ex.Message" } },
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
