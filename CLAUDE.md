# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository shape

This is a **WPF learning repository**, not a single application. It contains several independent trees that do not reference each other:

| Tree | Solution | TFM | What it is |
|---|---|---|---|
| `ch2`–`ch35` (Korean folder names, at repo root) | `HelloWPF/HelloWPF.sln` | `net8.0-windows` (ch35: `net9.0-windows`) | **The main, actively developed tree.** Book chapters rewritten into interactive tutorial apps. |
| `HelloWPF2/ch02`–`ch25` (English folder names) | `HelloWPF2/HelloWPF2.slnx` | `net10.0-windows` | A second, plain pass at the same chapters — small bare-bones examples straight from the book, no tutorial scaffolding. |
| `HelloWPF_Rider/`, `SignalR/`, `Thread/` | own `.sln` each | net9/net10 | Unrelated scratch projects. |

`HelloWPF/` itself is a near-empty starter project that exists mainly to host the solution file that aggregates the root `ch*` projects (their `.csproj` files live one level up, referenced as `..\ch2 Label\...`).

Only `ch35 MariaDB연동` (MySqlConnector) and `SignalR` have NuGet dependencies. Everything else is dependency-free.

## Build & run

Only the .NET 10 SDK is installed; it builds the net8/net9 projects fine.

```bash
# Whole main tree (35 projects)
dotnet build HelloWPF/HelloWPF.sln

# One chapter — quote the path, folder names contain spaces and Korean
dotnet build "ch21 스테이터스바/ch21 스테이터스바.csproj"
dotnet run   --project "ch21 스테이터스바/ch21 스테이터스바.csproj"

# The second tree
dotnet build HelloWPF2/HelloWPF2.slnx
```

There are no tests, linters, or CI in this repository.

Build output (`bin/`, `obj/`, `.vs/`, `.idea/`) was committed before `.gitignore` existed and was untracked in a later cleanup. The rules were always present — they simply don't apply to already-tracked files. If build artifacts ever reappear in `git status`, something re-added them; untrack rather than adding new rules.

Historical note for anyone bisecting: commits before that cleanup carry ~7,200 build artifacts, so old diffs are enormous and `obj/**/MainWindow.g.cs` exists in those trees. Checking one out and overriding `BaseIntermediateOutputPath` makes those stale generated files get globbed as compile inputs alongside fresh ones, and the build dies with hundreds of `CS0102: already contains a definition` errors.

## Tutorial format (root `ch*` projects)

Each chapter's `MainWindow.xaml` is a self-contained interactive lesson, not a minimal demo — `ch21 스테이터스바/MainWindow.xaml` is 750 lines and is the best current reference. `HelloWPF/WPF-튜토리얼-변환-명령서.md` is the spec that drives conversion of a chapter into this format, and carries a completion table that should be updated when a chapter is converted.

Structure: `Window` (`Title="ch{N} {컨트롤명} 튜토리얼"`, 600×850) → `TabControl` with 4–5 topic tabs → each `TabItem` is a `ScrollViewer > StackPanel` of `GroupBox` examples.

All 34 chapters carry both layers of scaffolding below — 514 practice blocks, at least 3 in every tab — driven by styles declared in `Window.Resources`:

**"코드 보기" (view source)** — after each example, an `Expander`/`TextBox` pair using `CodeExpanderStyle` + `CodeTextBoxStyle` showing the example's own XAML on a dark background.

**"직접 해보기" (try it yourself)** — an `Expander` (`PracticeExpanderStyle`) holding a task statement, an editable `TextBox`, and 실행/힌트/정답 보기 buttons. The wiring convention matters:

