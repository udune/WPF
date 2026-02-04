# WPF 튜토리얼 프로젝트 변환 명령서

이 파일의 경로를 Claude Code에 붙여넣으면 자동으로 아래 작업을 수행합니다.

---

## 지시사항

아래 지시를 읽고 즉시 실행하세요. 사용자에게 추가 확인을 묻지 말고 바로 진행합니다.

### 수행할 작업

1. 사용자가 지정한 `ch*` 프로젝트 폴더(`C:\Users\udune\Desktop\WPF\` 하위)를 확인합니다.
2. 해당 프로젝트의 `MainWindow.xaml`을 읽고, 이미 튜토리얼 형식(TabControl + Expander "코드 보기")으로 변환되었는지 확인합니다.
3. **아직 변환되지 않은 경우** 아래 규칙에 따라 변환합니다.
4. 완료 후 이 파일 하단의 "변환 완료 목록" 테이블을 업데이트합니다.

> **참고**: 이 명령서를 붙여넣을 때 변환할 프로젝트를 함께 지정하세요. (예: "ch4 버튼 변환해줘")

---

## 변환 규칙

### 1. Window 설정
- Title: `"ch{N} {컨트롤명} 튜토리얼"`
- `Height="600" Width="850"`

### 2. Window.Resources에 공통 스타일 2개 정의

```xml
<Style x:Key="CodeExpanderStyle" TargetType="Expander">
    <Setter Property="Header" Value="코드 보기"/>
    <Setter Property="Margin" Value="0,0,0,12"/>
    <Setter Property="IsExpanded" Value="False"/>
</Style>
<Style x:Key="CodeTextBoxStyle" TargetType="TextBox">
    <Setter Property="IsReadOnly" Value="True"/>
    <Setter Property="FontFamily" Value="Consolas"/>
    <Setter Property="FontSize" Value="12"/>
    <Setter Property="Background" Value="#1E1E1E"/>
    <Setter Property="Foreground" Value="#DCDCDC"/>
    <Setter Property="Padding" Value="10"/>
    <Setter Property="TextWrapping" Value="Wrap"/>
    <Setter Property="AcceptsReturn" Value="True"/>
    <Setter Property="VerticalScrollBarVisibility" Value="Auto"/>
    <Setter Property="MaxHeight" Value="300"/>
    <Setter Property="BorderBrush" Value="#333333"/>
    <Setter Property="BorderThickness" Value="1"/>
</Style>
```

### 3. TabControl 구성
- 해당 컨트롤의 주요 속성/기능을 4~5개 탭으로 분류
- 각 TabItem 안에 `ScrollViewer > StackPanel` 구조
- 섹션 제목: `TextBlock FontSize="18" FontWeight="Bold"`
- 예제는 GroupBox 또는 직접 배치

### 4. 탭 구성 가이드 (컨트롤별 조정)
- **기본 사용법**: 콘텐츠 지정 방법(태그, 속성, 코드비하인드), 컨트롤 고유 기능
- **레이아웃/정렬**: 정렬, 배치 관련 속성
- **폰트와 스타일**: FontSize, FontWeight, FontStyle, FontFamily, TextDecorations
- **색상과 외관**: Background, Foreground, Opacity, Padding, Margin, Border 등
- **상호작용/고급**: 이벤트, 코드비하인드 제어, Visibility 등

### 5. 코드 보기 Expander 규칙
- 모든 예제/섹션 아래에 Expander 추가
- `Expander Style="{StaticResource CodeExpanderStyle}"`
- 내부에 `TextBox Style="{StaticResource CodeTextBoxStyle}"`
- **줄바꿈은 반드시 `&#10;` 사용** (`xml:space="preserve"`나 실제 줄바꿈은 XML 속성값에서 공백으로 정규화되므로 사용 금지)
- XML 특수문자 이스케이프: `<` → `&lt;`, `>` → `&gt;`, `"` → `&quot;`, `&` → `&amp;`
- 코드 비하인드 예제는 XAML + C# 코드를 모두 포함

### 6. 코드 비하인드 (MainWindow.xaml.cs)
- 기존 이벤트 핸들러 유지
- 필요 시 동적 텍스트 변경, 동적 스타일 변경 이벤트 추가
- `using System.Windows.Media;` 추가 (Brushes 사용 시)

### 7. 검증
- 각 탭의 Expander 코드가 줄바꿈 포함 올바르게 표시되는지 확인
- 코드 비하인드 버튼이 정상 동작하는지 확인

---

## 변환 완료 목록

| 프로젝트 | 컨트롤 | 탭 수 | 상태 |
|---------|--------|-------|------|
| ch2 Label | Label | 5 | 완료 |
| ch3 텍스트블럭 | TextBlock | 5 | 완료 |
| ch4 버튼 | Button | 5 | 완료 |
| ch10 캘린더 | Calendar | 5 | 완료 |
| ch11 데이트피커 | DatePicker | 5 | 완료 |
| ch13 익스팬더 | Expander | 5 | 완료 |
| ch14 스택패널 | StackPanel | 5 | 완료 |
| ch15 랩패널 | WrapPanel | 5 | 완료 |
| ch16 독패널 | DockPanel | 5 | 완료 |
| ch17 그리드 | Grid | 5 | 완료 |
| ch18 유니폼그리드 | UniformGrid | 5 | 완료 |
| ch19 캔버스 | Canvas | 5 | 완료 |
| ch20 프로그레스바 | ProgressBar | 5 | 완료 |
| ch21 스테이터스바 | StatusBar | 5 | 완료 |
