using System;
using System.Windows;
using System.Windows.Controls;

namespace ch10_캘린더
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
            calBlackoutCode.BlackoutDates.AddDatesInPast();
        }

        // 탭3: SelectedDatesChanged 이벤트
        private void calEvent_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calEvent.SelectedDate.HasValue)
            {
                tbSelectedDate.Text = "선택된 날짜: " + calEvent.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
        }

        // 탭5: 오늘 날짜 선택
        private void SelectToday_Click(object sender, RoutedEventArgs e)
        {
            calCodeBehind.SelectedDate = DateTime.Today;
            tbCodeBehind.Text = "오늘 날짜 선택: " + DateTime.Today.ToString("yyyy-MM-dd");
        }

        // 탭5: 선택 해제
        private void ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            calCodeBehind.SelectedDate = null;
            tbCodeBehind.Text = "선택이 해제되었습니다.";
        }
    }
}
