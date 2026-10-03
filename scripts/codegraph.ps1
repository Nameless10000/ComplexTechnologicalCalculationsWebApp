param([Parameter(ValueFromRemainingArguments = $true)][string[]] $GraphArguments)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$cli = Join-Path $projectRoot '.codex-tools\node_modules\.bin\codegraph.cmd'
Push-Location $projectRoot
try {
    if (-not (Test-Path -LiteralPath $cli)) {
        npm.cmd install --prefix .codex-tools --save-exact '@colbymchenry/codegraph@1.6.2' --cache .codex-tools/npm-cache --no-audit --no-fund
        if ($LASTEXITCODE -ne 0) { throw 'CodeGraph installation failed.' }
    }
    if (-not $GraphArguments) { $GraphArguments = @('status') }
    & $cli @GraphArguments
    exit $LASTEXITCODE
} finally { Pop-Location }
