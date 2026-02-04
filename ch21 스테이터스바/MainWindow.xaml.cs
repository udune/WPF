using System.Windows;
using System.Windows.Controls;

namespace ch21_스테이터스바;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // 슬라이더 값 변경 시 상태바 업데이트
    private void sl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (sl.Value == 100)
            sb.Content = "완료";
        else
            sb.Content = "로딩중..";

        pb.Value = sl.Value;
    }
}