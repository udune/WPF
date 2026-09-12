# WPF 튜토리얼 프로젝트 변환 명령서

이 파일의 경로를 Claude Code에 붙여넣으면 자동으로 아래 작업을 수행합니다.

---

## 지시사항

아래 지시를 읽고 즉시 실행하세요. 사용자에게 추가 확인을 묻지 말고 바로 진행합니다.

### 수행할 작업

1. 사용자가 지정한 `ch*` 프로젝트 폴더(`D:\Projects\WPF\WPF\` 하위)를 확인합니다.
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

### 7. 직접 해보기 블록 (필수)

각 탭마다 연습 블록을 2개씩 넣습니다. **이름 규칙이 학습 진도 추적의 전제**이므로 반드시 지킵니다.

- 식별자는 `{탭번호}_{순번}` (예: `1_2`). 이 값이 블록의 모든 이름에 들어갑니다.
  - `x:Name="txtPractice1_2"` (입력창), `txtHint1_2` (힌트), `resultBorder1_2` / `resultPanel1_2` (실행 결과)
  - 코드 비교형은 결과 표시가 `txtResult1_2` 하나뿐입니다.
  - 버튼에는 `Tag="1_2"` 를 줍니다.
- 공용 스타일 5개를 `Window.Resources` 에 정의: `PracticeExpanderStyle`, `PracticeInputStyle`,
  `RunButtonStyle`, `HintButtonStyle`, `AnswerButtonStyle`
- 버튼 구성
  - XAML 실행형: `실행`(BtnRun_Click) / `힌트`(BtnHint_Click) / `정답 보기`(BtnAnswer_Click)
  - 코드 비교형: `확인`(BtnCheck_Click) / `힌트` / `정답 보기`
- 코드 비하인드에 `_answers`(태그→정답)와 `_requiredKeywords`(코드형 채점용) 딕셔너리를 둡니다.
- **XAML 실행형 정답과 템플릿은 단독으로 파싱돼야 합니다.** 실행기는 presentation 과 `x` 네임스페이스만
  붙여 주므로, `local:` 접두사나 스니펫 안에 없는 `{StaticResource}` 를 참조하면 실행 시 오류가 납니다.
  그런 주제는 코드 비교형으로 만드세요.

### 8. 검증
- 각 탭의 Expander 코드가 줄바꿈 포함 올바르게 표시되는지 확인
- 코드 비하인드 버튼이 정상 동작하는지 확인
- 연습 블록의 정답을 실제로 실행해 오류 없이 렌더링되는지 확인
- 앱을 한 번 실행한 뒤 `.study/{챕터폴더명}.json` 의 `PracticeTotal` 이 실제 블록 수와 맞는지 확인

---

## 변환 완료 목록

루트 `ch*` 34개 프로젝트 전부 변환 완료입니다. "연습 블록"은 직접 해보기 개수이며,
모든 탭이 최소 3개를 갖도록 맞춰져 있습니다.
(ch28/ch29/ch30 은 1번 탭이 실습 화면이라 2~5번 탭에만 블록이 있습니다.)

| 프로젝트 | 탭 수 | 연습 블록 | 상태 |
|---------|-------|-----------|------|
| ch2 Label | 5 | 19 | 완료 |
| ch3 텍스트블럭 | 5 | 17 | 완료 |
| ch4 버튼 | 5 | 17 | 완료 |
| ch5 텍스트박스 | 5 | 16 | 완료 |
| ch6 패스워드박스 | 5 | 15 | 완료 |
| ch7 이미지 | 5 | 15 | 완료 |
| ch8 체크박스 | 5 | 15 | 완료 |
| ch9 라디오버튼 | 5 | 17 | 완료 |
| ch10 캘린더 | 5 | 15 | 완료 |
| ch11 데이트피커 | 5 | 15 | 완료 |
| ch12 슬라이더 | 5 | 17 | 완료 |
| ch13 익스팬더 | 5 | 15 | 완료 |
| ch14 스택패널 | 5 | 15 | 완료 |
| ch15 랩패널 | 5 | 15 | 완료 |
| ch16 독패널 | 5 | 15 | 완료 |
| ch17 그리드 | 5 | 15 | 완료 |
| ch18 유니폼그리드 | 5 | 15 | 완료 |
| ch19 캔버스 | 5 | 15 | 완료 |
| ch20 프로그레스바 | 5 | 15 | 완료 |
| ch21 스테이터스바 | 5 | 15 | 완료 |
| ch22 툴바 | 5 | 15 | 완료 |
| ch23 메뉴 | 5 | 15 | 완료 |
| ch24 컨텍스트메뉴 | 5 | 15 | 완료 |
| ch25 데이터바인딩1 | 5 | 15 | 완료 |
| ch26 데이터바인딩2 | 5 | 15 | 완료 |
| ch27 데이터바인딩3 | 5 | 15 | 완료 |
| ch28 영어단어맞추기 | 4 | 12 | 완료 |
| ch29 화면이동 프레임 | 4 | 12 | 완료 |
| ch30 탭컨트롤 모달 모달리스 | 4 | 12 | 완료 |
| ch31 유저컨트롤 | 5 | 15 | 완료 |
| ch32 스타일 | 5 | 15 | 완료 |
| ch33 애니메이션 | 5 | 15 | 완료 |
| ch34 MVVM | 5 | 15 | 완료 |
| ch35 MariaDB연동 | 5 | 15 | 완료 |

합계 514개 블록.
