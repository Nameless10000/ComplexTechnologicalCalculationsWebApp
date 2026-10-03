using Contracts.History;
using Core.Contexts;
using Data.Infrastructure;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Test;

public class OutboxTest
{
    [Fact]
    public async Task KafkaFailureRetainsEventAndRecoveryPublishesIt()
    {
        await using var db = new AuthDBContext(new DbContextOptionsBuilder<AuthDBContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var context = new DefaultHttpContext();
        var store = new CalculationHistoryStoreService(db, new HttpContextAccessor { HttpContext = context });
        await store.SaveAsync(new CalculationHistoryEvent { Module = "furnace", UserId = 1, RequestJson = "{}", ResponseJson = "{}", CreationDateTime = DateTime.UtcNow });
        Assert.Single(db.CalculationHistory);
        Assert.Single(db.CalculationOutbox);
        Assert.True(context.Response.Headers.ContainsKey("X-Calculation-Id"));
        var publisher = new StubPublisher();
        var dispatcher = new OutboxDispatcher(db, publisher, NullLogger<OutboxDispatcher>.Instance);
        await dispatcher.DispatchAsync(default);
        var message = await db.CalculationOutbox.SingleAsync();
        Assert.Null(message.ProcessedAt);
        Assert.Equal(1, message.Attempts);
        publisher.Available = true;
        message.NextRetryAt = DateTime.UtcNow.AddSeconds(-1);
        await db.SaveChangesAsync();
        await dispatcher.DispatchAsync(default);
        Assert.NotNull(message.ProcessedAt);
        Assert.Equal(1, publisher.Published);
        await dispatcher.DispatchAsync(default);
        Assert.Equal(1, publisher.Published);
    }

    private class StubPublisher : IHistoryEventPublisher
    {
        public bool Available { get; set; }
        public int Published { get; private set; }
        public Task PublishAsync(CalculationHistoryEvent historyEvent, CancellationToken token = default)
        {
            if (!Available) throw new IOException("Kafka unavailable");
            Published++;
            return Task.CompletedTask;
        }
    }
}
