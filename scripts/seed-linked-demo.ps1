#requires -Version 7.0
param(
    [Parameter(Mandatory = $true)][ValidateNotNullOrEmpty()][string] $UserName,
    [ValidateSet('codex-ctc-check')][string] $ComposeProject = 'codex-ctc-check'
)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$demoId = [Guid]::NewGuid().ToString()
$normalizedUser = $UserName.ToUpperInvariant().Replace("'", "''")
# A clearly marked fixture for UI prefill only; it does not call calculation APIs.
# It is idempotent per user and only works in the explicitly named local test project.
$sql = @"
INSERT INTO "CalculationHistory" ("Id", "UserId", "UserName", "Module", "RequestId", "CorrelationId", "RequestJson", "ResponseJson", "Status", "CreatedAt")
SELECT '$demoId', u."Id", u."UserName", 'aglom-mode', 'demo-linked-aglom', 'demo-linked-aglom', '{"demo":true}',
'{"components":[{"componentName":"\u0418\u0442\u043e\u0433","reportFe":58.124,"reportS":0.028,"reportCaO":8.468,"reportSiO2":6.048,"reportAl2O3":1.066,"reportMgO":1.865,"reportMnO":0.1,"reportTiO2":0.5,"reportComponentOfShihta":110.092}]}', 'Saved', NOW()
FROM "AspNetUsers" u WHERE u."NormalizedUserName" = '$normalizedUser'
AND NOT EXISTS (SELECT 1 FROM "CalculationHistory" h WHERE h."UserId"=u."Id" AND h."RequestId"='demo-linked-aglom');
SELECT h."Id" FROM "CalculationHistory" h JOIN "AspNetUsers" u ON u."Id"=h."UserId"
WHERE u."NormalizedUserName"='$normalizedUser' AND h."RequestId"='demo-linked-aglom' ORDER BY h."CreatedAt" DESC LIMIT 1;
"@
Push-Location $projectRoot
try {
    $result = $sql | docker compose -p $ComposeProject exec -T postgres psql -U postgres -d AuthDB -At -v ON_ERROR_STOP=1
    if ($LASTEXITCODE -ne 0) { throw 'Не удалось добавить demo. Проверьте, что тестовый стенд запущен.' }
    $recordId = $result | Where-Object { $_ -match '^[0-9a-f-]{36}$' } | Select-Object -Last 1
    if (-not $recordId) { throw 'Пользователь не найден. Укажите имя из профиля после регистрации.' }
    Write-Output "Демонстрационный результат Aglom добавлен для $UserName (данные для проверки переноса, не реальный расчёт)."
    Write-Output "История: http://localhost:3000/calculations?module=aglom-mode"
    Write-Output "Перенос: http://localhost:3000/slag-mode?sourceCalculationId=$recordId"
    Write-Output 'Ожидается: Agglomerate23 Fe=58.124, CaO=8.468; расход сохранён из шаблона Slag.'
} finally { Pop-Location }
