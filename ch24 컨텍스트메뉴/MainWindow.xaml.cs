using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ch24_컨텍스트메뉴
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        // 기본 사용법 - 메시지 표시
        private void ShowMessage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("메뉴 항목이 클릭되었습니다!", "알림");
        }

        // 기본 사용법 - 시간 표시
        private void ShowTime_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"현재 시간: {DateTime.Now:HH:mm:ss}", "시간");
        }

        // 계층 구조와 스타일 - 굵게
        private void Bold_Checked(object sender, RoutedEventArgs e)
        {
            checkableText.FontWeight = FontWeights.Bold;
        }

        private void Bold_Unchecked(object sender, RoutedEventArgs e)
        {
            checkableText.FontWeight = FontWeights.Normal;
        }

        // 계층 구조와 스타일 - 기울임
        private void Italic_Checked(object sender, RoutedEventArgs e)
        {
            checkableText.FontStyle = FontStyles.Italic;
        }

        private void Italic_Unchecked(object sender, RoutedEventArgs e)
        {
            checkableText.FontStyle = FontStyles.Normal;
        }

        // 계층 구조와 스타일 - 밑줄
        private void Underline_Checked(object sender, RoutedEventArgs e)
        {
            checkableText.TextDecorations = TextDecorations.Underline;
        }

        private void Underline_Unchecked(object sender, RoutedEventArgs e)
        {
            checkableText.TextDecorations = null;
        }

        // 다양한 컨트롤 - 항목 추가
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("항목 추가 기능", "알림");
        }

        // 다양한 컨트롤 - 항목 삭제
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("선택 항목 삭제 기능", "알림");
        }

        // 다양한 컨트롤 - Rectangle 색상 변경
        private void SetRed_Click(object sender, RoutedEventArgs e)
        {
            colorRect.Fill = Brushes.Red;
        }

        private void SetGreen_Click(object sender, RoutedEventArgs e)
        {
            colorRect.Fill = Brushes.Green;
        }

        private void SetBlue_Click(object sender, RoutedEventArgs e)
        {
            colorRect.Fill = Brushes.Blue;
        }

        private void SetRandom_Click(object sender, RoutedEventArgs e)
        {
            colorRect.Fill = new SolidColorBrush(Color.FromRgb(
                (byte)random.Next(256),
                (byte)random.Next(256),
                (byte)random.Next(256)));
        }

        // 다양한 컨트롤 - Ellipse 색상 변경
        private void SetEllipseRed_Click(object sender, RoutedEventArgs e)
        {
            colorEllipse.Fill = Brushes.Red;
        }

        private void SetEllipseGreen_Click(object sender, RoutedEventArgs e)
        {
            colorEllipse.Fill = Brushes.Green;
        }

        private void SetEllipseBlue_Click(object sender, RoutedEventArgs e)
        {
            colorEllipse.Fill = Brushes.Blue;
        }

        // 다양한 컨트롤 - 텍스트 지우기
        private void ClearText_Click(object sender, RoutedEventArgs e)
        {
            customContextTextBox.Text = "";
        }

        // 동적 ContextMenu - 코드에서 생성
        private void CreateContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var contextMenu = new ContextMenu();

            var item1 = new MenuItem { Header = "동적 항목 1" };
            item1.Click += (s, args) => MessageBox.Show("항목 1 클릭", "동적 메뉴");

            var item2 = new MenuItem { Header = "동적 항목 2" };
            item2.Click += (s, args) => MessageBox.Show("항목 2 클릭", "동적 메뉴");

            var item3 = new MenuItem { Header = "현재 시간" };
            item3.Click += (s, args) => MessageBox.Show($"시간: {DateTime.Now:HH:mm:ss}", "동적 메뉴");

            contextMenu.Items.Add(item1);
            contextMenu.Items.Add(item2);
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(item3);

            dynamicContextBorder.ContextMenu = contextMenu;
            MessageBox.Show("ContextMenu가 생성되었습니다. 영역을 우클릭하세요.", "알림");
        }

        // 동적 ContextMenu - ContextMenuOpening
        private void DynamicBorder_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // 기존 동적 항목 제거 (첫 번째 고정 항목 제외)
            while (dynamicMenu.Items.Count > 1)
            {
                dynamicMenu.Items.RemoveAt(1);
            }

            // 동적 항목 추가
            dynamicMenu.Items.Add(new Separator());
            dynamicMenu.Items.Add(new MenuItem { Header = $"현재 시간: {DateTime.Now:HH:mm:ss}" });
            dynamicMenu.Items.Add(new MenuItem { Header = $"랜덤 숫자: {random.Next(100)}" });
        }

        // 동적 ContextMenu - 조건부 메뉴
        private void ConditionalBorder_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            deleteMenuItem.IsEnabled = enableDeleteCheck.IsChecked == true;
        }

        // 동적 ContextMenu - 프로그래밍 방식으로 열기
        private void OpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (programmaticButton.ContextMenu != null)
            {
                programmaticButton.ContextMenu.PlacementTarget = programmaticButton;
                programmaticButton.ContextMenu.IsOpen = true;
            }
        }

        // 실용 예제 - 파일 열기
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (fileListBox.SelectedItem is ListBoxItem item)
            {
                MessageBox.Show($"파일 열기: {item.Content}", "열기");
            }
            else
            {
                MessageBox.Show("파일을 선택하세요.", "알림");
            }
        }

        // 실용 예제 - 파일 삭제
        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (fileListBox.SelectedItem is ListBoxItem item)
            {
                var result = MessageBox.Show($"'{item.Content}'을(를) 삭제하시겠습니까?", "삭제 확인",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    fileListBox.Items.Remove(item);
                }
            }
            else
            {
                MessageBox.Show("삭제할 파일을 선택하세요.", "알림");
            }
        }

        // 실용 예제 - 파일 속성
        private void FileProperties_Click(object sender, RoutedEventArgs e)
        {
            if (fileListBox.SelectedItem is ListBoxItem item)
            {
                MessageBox.Show($"파일: {item.Content}\n유형: 파일\n크기: 알 수 없음", "속성");
            }
            else
            {
                MessageBox.Show("파일을 선택하세요.", "알림");
            }
        }
    }
}
