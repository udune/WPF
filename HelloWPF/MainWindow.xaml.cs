// 학습 현황판.
//
// 챕터 앱들이 .study/{챕터}.json 에 남긴 기록을 모아 보여줍니다.
// 챕터 폴더는 저장소 루트를 훑어서 찾으므로, 새 챕터가 생겨도 코드 수정이 필요 없습니다.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StudyTracking;

namespace HelloWPF
{
    /// <summary>목록 한 줄. XAML 바인딩 대상이라 public 이어야 합니다.</summary>
    public sealed class ChapterRow
    {
        public string Name { get; init; } = "";
        public string? ProjectDirectory { get; init; }
        public int Percent { get; init; }
        public int DoneCount { get; init; }
        public int PracticeTotal { get; init; }
        public int Runs { get; init; }
        public int LaunchCount { get; init; }
        public string StudyTimeText { get; init; } = "-";
        public string LastStudiedText { get; init; } = "-";

        public string PercentText => PracticeTotal > 0 ? $"{Percent}%" : "연습 없음";

        public string PracticeText => PracticeTotal > 0 ? $"{DoneCount} / {PracticeTotal}" : "-";

        public bool CanLaunch => ProjectDirectory != null;

        public Brush BarBrush => Percent >= 100
            ? new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50))
            : Percent > 0
                ? new SolidColorBrush(Color.FromRgb(0x21, 0x96, 0xF3))
                : new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC));
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (_, _) => Reload();
        }

        private void Reload()
        {
            var studyDir = StudyPaths.ResolveDirectory();
            var root = StudyPaths.ResolveRepositoryRoot();

            var records = LoadRecords(studyDir);
            var rows = BuildRows(root, records);

            lstChapters.ItemsSource = rows;

            var withPractice = rows.Where(r => r.PracticeTotal > 0).ToList();
            var totalBlocks = withPractice.Sum(r => r.PracticeTotal);
            var doneBlocks = withPractice.Sum(r => r.DoneCount);
            var studied = rows.Count(r => r.LaunchCount > 0);
            var seconds = records.Values.Sum(r => r.TotalStudySeconds);

            barOverall.Value = totalBlocks > 0 ? doneBlocks * 100.0 / totalBlocks : 0;
            txtSummary.Text =
                $"챕터 {studied}/{rows.Count}개 학습 시작   ·   " +
                $"연습 {doneBlocks}/{totalBlocks}개 완료   ·   " +
                $"누적 학습 시간 {FormatDuration(seconds)}";

            var noPractice = rows.Count(r => r.PracticeTotal == 0);
            txtFooter.Text =
                $"기록 위치: {studyDir}    (행을 더블클릭하면 해당 챕터가 실행됩니다)" +
                (noPractice > 0
                    ? $"\n'연습 없음' {noPractice}개 챕터는 '직접 해보기' 블록이 아직 만들어지지 않아 학습 시간만 기록됩니다."
                    : "");
        }

        private static Dictionary<string, ChapterRecord> LoadRecords(string studyDir)
        {
            var result = new Dictionary<string, ChapterRecord>();
            if (!Directory.Exists(studyDir)) return result;

            foreach (var file in Directory.EnumerateFiles(studyDir, "*.json"))
            {
                try
                {
                    var record = JsonSerializer.Deserialize<ChapterRecord>(
                        File.ReadAllText(file), StudyPaths.Json);
                    if (record != null && !string.IsNullOrEmpty(record.Chapter))
                        result[record.Chapter] = record;
                }
                catch
                {
                    // 손상된 기록 하나 때문에 현황판 전체가 죽지 않도록 건너뜁니다.
                }
            }

            return result;
        }

        private static List<ChapterRow> BuildRows(string? root, Dictionary<string, ChapterRecord> records)
        {
            var rows = new List<ChapterRow>();
            var seen = new HashSet<string>();

            if (root != null)
            {
                var chapters = Directory.EnumerateDirectories(root, "ch*")
                    .Where(d => Directory.EnumerateFiles(d, "*.csproj").Any())
                    .OrderBy(ChapterOrder)
                    .ToList();

                foreach (var dir in chapters)
                {
                    var name = Path.GetFileName(dir);
                    seen.Add(name);
                    rows.Add(BuildRow(name, dir, records.GetValueOrDefault(name)));
                }
            }

            // 폴더는 사라졌지만 기록만 남은 챕터도 잃어버리지 않도록 덧붙입니다.
            foreach (var pair in records.Where(p => !seen.Contains(p.Key)))
                rows.Add(BuildRow(pair.Key, null, pair.Value));

            return rows;
        }

        private static ChapterRow BuildRow(string name, string? dir, ChapterRecord? record)
        {
            // 블록 개수는 앱을 한 번 실행해야 기록에 남습니다. 아직 실행 안 한 챕터도
            // 정확한 분모를 보여주려고 XAML 을 직접 세어 둡니다.
            var declared = CountPracticeBlocks(dir);

            if (record == null)
                return new ChapterRow { Name = name, ProjectDirectory = dir, PracticeTotal = declared };

            var done = record.Practices.Values.Count(p => p.Status == StudyStatus.Done);
            var partial = record.Practices.Values.Count(p => p.Status == StudyStatus.Partial);
            var total = new[] { record.PracticeTotal, record.Practices.Count, declared }.Max();

            // 정답을 보고 맞춘 블록은 절반만 인정합니다.
            var credit = done + partial * 0.5;

            return new ChapterRow
            {
                Name = name,
                ProjectDirectory = dir,
                PracticeTotal = total,
                DoneCount = done + partial,
                Percent = total > 0 ? (int)System.Math.Round(credit * 100 / total) : 0,
                Runs = record.Practices.Values.Sum(p => p.Runs),
                LaunchCount = record.LaunchCount,
                StudyTimeText = FormatDuration(record.TotalStudySeconds),
                LastStudiedText = record.LastStudiedAt?.ToString("yyyy-MM-dd HH:mm") ?? "-",
            };
        }

        /// <summary>
        /// 챕터 XAML 에 선언된 연습 블록 수를 셉니다. 실행 이력이 없어도 분모를 알 수 있고,
        /// '아직 안 해본 챕터' 와 '연습 블록이 아예 없는 챕터' 를 구분할 수 있습니다.
        /// </summary>
        private static int CountPracticeBlocks(string? dir)
        {
            if (dir == null) return 0;

            var xaml = Path.Combine(dir, "MainWindow.xaml");
            if (!File.Exists(xaml)) return 0;

            try
            {
                return System.Text.RegularExpressions.Regex
                    .Matches(File.ReadAllText(xaml), @"x:Name=""txtPractice(\d+_\d+)""")
                    .Select(m => m.Groups[1].Value)
                    .Distinct()
                    .Count();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>"ch2" 가 "ch10" 앞에 오도록 숫자 기준으로 정렬합니다.</summary>
        private static int ChapterOrder(string dir)
        {
            var name = Path.GetFileName(dir);
            var digits = new string(name.Skip(2).TakeWhile(char.IsDigit).ToArray());
            return int.TryParse(digits, out var n) ? n : int.MaxValue;
        }

        private static string FormatDuration(long seconds)
        {
            if (seconds <= 0) return "-";
            if (seconds < 60) return $"{seconds}초";

            var minutes = seconds / 60;
            return minutes < 60 ? $"{minutes}분" : $"{minutes / 60}시간 {minutes % 60}분";
        }

        // ------------------------------------------------------------ 이벤트

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => Reload();

        private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            var dir = StudyPaths.ResolveDirectory();
            Directory.CreateDirectory(dir);
            Process.Start(new ProcessStartInfo("explorer.exe", dir) { UseShellExecute = true });
        }

        private void BtnLaunch_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: ChapterRow row }) Launch(row);
        }

        private void LstChapters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstChapters.SelectedItem is ChapterRow row) Launch(row);
        }

        private void Launch(ChapterRow row)
        {
            if (row.ProjectDirectory == null) return;

            // 챕터마다 대상 프레임워크가 달라서(net8/net9) 폴더를 고정할 수 없습니다.
            // 가장 최근에 빌드된 실행 파일을 고릅니다.
            var binDir = Path.Combine(row.ProjectDirectory, "bin");
            var exe = Directory.Exists(binDir)
                ? Directory.EnumerateFiles(binDir, "*.exe", SearchOption.AllDirectories)
                    .OrderByDescending(File.GetLastWriteTimeUtc)
                    .FirstOrDefault()
                : null;

            if (exe == null)
            {
                MessageBox.Show(
                    $"'{row.Name}' 의 실행 파일을 찾지 못했습니다.\n먼저 빌드해 주세요.",
                    "실행 파일 없음", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(exe)
                {
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(exe),
                });
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "실행 실패", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
