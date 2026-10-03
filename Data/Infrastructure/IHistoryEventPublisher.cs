using Contracts.History;

namespace Data.Infrastructure;

public interface IHistoryEventPublisher
{
    Task PublishAsync(CalculationHistoryEvent historyEvent, CancellationToken cancellationToken = default);
}
