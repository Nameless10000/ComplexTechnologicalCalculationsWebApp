using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных проекта расчета показателей теплового режима доменной плавки
    /// </summary>
    public class TModeDBContext (DbContextOptions<TModeDBContext> opts) : DbContext(opts)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
