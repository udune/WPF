using System.Windows.Controls;

namespace ch31_유저컨트롤.uc
{
    /// <summary>
    /// UserControl1 - 재사용 가능한 텍스트 입력 컨트롤
    /// 제목, 텍스트 입력, 글자 수 표시, 진행률 바를 포함합니다.
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        /// <summary>
        /// 제목 속성 - Label.Content에 바인딩됩니다.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 최대 글자 수 속성 - TextBox.MaxLength와 ProgressBar.Maximum에 바인딩됩니다.
        /// </summary>
        public string MaxLength { get; set; } = "100";

        /// <summary>
        /// ProgressBar 높이 속성 - ProgressBar.Height에 바인딩됩니다.
        /// </summary>
        public string PbHeight { get; set; } = "20";

        public UserControl1()
        {
            InitializeComponent();
            // DataContext를 현재 인스턴스로 설정하여 바인딩 활성화
            DataContext = this;
        }
    }
}
