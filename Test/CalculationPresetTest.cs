using Core.Contexts;
using Data.Services;
using Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Test;

public class CalculationPresetTest
{
    [Fact]
    public async Task PresetsRespectOwnershipAndNames()
    {
        await using var db = new AuthDBContext(new DbContextOptionsBuilder<AuthDBContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var service = new CalculationPresetService(db);
        var row = await service.Save(1, null, "furnace", "Режим", "описание", "{\"coke_rate\":420}");
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Find(2, row.Id));
        await Assert.ThrowsAsync<ConflictException>(() => service.Save(1, null, "furnace", "режим", "", "{}"));
        await service.Save(2, null, "furnace", "Режим", "", "{}");
        await service.Save(1, row.Id, "furnace", "Новый", "новое описание", "{}");
        Assert.Equal("Новый", (await service.Find(1, row.Id)).Name);
        await service.Delete(1, row.Id);
        Assert.Single(db.CalculationPresets);
    }
}
