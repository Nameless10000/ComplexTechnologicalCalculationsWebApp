# Навигация по решению

CodeGraph установлен локально через npm: `@colbymchenry/codegraph@1.6.2`.
Пакет с точным именем `codegraph` в npm не предоставляет нужного CLI.
Индекс и зависимости не включаются в Git.

```powershell
./scripts/codegraph.ps1 init .
./scripts/codegraph.ps1 sync .
./scripts/codegraph.ps1 query FurnaceCalculationService --json
./scripts/codegraph.ps1 callers Calculate --json
./scripts/codegraph.ps1 impact FurnaceCalculationService --depth 3
./scripts/codegraph.ps1 node Data/Services/FurnaceCalculationService.cs
```

Первичная индексация 03.10.2026: 246 файлов, 3141 узел, 5370 связей.
Перед изменением подсистемы проверять её символы и зависимости; после этапа обновлять индекс.
Документация CLI: https://colbymchenry.github.io/codegraph/reference/cli/
