using Core.Contexts;

namespace Test;

/// <summary>
/// Тесты для либы BlastFurnaceSmeltingGasDynamicMode
/// </summary>
public class BlastFurnaceSmeltingGasDynamicModeTest
{
    private readonly TBalDBContext _dbContext;

    // Пример получения сервисов/репо/контекстов в тестовую среду
    public BlastFurnaceSmeltingGasDynamicModeTest(TBalDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Тестовый тест)
    /// </summary>
    [Fact]
    public void Test1()
    {
        var data = _dbContext.Model;

        var result = 2 * 2;
        Assert.Equal(4, result);
    }
}