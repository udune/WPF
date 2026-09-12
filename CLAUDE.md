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

Two layers of scaffolding appear in every converted chapter, both driven by styles declared in `Window.Resources`:

**"코드 보기" (view source)** — after each example, an `Expander`/`TextBox` pair using `CodeExpanderStyle` + `CodeTextBoxStyle` showing the example's own XAML on a dark background.

**"직접 해보기" (try it yourself)** — an `Expander` (`PracticeExpanderStyle`) holding a task statement, an editable `TextBox`, and 실행/힌트/정답 보기 buttons. The wiring convention matters:

- Every practice block is identified by a `{tab}_{index}` tag, e.g. `1_2`. That tag names all its elements — `txtPractice1_2`, `txtHint1_2`, `resultBorder1_2`, `resultPanel1_2` — and is passed to the shared handlers via `Tag="1_2"`.
- Handlers `BtnRun_Click` / `BtnHint_Click` / `BtnAnswer_Click` are shared across all blocks in the file. They resolve elements at runtime with `FindName($"txtPractice{tag}")`, so **a name that doesn't follow the pattern silently does nothing**.
- 실행 feeds the user's text to `ExecuteXaml`, which injects the presentation `xmlns` onto the root tag before `XamlReader.Parse` and renders the result into `resultPanel{tag}`. The injection is per-chapter and hardcoded to that chapter's root control (`if (xamlCode.TrimStart().StartsWith("<StatusBar"))`) — adapt it when porting.
- Answers live in the code-behind `_answers` dictionary keyed by the same tag. Code-behind exercises (rather than XAML ones) are checked by `BtnCheck_Click` against `_requiredKeywords` instead of being executed.

### XAML authoring constraints in these files

- Line breaks inside code-display `TextBox` values **must** be `&#10;`. Literal newlines and `xml:space="preserve"` are normalized to spaces in XML attribute values.
- Escape `<` `>` `"` `&` as `&lt;` `&gt;` `&quot;` `&amp;` inside those attribute values.
- Root `ch*` projects use Korean identifiers in namespaces (`namespace ch21_스테이터스바`, `x:Class="ch21_스테이터스바.MainWindow"`), derived from the folder name via `RootNamespace`. `HelloWPF2` uses English equivalents.

## ch34 MVVM

The only project with real architecture: `Models/` (POCOs) → `ViewModels/` (`PersonViewModel` exposing a list plus a command) → `Commands/PersonCommand` (hand-rolled `ICommand`, execute + canExecute delegates) → `Views/PersonView.xaml`. Everything else in the repo is code-behind driven.

## Language

UI text, task statements, hints, comments, and commit messages are **Korean**. Type and member names are English (except the Korean namespace names noted above). Commits follow `feat: ch{N} {주제}`.
