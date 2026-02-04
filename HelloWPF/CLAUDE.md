# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

This is a .NET 8.0 WPF solution. The solution file is `HelloWPF.sln` in this directory.

```bash
# Build the entire solution
dotnet build HelloWPF.sln

# Build a specific chapter project
dotnet build "../ch34 MVVM/ch34 MVVM.csproj"

# Run a specific project
dotnet run --project "../ch34 MVVM/ch34 MVVM.csproj"
```

There are no test projects or linting tools configured.

## Architecture

This is a WPF tutorial solution with 35+ chapter projects under `C:\Users\udune\Desktop\WPF\`. Each `ch*` folder is an independent WPF application targeting `net8.0-windows` with `<UseWPF>true</UseWPF>`. No external NuGet dependencies.

**Project types:**
- **ch2–ch24**: Individual WPF control tutorials (Label, Button, Grid, Menu, etc.) — each has `MainWindow.xaml` + code-behind
- **ch25–ch27**: Data binding tutorials
- **ch28–ch33**: Applied topics (game, navigation, TabControl/modals, UserControl, Styles, Animation)
- **ch34 MVVM**: Full MVVM pattern demo with `Models/`, `ViewModels/`, `Commands/`, `Views/` folders implementing `INotifyPropertyChanged` and `ICommand`
- **ch35 MariaDB**: Database integration (excluded from main build)

## Tutorial Conversion Convention

The file `WPF-튜토리얼-변환-명령서.md` defines the standard format for converting chapter projects into a tutorial layout. Key rules:

- Window: `Title="ch{N} {컨트롤명} 튜토리얼"`, `Height="600" Width="850"`
- Content structure: `TabControl` with 4–5 tabs, each containing `ScrollViewer > StackPanel`
- Every example has an `Expander` ("코드 보기") with a dark-themed read-only `TextBox` showing the source
- Two shared styles in `Window.Resources`: `CodeExpanderStyle` and `CodeTextBoxStyle`
- Line breaks in code TextBox values **must** use `&#10;` (not literal newlines or `xml:space="preserve"`)
- XML special characters must be escaped (`&lt;`, `&gt;`, `&quot;`, `&amp;`)

## Language

Documentation, UI text, and commit context are in **Korean**. Code identifiers and XML comments are in English.
