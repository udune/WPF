// 학습 진도 자동 저장/복원기.
//
// 루트 Directory.Build.targets 가 이 파일을 모든 ch* 챕터 프로젝트에 자동으로 링크합니다.
// 기존 챕터 코드(XAML / code-behind / csproj)는 한 줄도 수정하지 않습니다.
// Window 와 Button 의 클래스 핸들러를 전역 등록해서 기존 핸들러 위에 얹히는 방식입니다.
//
// 원칙: 추적 기능은 학습 앱의 동작을 절대 방해하지 않는다.
//       따라서 모든 진입점을 try/catch 로 감싸고, 실패해도 조용히 넘어갑니다.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace StudyTracking
{
    internal static class StudyTracker
    {
        // 연습 블록 식별자는 "{탭}_{번호}" 형식입니다. (예: 1_2)
        private static readonly Regex TagPattern = new(@"^\d+_\d+$", RegexOptions.Compiled);

        private static readonly Dictionary<string, TextBox> Inputs = new();
        private static readonly Stopwatch ActiveTime = new();

        private static ChapterRecord _record = new();
        private static string _file = "";
        private static bool _attached;
        private static bool _dirty;
        private static Window? _window;
        private static DispatcherTimer? _flushTimer;
        private static DispatcherTimer? _draftTimer;

        /// <summary>
        /// 어셈블리가 로드될 때 자동 실행됩니다. 챕터 코드에서 호출할 필요가 없습니다.
        /// </summary>
        [ModuleInitializer]
        internal static void Init()
        {
            try
            {
                EventManager.RegisterClassHandler(
                    typeof(Window), FrameworkElement.LoadedEvent,
                    new RoutedEventHandler(OnAnyWindowLoaded));

                // 챕터의 자체 Click 핸들러가 e.Handled 를 세워도 놓치지 않도록 handledEventsToo 사용.
                EventManager.RegisterClassHandler(
                    typeof(Button), ButtonBase.ClickEvent,
                    new RoutedEventHandler(OnAnyButtonClick), handledEventsToo: true);
            }
            catch
            {
                // 후킹 실패 시 추적만 비활성화되고 앱은 정상 동작합니다.
            }
        }

        // ---------------------------------------------------------------- 부착

        private static void OnAnyWindowLoaded(object sender, RoutedEventArgs e)
        {
            // 첫 번째로 열린 창(=MainWindow)만 추적합니다.
            // ch30 처럼 모달/모달리스 창을 여는 챕터에서 중복 부착되는 것을 막습니다.
            if (_attached || sender is not Window window) return;
            _attached = true;

            try { Attach(window); }
            catch { /* 추적 실패가 학습을 막지 않도록 무시 */ }
        }

        private static void Attach(Window window)
        {
            _window = window;

            var chapter = Assembly.GetEntryAssembly()?.GetName().Name
                          ?? window.GetType().Namespace
                          ?? "unknown";

            var dir = StudyPaths.ResolveDirectory();
            Directory.CreateDirectory(dir);
            _file = Path.Combine(dir, StudyPaths.Sanitize(chapter) + ".json");

            _record = Load(_file) ?? new ChapterRecord();
            _record.Chapter = chapter;
            _record.LaunchCount++;

            var now = DateTimeOffset.Now;
            _record.FirstStudiedAt ??= now;
            _record.LastStudiedAt = now;

            DiscoverPractices(window);
            RestoreState(window);

            window.Activated += (_, _) => ActiveTime.Start();
            window.Deactivated += (_, _) => { AccumulateTime(); ActiveTime.Reset(); };
            window.Closing += (_, _) => { AccumulateTime(); Save(force: true); };
            if (window.IsActive) ActiveTime.Start();

            // 크래시로 창 닫힘 이벤트를 못 받는 경우에 대비한 주기적 플러시.
            _flushTimer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromSeconds(5),
            };
            _flushTimer.Tick += (_, _) => { AccumulateTime(); Save(); };
            _flushTimer.Start();

            // 타이핑은 매 글자 저장하지 않고 2초 쉬면 저장합니다.
            _draftTimer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromSeconds(2),
            };
            _draftTimer.Tick += (_, _) => { _draftTimer!.Stop(); Save(); };

            _dirty = true;
            Save(force: true);
        }

        // ------------------------------------------------------------ 블록 탐지

        /// <summary>
        /// txtPractice{탭}_{번호} 명명 규칙을 가진 TextBox 를 모두 찾아 연습 블록으로 등록합니다.
        /// </summary>
        private static void DiscoverPractices(Window window)
        {
            const string prefix = "txtPractice";

            foreach (var element in EnumerateLogicalTree(window).OfType<TextBox>())
            {
                if (!element.Name.StartsWith(prefix, StringComparison.Ordinal)) continue;

                var tag = element.Name.Substring(prefix.Length);
                if (!TagPattern.IsMatch(tag)) continue;

                Inputs[tag] = element;
                if (!_record.Practices.ContainsKey(tag))
                    _record.Practices[tag] = new PracticeRecord();

                var captured = tag;
                element.TextChanged += (s, _) => OnDraftChanged(captured, (TextBox)s);
            }

            _record.PracticeTotal = Inputs.Count;
        }

        /// <summary>
        /// 논리 트리를 순회합니다. 시각 트리를 쓰면 선택되지 않은 TabItem 안의 요소를
        /// 놓치므로(TabControl 이 비선택 탭을 렌더링하지 않음) 반드시 논리 트리를 씁니다.
        /// </summary>
        private static IEnumerable<DependencyObject> EnumerateLogicalTree(DependencyObject root)
        {
            var pending = new Stack<DependencyObject>();
            pending.Push(root);

            while (pending.Count > 0)
            {
                var current = pending.Pop();
                yield return current;

                foreach (var child in LogicalTreeHelper.GetChildren(current).OfType<DependencyObject>())
                    pending.Push(child);
            }
        }

        // -------------------------------------------------------------- 복원

        private static void RestoreState(Window window)
        {
            foreach (var pair in Inputs)
            {
                var record = _record.Practices[pair.Key];
                if (!string.IsNullOrEmpty(record.Draft))
                    pair.Value.Text = record.Draft;

                MarkStatus(pair.Value, record.Status);
            }

            var tabs = EnumerateLogicalTree(window).OfType<TabControl>().FirstOrDefault();
            if (tabs == null) return;

            if (_record.LastTab > 0 && _record.LastTab < tabs.Items.Count)
                tabs.SelectedIndex = _record.LastTab;

            tabs.SelectionChanged += (s, e) =>
            {
                // 내부 컨트롤(ComboBox 등)의 SelectionChanged 가 올라온 것은 무시합니다.
                if (!ReferenceEquals(e.OriginalSource, s)) return;
                _record.LastTab = ((TabControl)s).SelectedIndex;
                _dirty = true;
            };
        }

        /// <summary>연습 블록을 감싼 Expander 헤더에 완료 표시를 붙입니다.</summary>
        private static void MarkStatus(DependencyObject from, string status)
        {
            if (FindAncestorExpander(from) is not { } expander) return;

            expander.Header = status switch
            {
                StudyStatus.Done => "직접 해보기   ✓ 완료",
                StudyStatus.Partial => "직접 해보기   △ 정답 확인함",
                _ => "직접 해보기",
            };
        }

        private static Expander? FindAncestorExpander(DependencyObject from)
        {
            var current = LogicalTreeHelper.GetParent(from);
            while (current != null)
            {
                if (current is Expander expander) return expander;
                current = LogicalTreeHelper.GetParent(current);
            }
            return null;
        }

        // ------------------------------------------------------------ 버튼 후킹

        private static void OnAnyButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_attached || sender is not Button button) return;
            if (button.Tag is not string tag || !TagPattern.IsMatch(tag)) return;
            if (!_record.Practices.TryGetValue(tag, out var record)) return;

            try
            {
                switch ((button.Content as string)?.Trim())
                {
                    case "실행":
                    case "확인":
                        record.Runs++;
                        // 클래스 핸들러는 챕터의 인스턴스 핸들러보다 먼저 실행됩니다.
                        // 성공 여부는 챕터 핸들러가 결과를 그린 뒤에 확인해야 합니다.
                        button.Dispatcher.BeginInvoke(
                            DispatcherPriority.Background,
                            new Action(() => EvaluateResult(tag, record)));
                        break;

                    case "힌트":
                        record.Hints++;
                        break;

                    case "정답 보기":
                        record.Answers++;
                        break;

                    default:
                        return;
                }

                _dirty = true;
                Save();
            }
            catch { /* 무시 */ }
        }

        private static void EvaluateResult(string tag, PracticeRecord record)
        {
            try
            {
                if (IsSuccess(tag) != true) { Save(); return; }

                record.Successes++;

                if (record.Status != StudyStatus.Done)
                {
                    // 정답을 먼저 본 뒤 성공한 경우는 "부분완료" 로 남깁니다.
                    record.Status = record.Answers > 0 ? StudyStatus.Partial : StudyStatus.Done;
                    record.CompletedAt ??= DateTimeOffset.Now;

                    if (Inputs.TryGetValue(tag, out var input))
                        MarkStatus(input, record.Status);
                }

                _dirty = true;
                Save();
            }
            catch { /* 무시 */ }
        }

        /// <summary>
        /// 챕터가 그린 결과를 읽어 성공 여부를 판정합니다.
        /// XAML 실행형은 resultPanel{tag} 에 "성공..." TextBlock 을,
        /// 코드 비교형은 txtResult{tag} 에 "정답입니다..." 를 씁니다.
        /// </summary>
        private static bool? IsSuccess(string tag)
        {
            if (_window == null) return null;

            if (_window.FindName("resultPanel" + tag) is Panel panel)
                return panel.Children.OfType<TextBlock>().Any(t => t.Text.Contains("성공"));

            if (_window.FindName("txtResult" + tag) is TextBlock result)
                return result.Text.Contains("정답입니다");

            return null;
        }

        // ------------------------------------------------------------ 저장/적재

        private static void OnDraftChanged(string tag, TextBox input)
        {
            if (!_record.Practices.TryGetValue(tag, out var record)) return;

            record.Draft = input.Text;
            _dirty = true;

            _draftTimer?.Stop();
            _draftTimer?.Start();
        }

        private static void AccumulateTime()
        {
            if (!ActiveTime.IsRunning) return;

            var seconds = (long)ActiveTime.Elapsed.TotalSeconds;
            if (seconds <= 0) return;

            _record.TotalStudySeconds += seconds;
            _record.LastStudiedAt = DateTimeOffset.Now;
            _dirty = true;
            ActiveTime.Restart();
        }

        private static ChapterRecord? Load(string path)
        {
            try
            {
                return File.Exists(path)
                    ? JsonSerializer.Deserialize<ChapterRecord>(File.ReadAllText(path), StudyPaths.Json)
                    : null;
            }
            catch
            {
                // 손상된 기록은 버리고 새로 시작합니다. 학습을 막는 것보다 낫습니다.
                return null;
            }
        }

        private static void Save(bool force = false)
        {
            if (!force && !_dirty) return;
            if (string.IsNullOrEmpty(_file)) return;

            try
            {
                // 저장 도중 앱이 죽어도 기존 기록이 깨지지 않도록 임시 파일에 쓰고 교체합니다.
                var temp = _file + ".tmp";
                File.WriteAllText(temp, JsonSerializer.Serialize(_record, StudyPaths.Json));
                File.Move(temp, _file, overwrite: true);
                _dirty = false;
            }
            catch { /* 디스크 문제로 학습이 중단되지 않도록 무시 */ }
        }


    }
}
