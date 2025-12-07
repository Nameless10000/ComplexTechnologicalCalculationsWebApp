using Core.Models.GasDynamic;
using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных проекта расчета газодинамического режима доменной плавки
    /// </summary>
    public class GasDynamicDBContext(DbContextOptions<GasDynamicDBContext> opts) : DbContext(opts)
    {
        public DbSet<CalculationModel> CalculationModels { get; set; }
    }
}