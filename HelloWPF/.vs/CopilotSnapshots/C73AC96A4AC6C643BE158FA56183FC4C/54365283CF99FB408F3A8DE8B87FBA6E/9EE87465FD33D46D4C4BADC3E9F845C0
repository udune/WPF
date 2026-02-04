using ch34_MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ch34_MVVM.Views
{
    /// <summary>
    /// PersonView.xaml에 대한 상호 작용 논리
    /// MVVM 패턴의 View 코드 비하인드입니다.
    /// DataContext 설정 외에는 최소한의 코드만 포함합니다.
    /// </summary>
    public partial class PersonView : Page
    {
        /*
         * ========== MVVM의 코드 비하인드 원칙 ==========
         * 
         * 최소화:
         * - 비즈니스 로직 없음
         * - UI 로직만 (필요 시)
         * - DataContext 설정
         * - 초기화 코드
         * 
         * 피해야 할 것:
         * - 데이터 처리
         * - 비즈니스 로직
         * - 이벤트 핸들러 (가능한 Command 사용)
         * 
         * 허용되는 것:
         * - DataContext 설정
         * - 애니메이션 (순수 UI)
         * - 포커스 관리
         * - 스크롤 위치
         * - ViewModel에 전달할 수 없는 UI 전용 로직
         */
        
        /// <summary>
        /// PersonView 생성자
        /// View를 초기화하고 ViewModel과 연결합니다.
        /// </summary>
        public PersonView()
        {
            /*
             * ========== 생성자 실행 순서 ==========
             * 
             * 1. InitializeComponent():
             *    - XAML 파싱 및 로드
             *    - UI 요소 생성 (Grid, ListView, TextBox, Button)
             *    - 바인딩 표현식 파싱 (아직 평가 안 됨)
             *    - 이벤트 핸들러 등록
             *    - 리소스 로드
             * 
             * 2. DataContext 설정:
             *    - PersonViewModel 인스턴스 생성
             *    - ViewModel 생성자 실행:
             *      - PersonList 초기화
             *      - PersonCommand 생성
             *    - DataContext에 ViewModel 할당
             *    - 모든 바인딩 평가 시작
             *      - {Binding PersonList} → PersonViewModel.PersonList
             *      - {Binding PersonCommand} → PersonViewModel.PersonCommand
             *    - ListView에 데이터 표시
             * 
             * 3. Loaded 이벤트:
             *    - View가 화면에 표시된 후
             *    - 이후 처리 가능
             */
            InitializeComponent();
            /*
             * InitializeComponent():
             * - 자동 생성된 메서드
             * - PersonView.g.cs (generated) 파일에 정의
             * - obj/Debug 폴더에 위치
             * - XAML을 C# 코드로 변환한 결과
             * 
             * 생성 내용:
             * - UI 요소 인스턴스화
             * - 속성 설정
             * - 이벤트 연결
             * - 리소스 로드
             * - 바인딩 설정 (표현식만, 아직 평가 안 됨)
             * 
             * 예 (생성된 코드):
             * private void InitializeComponent()
             * {
             *     // XAML 리소스 로드
             *     System.Uri resourceLocator = new System.Uri("/ch34 MVVM;component/views/personview.xaml", System.UriKind.Relative);
             *     System.Windows.Application.LoadComponent(this, resourceLocator);
             * }
             */
            
            DataContext = new PersonViewModel();
            /*
             * DataContext 설정:
             * - View와 ViewModel 연결의 핵심
             * - 모든 바인딩의 기본 소스
             * 
             * DataContext란?:
             * - DependencyObject의 속성
             * - 바인딩 표현식의 기본 소스
             * - 자식 요소로 상속
             * - null 가능
             * 
             * 바인딩 해석:
             * {Binding PersonList}
             * 1. 현재 요소의 DataContext 확인
             * 2. PersonViewModel 발견
             * 3. PersonViewModel.PersonList 접근
             * 4. ListView.ItemsSource에 할당
             * 
             * DataContext 상속:
             * Page (DataContext = PersonViewModel)
             *   ↓
             * Grid (DataContext = PersonViewModel, 상속)
             *   ↓
             * ListView (DataContext = PersonViewModel, 상속)
             *   ↓
             * ItemsSource = {Binding PersonList}
             *   → PersonViewModel.PersonList
             * 
             * ElementName 바인딩은 DataContext 무시:
             * {Binding ElementName=lv, Path=SelectedItem.Name}
             * - lv 요소를 직접 참조
             * - DataContext 무관
             * 
             * new PersonViewModel():
             * - ViewModel 인스턴스 생성
             * - 생성자 실행:
             *   - PersonList 초기화
             *   - PersonCommand 생성
             * 
             * 대안 (Dependency Injection):
             * private readonly PersonViewModel _viewModel;
             * 
             * public PersonView(PersonViewModel viewModel)
             * {
             *     InitializeComponent();
             *     _viewModel = viewModel;
             *     DataContext = _viewModel;
             * }
             * 
             * 장점:
             * - 테스트 용이
             * - 느슨한 결합
             * - Mock ViewModel 주입 가능
             * 
             * 사용 (DI 컨테이너):
             * services.AddTransient<PersonViewModel>();
             * services.AddTransient<PersonView>();
             * 
             * var view = serviceProvider.GetRequiredService<PersonView>();
             * 
             * XAML에서 DataContext 설정 (대안):
             * <Page.DataContext>
             *     <vm:PersonViewModel/>
             * </Page.DataContext>
             * 
             * 단점:
             * - 디자인 타임에도 인스턴스 생성
             * - 생성자 매개변수 전달 어려움
             * 
             * 디자인 타임 DataContext:
             * <Page d:DataContext="{d:DesignInstance Type=vm:PersonViewModel, IsDesignTimeCreatable=True}">
             * 
             * 장점:
             * - Visual Studio 디자이너에서 바인딩 IntelliSense
             * - 디자인 타임 데이터 표시
             * - 실제 ViewModel은 코드에서 설정
             */
        }
        
        /*
         * ========== 코드 비하인드 최소화 예제 ==========
         * 
         * 나쁜 예 (MVVM 위반):
         * 
         * private void Button_Click(object sender, RoutedEventArgs e)
         * {
         *     MessageBox.Show(tBox1.Text);
         * }
         * 
         * 문제:
         * - UI 요소 직접 참조 (tBox1)
         * - 비즈니스 로직 (MessageBox)
         * - 테스트 불가
         * 
         * 좋은 예 (MVVM):
         * 
         * XAML:
         * <Button Command="{Binding ShowMessageCommand}"
         *         CommandParameter="{Binding Text, ElementName=tBox1}"/>
         * 
         * ViewModel:
         * public ICommand ShowMessageCommand { get; }
         * ShowMessageCommand = new RelayCommand<string>(text => MessageBox.Show(text));
         * 
         * ========== 허용되는 코드 비하인드 ==========
         * 
         * 1. 애니메이션 (순수 UI):
         * private void Page_Loaded(object sender, RoutedEventArgs e)
         * {
         *     var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
         *     this.BeginAnimation(OpacityProperty, fadeIn);
         * }
         * 
         * 2. 포커스 관리:
         * private void Page_Loaded(object sender, RoutedEventArgs e)
         * {
         *     tBox1.Focus();
         * }
         * 
         * 3. 스크롤 위치:
         * private void ScrollToSelectedItem(object sender, SelectionChangedEventArgs e)
         * {
         *     if (lv.SelectedItem != null)
         *     {
         *         lv.ScrollIntoView(lv.SelectedItem);
         *     }
         * }
         * 
         * 4. UI 전용 로직 (ViewModel에 전달 불가):
         * private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
         * {
         *     // 숫자만 입력 허용
         *     e.Handled = !int.TryParse(e.Text, out _);
         * }
         * 
         * 5. 드래그 앤 드롭:
         * private void ListView_Drop(object sender, DragEventArgs e)
         * {
         *     if (e.Data.GetDataPresent(DataFormats.FileDrop))
         *     {
         *         var files = (string[])e.Data.GetData(DataFormats.FileDrop);
         *         var vm = DataContext as PersonViewModel;
         *         vm?.LoadFilesCommand.Execute(files);
         *     }
         * }
         * 
         * ========== Attached Behavior (대안) ==========
         * 
         * 코드 비하인드를 피하는 방법:
         * 
         * public static class FocusBehavior
         * {
         *     public static readonly DependencyProperty FocusOnLoadProperty =
         *         DependencyProperty.RegisterAttached(
         *             "FocusOnLoad",
         *             typeof(bool),
         *             typeof(FocusBehavior),
         *             new PropertyMetadata(false, OnFocusOnLoadChanged));
         *     
         *     public static bool GetFocusOnLoad(DependencyObject obj)
         *     {
         *         return (bool)obj.GetValue(FocusOnLoadProperty);
         *     }
         *     
         *     public static void SetFocusOnLoad(DependencyObject obj, bool value)
         *     {
         *         obj.SetValue(FocusOnLoadProperty, value);
         *     }
         *     
         *     private static void OnFocusOnLoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
         *     {
         *         if ((bool)e.NewValue && d is FrameworkElement element)
         *         {
         *             element.Loaded += (s, args) => element.Focus();
         *         }
         *     }
         * }
         * 
         * XAML:
         * <TextBox local:FocusBehavior.FocusOnLoad="True"/>
         * 
         * ========== EventToCommand (대안) ==========
         * 
         * System.Windows.Interactivity 사용:
         * 
         * <TextBox>
         *     <i:Interaction.Triggers>
         *         <i:EventTrigger EventName="TextChanged">
         *             <i:InvokeCommandAction Command="{Binding TextChangedCommand}"
         *                                   CommandParameter="{Binding Text, RelativeSource={RelativeSource AncestorType=TextBox}}"/>
         *         </i:EventTrigger>
         *     </i:Interaction.Triggers>
         * </TextBox>
         * 
         * ViewModel:
         * public ICommand TextChangedCommand { get; }
         * TextChangedCommand = new RelayCommand<string>(text => FilterData(text));
         * 
         * ========== View와 ViewModel 통신 ==========
         * 
         * 1. Command (일반적):
         * XAML: Command="{Binding SaveCommand}"
         * ViewModel: public ICommand SaveCommand { get; }
         * 
         * 2. 속성 바인딩:
         * XAML: Text="{Binding SearchText, Mode=TwoWay}"
         * ViewModel: public string SearchText { get; set; }
         * 
         * 3. Messenger 패턴:
         * ViewModel: Messenger.Send(new PersonUpdatedMessage(person));
         * View: Messenger.Register<PersonUpdatedMessage>(this, HandleMessage);
         * 
         * 4. Event Aggregator:
         * ViewModel: _eventAggregator.Publish(new PersonUpdatedEvent(person));
         * View: _eventAggregator.Subscribe(HandlePersonUpdated);
         * 
         * ========== 테스트 ==========
         * 
         * 코드 비하인드 최소화의 장점:
         * 
         * ViewModel 테스트 (쉬움):
         * [Fact]
         * public void ShowMessage_WithValidText_DisplaysMessage()
         * {
         *     // Arrange
         *     var viewModel = new PersonViewModel();
         *     var text = "테스트";
         *     
         *     // Act
         *     viewModel.PersonCommand.Execute(text);
         *     
         *     // Assert
         *     // MessageBox 대신 IDialogService Mock 사용
         * }
         * 
         * View 테스트 (어려움):
         * - UI Automation 필요
         * - 느림
         * - 불안정
         * 
         * ========== 주의사항 ==========
         * 
         * 1. DataContext 타이밍:
         *    - InitializeComponent() 후 설정
         *    - 바인딩 전에 설정
         * 
         * 2. Memory Leak:
         *    - ViewModel에서 View 참조 금지
         *    - 이벤트 핸들러 해제
         * 
         * 3. ViewModel 재사용:
         *    - 여러 View에서 같은 ViewModel
         *    - Singleton vs Transient
         * 
         * 4. Navigation:
         *    - Frame.Navigate(new PersonView());
         *    - ViewModel은 NavigationService에서 주입
         */
    }
}
