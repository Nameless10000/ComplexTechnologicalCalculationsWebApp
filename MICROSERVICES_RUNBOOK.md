# Запуск и эксплуатация микросервисной версии

## Что теперь из чего состоит

Публичный контракт для фронтенда остался в проекте `Web`. React продолжает ходить в те же MVC/API endpoints, а `Web` внутри вызывает расчетные сервисы по gRPC.

Основные проекты:

- `Web` - публичный API-фасад, авторизация, миграции БД, чтение истории и пресетов.
- `Contracts` - общий gRPC-контракт и DTO события истории расчетов.
- `AglomModeService` - gRPC-сервис расчета агломерационного режима. Внутри оставлен существующий REST-вызов во внешний сервис из `BaseLib.AglomMode`.
- `GasDynamicService` - gRPC-сервис газодинамического расчета. Использует локальный алгоритм из `BaseLib`.
- `SlagModeService` - gRPC-сервис шлакового режима. Внутри оставлен существующий REST-вызов во внешний сервис из `BaseLib.SlagMode`.
- `CalculationHistoryWriter` - background worker, читает Kafka topic и пишет историю расчетов в существующие таблицы БД.
- `BaseLib` - расчетные алгоритмы и DTO. Алгоритмы не переносились и не менялись.
- `Core` - EF Core контексты, модели БД, миграции.
- `Data` - application services, AutoMapper, Kafka producer, чтение истории/пресетов.

## Поток выполнения расчета

1. Фронтенд отправляет HTTP-запрос в `Web`, например `GasDynamic/Calculate`.
2. Контроллер вызывает соответствующий сервис из `Data.Services`.
3. `Data.Services` сериализует входной DTO в JSON и вызывает нужный gRPC-сервис.
4. gRPC-сервис десериализует JSON в исходную модель `BaseLib`, вызывает старый расчетный код и возвращает JSON результата.
5. `Web` возвращает клиенту прежний HTTP-ответ, например `{ data = calculationResult }`.
6. `Data.Services` публикует событие в Kafka topic `calculation-history`.
7. `CalculationHistoryWriter` читает событие и сохраняет историю в существующую схему БД.

Важная особенность: запись истории стала асинхронной. Ответ расчета клиент получает после успешного расчета и публикации события в Kafka, но до фактической записи worker-ом в БД.

## Запуск через Docker Compose

Из корня репозитория:

```powershell
docker compose up --build
```

После запуска:

- `Web`: `http://localhost:5000`
- `AglomModeService`: `http://localhost:5101` gRPC, HTTP/2
- `GasDynamicService`: `http://localhost:5102` gRPC, HTTP/2
- `SlagModeService`: `http://localhost:5103` gRPC, HTTP/2
- `Postgres`: `localhost:5432`
- `Kafka`: `localhost:9092`

Остановить:

```powershell
docker compose down
```

Остановить и удалить данные Postgres/Kafka:

```powershell
docker compose down -v
```

В `compose.yaml` внешние REST-сервисы для `AglomMode` и `SlagMode` настроены через `host.docker.internal`, чтобы контейнеры могли достучаться до сервисов, запущенных на хост-машине.

## Локальный запуск без Docker

Нужно отдельно поднять:

- PostgreSQL на `localhost:5432`;
- Kafka на `localhost:9092`;
- внешние REST-сервисы:
  - Aglom: `localhost:5296`;
  - Slag: `localhost:44324`.

Затем в разных терминалах:

```powershell
dotnet run --project AglomModeService/AglomModeService.csproj
dotnet run --project GasDynamicService/GasDynamicService.csproj
dotnet run --project SlagModeService/SlagModeService.csproj
dotnet run --project CalculationHistoryWriter/CalculationHistoryWriter.csproj
dotnet run --project Web/Web.csproj
```

По умолчанию gRPC endpoints берутся из `appsettings`:

- `AglomModeService`: `http://localhost:5101`
- `GasDynamicService`: `http://localhost:5102`
- `SlagModeService`: `http://localhost:5103`

`Web` смотрит на них через секцию `GrpcServices` в `Web/appsettings.Development.json`.

## Конфигурация

Ключевые секции:

```json
"GrpcServices": {
  "AglomMode": "http://localhost:5101",
  "GasDynamic": "http://localhost:5102",
  "SlagMode": "http://localhost:5103"
}
```

```json
"Kafka": {
  "BootstrapServers": "localhost:9092",
  "CalculationHistoryTopic": "calculation-history"
}
```

```json
"ExternalServer": {
  "Domain": "localhost:44324",
  "AglomDomain": "localhost:5296"
}
```

