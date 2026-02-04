using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ch13_익스팬더
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 탭3: Expanded / Collapsed 이벤트
        private void expEvent_Expanded(object sender, RoutedEventArgs e)
        {
            tbEventResult.Text = "Expanded! 펼쳐졌습니다.";
        }

        private void expEvent_Collapsed(object sender, RoutedEventArgs e)
        {
            tbEventResult.Text = "Collapsed! 접혔습니다.";
        }

        // 탭3: 코드비하인드로 IsExpanded 제어
        private void ExpandClick(object sender, RoutedEventArgs e)
        {
            expCodeBehind.IsExpanded = true;
        }

        private void CollapseClick(object sender, RoutedEventArgs e)
        {
            expCodeBehind.IsExpanded = false;
        }

        private void ToggleClick(object sender, RoutedEventArgs e)
        {
            expCodeBehind.IsExpanded = !expCodeBehind.IsExpanded;
        }

        // 탭3: 아코디언 패턴
        private void Accordion_Expanded(object sender, RoutedEventArgs e)
        {
            var current = sender as Expander;
            if (current != expAccordion1) expAccordion1.IsExpanded = false;
            if (current != expAccordion2) expAccordion2.IsExpanded = false;
            if (current != expAccordion3) expAccordion3.IsExpanded = false;
        }

        // 탭5: 헤더 동적 변경
        private void ChangeHeader_Click(object sender, RoutedEventArgs e)
        {
            expDynamic.Header = "변경된 헤더 (" + DateTime.Now.ToString("HH:mm:ss") + ")";
        }

        // 탭5: 배경색 동적 변경
        private void ChangeBackground_Click(object sender, RoutedEventArgs e)
        {
            expDynamic.Background = expDynamic.Background == Brushes.LightCoral
                ? Brushes.LightCyan : Brushes.LightCoral;
        }
    }
}