- Every practice block is identified by a `{tab}_{index}` tag, e.g. `1_2`. That tag names all its elements — `txtPractice1_2`, `txtHint1_2`, `resultBorder1_2`, `resultPanel1_2` — and is passed to the shared handlers via `Tag="1_2"`.
- Handlers `BtnRun_Click` / `BtnHint_Click` / `BtnAnswer_Click` are shared across all blocks in the file. They resolve elements at runtime with `FindName($"txtPractice{tag}")`, so **a name that doesn't follow the pattern silently does nothing**.
- 실행 feeds the user's text to `ExecuteXaml`, which injects the presentation and `x` namespaces onto the root tag before `XamlReader.Parse` and renders the result into `resultPanel{tag}`. All 34 chapters now use the same regex-based injection that works with any root element; the earlier per-chapter hardcoded form (`if (xamlCode.TrimStart().StartsWith("<StatusBar"))`) accepted only that chapter's own control and has been replaced.
- Answers live in the code-behind `_answers` dictionary keyed by the same tag. Code-behind exercises (rather than XAML ones) use a 확인 button wired to `BtnCheck_Click`, which checks `_requiredKeywords` and writes into `txtResult{tag}` instead of executing anything.
- A practice block's XAML must parse standalone — the executor declares only the presentation and `x` namespaces, so an answer referencing `local:` or a `{StaticResource}` the snippet doesn't define will throw at 실행. Make such exercises code-type instead.
- A `StringFormat` that *starts* with `{` must be escaped as `StringFormat='{}{0:F0}'`; without the leading `{}` the XAML parser reads it as a nested markup extension and throws.

### XAML authoring constraints in these files

- Line breaks inside code-display `TextBox` values **must** be `&#10;`. Literal newlines and `xml:space="preserve"` are normalized to spaces in XML attribute values.
- Escape `<` `>` `"` `&` as `&lt;` `&gt;` `&quot;` `&amp;` inside those attribute values.
- Root `ch*` projects use Korean identifiers in namespaces (`namespace ch21_스테이터스바`, `x:Class="ch21_스테이터스바.MainWindow"`), derived from the folder name via `RootNamespace`. `HelloWPF2` uses English equivalents.

## 학습 진도 추적 (`.study/`)

Every chapter app records study progress with **no per-chapter code**. `Directory.Build.targets` links `Shared/StudyRecords.cs` + `Shared/StudyTracker.cs` into each root `ch*` project (gated by `EnableStudyTracker`, which also keeps them out of `HelloWPF2/`, `SignalR`, and `Thread`). A `[ModuleInitializer]` registers class handlers on `Window.Loaded` and `Button.Click`, so tracking attaches itself to any chapter without that chapter knowing.

- Progress lands in `.study/{챕터폴더명}.json` — one file per chapter so two chapter apps running at once can't clobber each other. The path is found by walking up from `AppContext.BaseDirectory` to the directory containing `.git`; outside a clone it falls back to `%APPDATA%\WpfStudy`. `.study/` is deliberately **not** git-ignored.
- Blocks are discovered by walking the **logical** tree for `txtPractice{tag}` names. Using the visual tree would miss unselected `TabItem` contents, which WPF does not realize.
- Success is judged after the chapter's own handler runs (via `Dispatcher.BeginInvoke`) by reading what it rendered: `"성공"` in `resultPanel{tag}`, or `"정답입니다"` in `txtResult{tag}`. Completing without pressing 정답 보기 is `완료`; after peeking it is `부분완료` and counts half in the dashboard.
- Tracking is written to never break a lesson — every entry point swallows its exceptions, and a corrupt JSON file is discarded rather than surfaced.

`HelloWPF` is no longer a starter project; it is the **학습 현황판** (dashboard) that reads those files. It counts each chapter's blocks by regex over its `MainWindow.xaml`, so chapters never launched still show a correct denominator.

## ch34 MVVM

The only project with real architecture: `Models/` (POCOs) → `ViewModels/` (`PersonViewModel` exposing a list plus a command) → `Commands/PersonCommand` (hand-rolled `ICommand`, execute + canExecute delegates) → `Views/PersonView.xaml`. Everything else in the repo is code-behind driven.

## Language

UI text, task statements, hints, comments, and commit messages are **Korean**. Type and member names are English (except the Korean namespace names noted above). Commits follow `feat: ch{N} {주제}`.
