using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace ch11_데이트피커
{
    public partial class MainWindow : Window
    {
        // 각 연습의 정답
        private readonly Dictionary<string, string> _answers = new()
        {
            // 탭 1: 기본 사용법
            { "1_1", "<DatePicker/>" },
            { "1_2", "<DatePicker SelectedDate=\"2025-12-25\"/>" },
            { "1_3", "<DatePicker SelectedDateFormat=\"Long\"/>" },

            // 탭 2: 날짜 범위 제한
            { "2_1", "<DatePicker DisplayDateStart=\"2025-03-01\" DisplayDateEnd=\"2025-03-31\"/>" },
            { "2_2", "private void DisablePastDates()\n{\n    datePickerControl.BlackoutDates.AddDatesInPast();\n}" },

            // 탭 3: 이벤트와 텍스트 입력
            { "3_1", "private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)\n{\n    if (myDatePicker.SelectedDate.HasValue)\n    {\n        txtResult.Text = myDatePicker.SelectedDate.Value.ToString(\"yyyy-MM-dd\");\n    }\n}" },
            { "3_2", "<DatePicker IsTodayHighlighted=\"False\"/>" },

            // 탭 4: 데이터 바인딩
            { "4_1", "<TextBlock Text=\"{Binding ElementName=dpSource, Path=Text}\"/>" },

            // 탭 5: 스타일과 코드비하인드
            { "5_1", "<DatePicker Background=\"LightGreen\" BorderBrush=\"Green\"/>" },
            { "5_2", "<DatePicker FirstDayOfWeek=\"Monday\"/>" },
            { "5_3", "private void SelectTodayDate()\n{\n    myDatePicker.SelectedDate = DateTime.Today;\n}" },
        };

        // 코드 비교 검증용 필수 키워드
        private readonly Dictionary<string, string[]> _requiredKeywords = new()
        {
            { "2_2", new[] { "BlackoutDates", "AddDatesInPast" } },
            { "3_1", new[] { "SelectedDate", "HasValue", "Value" } },
            { "5_3", new[] { "SelectedDate", "DateTime.Today" } },
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        #region 직접 해보기 이벤트 핸들러

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

                    // 모든 필수 키워드가 포함되어 있는지 확인
                    var missingKeywords = keywords.Where(k => !userCode.Contains(k)).ToList();

                    if (missingKeywords.Count == 0)
                    {
                        txtResult.Text = "정답입니다! 모든 필수 요소가 포함되어 있습니다.";
                        txtResult.Foreground = Brushes.Green;
                    }
                    else
                    {
                        txtResult.Text = $"다시 확인해보세요. 누락된 요소: {string.Join(", ", missingKeywords)}";
                        txtResult.Foreground = Brushes.Red;
                    }
                }
            }
        }

        // XAML 실행 메서드
        private void ExecuteXaml(string xamlCode, StackPanel resultPanel, Border resultBorder)
        {
            resultPanel.Children.Clear();
            resultBorder.Visibility = Visibility.Visible;

            try
            {
                string fullXaml = xamlCode;

                // XAML 네임스페이스 추가
                if (!xamlCode.Contains("xmlns="))
                {
                    string trimmed = xamlCode.TrimStart();
                    if (trimmed.StartsWith("<DatePicker"))
                    {
                        fullXaml = xamlCode.Replace("<DatePicker",
                            "<DatePicker xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<StackPanel"))
                    {
                        fullXaml = xamlCode.Replace("<StackPanel",
                            "<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                    else if (trimmed.StartsWith("<TextBlock"))
                    {
                        fullXaml = xamlCode.Replace("<TextBlock",
                            "<TextBlock xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'");
                    }
                }

                var element = XamlReader.Parse(fullXaml) as UIElement;
                if (element != null)
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

        #endregion

        #region 기존 이벤트 핸들러

        // 탭2: 오늘 이전 날짜 비활성화
        private void BlackoutPastDates_Click(object sender, RoutedEventArgs e)
        {
            dpBlackoutCode.BlackoutDates.AddDatesInPast();
        }

        // 탭3: SelectedDateChanged 이벤트
        private void dpEvent_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpEvent.SelectedDate.HasValue)
            {
                tbEventResult.Text = "선택: " + dpEvent.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
        }

        // 탭3: 달력 팝업 열기/닫기
        private void OpenDropDown_Click(object sender, RoutedEventArgs e)
        {
            dpDropDown.IsDropDownOpen = true;
        }

        private void CloseDropDown_Click(object sender, RoutedEventArgs e)
        {
            dpDropDown.IsDropDownOpen = false;
        }

        // 탭5: 오늘 선택
        private void SelectToday_Click(object sender, RoutedEventArgs e)
        {
            dpCodeBehind.SelectedDate = DateTime.Today;
            tbCodeBehindResult.Text = "오늘 선택: " + DateTime.Today.ToString("yyyy-MM-dd");
        }

        // 탭5: 선택 해제
        private void ClearDate_Click(object sender, RoutedEventArgs e)
        {
            dpCodeBehind.SelectedDate = null;
            tbCodeBehindResult.Text = "선택이 해제되었습니다.";
        }

        // 탭5: Short/Long 형식 전환
        private void ToggleFormat_Click(object sender, RoutedEventArgs e)
        {
            if (dpCodeBehind.SelectedDateFormat == DatePickerFormat.Short)
            {
                dpCodeBehind.SelectedDateFormat = DatePickerFormat.Long;
                tbCodeBehindResult.Text = "Long 형식으로 변경";
            }
            else
            {
                dpCodeBehind.SelectedDateFormat = DatePickerFormat.Short;
                tbCodeBehindResult.Text = "Short 형식으로 변경";
            }
        }

        #endregion
    }
}