Для Docker эти значения переопределяются переменными окружения в `compose.yaml`.

## Проверка

Сборка:

```powershell
dotnet build ComplexTechnologicalCalculationsWebApp.sln
```

Проверка docker-compose конфига:

```powershell
docker compose config
```

Тесты:

```powershell
dotnet test ComplexTechnologicalCalculationsWebApp.sln
```

Сейчас часть тестов зависит от внешних REST-сервисов `AglomMode` и `SlagMode`. Если эти сервисы не подняты или не возвращают ожидаемый ответ/JWT, тесты падают, хотя solution собирается.

## Типичные места сбоя

### `Web` не может вызвать gRPC-сервис

Симптомы:

- ошибка `StatusCode="Unavailable"`;
- `connection refused`;
- расчетный endpoint возвращает `BadRequest` с ошибкой подключения.

Что проверить:

- запущен ли нужный сервис;
- совпадает ли адрес в `Web/appsettings.Development.json` или переменной `GrpcServices__...`;
- слушает ли gRPC-сервис HTTP/2;
- нет ли конфликта портов `5101`, `5102`, `5103`.

Быстрая проверка:

```powershell
docker compose ps
```

или при локальном запуске:

```powershell
netstat -ano | findstr 5101
netstat -ano | findstr 5102
netstat -ano | findstr 5103
```

### История расчета не появляется сразу

Это ожидаемо, потому что запись истории теперь идет через Kafka и worker.

Что проверить:

- запущен ли `CalculationHistoryWriter`;
- доступна ли Kafka;
- совпадает ли topic `calculation-history`;
- нет ли ошибок в логах worker-а;
- поднята ли БД и применились ли миграции.

Команды:

```powershell
docker compose logs history-writer
docker compose logs kafka
docker compose logs postgres
```

### Расчет прошел, но публикация в Kafka упала

Симптомы:

- расчетный сервис вернул результат в gRPC;
- `Web` вернул ошибку на HTTP-запросе из-за Kafka producer.

Причины:

- Kafka не запущена;
- неверный `Kafka__BootstrapServers`;
- Kafka еще стартует.

Решение:

- дождаться полного старта Kafka;
- перезапустить `web`;
- проверить `docker compose logs kafka`.

### Внешний REST-сервис Aglom/Slag недоступен

Симптомы:

- `AglomMode` возвращает пустой `AglomResponseData`;
- `SlagMode` падает с ошибкой `Не удалось получить JWT-токен от сервера`;
- тесты `AglomModeTest`/`SlagModeTest` падают.

Что проверить:

- запущен ли внешний REST-сервис;
- правильный ли порт:
  - Aglom: `5296`;
  - Slag: `44324`;
- для Docker доступен ли сервис через `host.docker.internal`;
- корректны ли `Authorization:UserName` и `Authorization:Password` для Slag.

### Postgres поднят, но базы не найдены

`compose.yaml` создает базы через `docker/postgres/init-multiple-databases.sql` только при первом создании volume.

Если volume уже существовал до добавления init-скрипта:

```powershell
docker compose down -v
docker compose up --build
```

Осторожно: `down -v` удалит локальные данные контейнерной БД.

### Миграции не применились

Сейчас миграции по-прежнему применяет `Web` при старте. Если запускать только worker без `Web`, таблиц может не быть.

Решение:

- сначала запустить `Web`;
- либо применить миграции вручную через EF tooling.

### Docker gRPC-сервис слушает не тот порт

В контейнерах gRPC-сервисы слушают `http://+:8080`, а наружу проброшены:

- `5101:8080`;
- `5102:8080`;
- `5103:8080`.

Внутри docker-сети `Web` ходит не на `localhost`, а на имена сервисов:

- `http://aglom-mode-service:8080`;
- `http://gas-dynamic-service:8080`;
- `http://slag-mode-service:8080`.

## Что важно помнить при дальнейшей разработке

- Не менять DTO, которые возвращаются из контроллеров `Web`, без отдельного согласования с фронтом.
- Расчетные алгоритмы держать в `BaseLib`; gRPC-сервисы только оборачивают их.
- Если добавляется новый расчетный модуль, обычный путь такой:
  1. добавить gRPC service в `Contracts/Protos/calculation.proto`;
  2. создать отдельный запускаемый сервис;
  3. зарегистрировать gRPC client в `Web`;
  4. публиковать `CalculationHistoryEvent`;
  5. добавить обработку module name в `CalculationHistoryWriter`;
  6. добавить сервис в `compose.yaml`.
- Схему БД не менять без миграций и отдельного решения по совместимости истории.

