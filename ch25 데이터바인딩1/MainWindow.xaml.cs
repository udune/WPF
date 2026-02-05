using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ch25_데이터바인딩1
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        // 기본 사용법 - 배경색 변경
        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            var colors = new SolidColorBrush[]
            {
                Brushes.Salmon,
                Brushes.LightGreen,
                Brushes.LightBlue,
                Brushes.Gold,
                Brushes.LightCoral,
                Brushes.MediumPurple
            };

            colorSourceBtn.Background = colors[random.Next(colors.Length)];
        }

        // UpdateSourceTrigger - Explicit 업데이트
        private void UpdateExplicit_Click(object sender, RoutedEventArgs e)
        {
            var binding = explicitTextBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
            MessageBox.Show("소스가 업데이트되었습니다!", "알림");
        }
    }
}
