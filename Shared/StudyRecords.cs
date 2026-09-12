// 학습 진도 기록의 데이터 모델과 저장 위치 규칙.
//
// 챕터 앱(StudyTracker)과 학습 현황판(HelloWPF)이 함께 쓰는 파일이라
// 여기에는 WPF 의존성이나 후킹 로직을 넣지 않습니다. 순수 데이터만 둡니다.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace StudyTracking
{
    internal static class StudyStatus
    {
        public const string Todo = "미완료";
        public const string Partial = "부분완료";
        public const string Done = "완료";
    }

    /// <summary>연습 블록 하나("직접 해보기" Expander 하나)의 기록.</summary>
    internal sealed class PracticeRecord
    {
        public int Runs { get; set; }
        public int Successes { get; set; }
        public int Hints { get; set; }
        public int Answers { get; set; }
        public string Status { get; set; } = StudyStatus.Todo;
        public string? Draft { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }

    /// <summary>챕터 하나의 학습 기록. .study/{챕터명}.json 에 그대로 직렬화됩니다.</summary>
    internal sealed class ChapterRecord
    {
        public string Chapter { get; set; } = "";
        public DateTimeOffset? FirstStudiedAt { get; set; }
        public DateTimeOffset? LastStudiedAt { get; set; }
        public long TotalStudySeconds { get; set; }
        public int LaunchCount { get; set; }
        public int LastTab { get; set; }
        public int PracticeTotal { get; set; }
        public Dictionary<string, PracticeRecord> Practices { get; set; } = new();
    }

    internal static class StudyPaths
    {
        public static readonly JsonSerializerOptions Json = new()
        {
            WriteIndented = true,
            // 한글이 \uXXXX 로 이스케이프되면 사람이 읽을 수 없으므로 해제합니다.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        /// <summary>
        /// 실행 파일은 bin/Debug/{tfm}/ 에서 돌기 때문에 .git 이 있는 저장소 루트까지
        /// 거슬러 올라가 .study/ 를 찾습니다. 저장소 밖에서 실행되면 %APPDATA% 로 폴백합니다.
        /// </summary>
        public static string ResolveDirectory()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, ".git")))
                    return Path.Combine(dir.FullName, ".study");
                dir = dir.Parent;
            }

            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "WpfStudy");
        }

        /// <summary>저장소 루트. 현황판이 챕터 폴더를 훑을 때 씁니다.</summary>
        public static string? ResolveRepositoryRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, ".git")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return null;
        }

        public static string Sanitize(string name)
        {
            foreach (var invalid in Path.GetInvalidFileNameChars())
                name = name.Replace(invalid, '_');
            return name;
        }
    }
}
