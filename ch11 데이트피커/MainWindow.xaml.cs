using System;
using System.Windows;
using System.Windows.Controls;

namespace ch11_데이트피커
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

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
    }
}
