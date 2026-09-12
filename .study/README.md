# 학습 진도 기록

챕터 앱이 실행될 때마다 이 폴더에 `{챕터폴더명}.json` 을 남깁니다.
챕터별로 파일을 나눈 이유는 두 챕터를 동시에 켜도 서로의 기록을 덮어쓰지 않게 하기 위해서입니다.

이 폴더는 **의도적으로 git 에 추적됩니다.** `.gitignore` 에 넣지 마세요.
추적해 두면 기록이 실수로 지워져도 `git checkout` 으로 되살릴 수 있습니다.

전체 진도는 학습 현황판에서 봅니다.

```
dotnet run --project HelloWPF/HelloWPF.csproj
```

## 기록을 건드리지 않고 시험해 보려면

`WPF_STUDY_DIR` 환경 변수에 다른 경로를 지정하면 챕터 앱과 현황판이 그쪽을 씁니다.

```powershell
$env:WPF_STUDY_DIR = "$env:TEMP\wpf-study-test"
```
