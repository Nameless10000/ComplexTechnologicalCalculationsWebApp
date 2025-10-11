using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных проекта расчета газодинамического режима доменной плавки
    /// </summary>
    public class GasDynamicDBContext (DbContextOptions<GasDynamicDBContext> opts) : DbContext(opts)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
