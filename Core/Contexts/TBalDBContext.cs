using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных проекта расчета теплового баланса доменной плавки
    /// </summary>
    public class TBalDBContext (DbContextOptions<TBalDBContext> opts) : DbContext(opts)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
