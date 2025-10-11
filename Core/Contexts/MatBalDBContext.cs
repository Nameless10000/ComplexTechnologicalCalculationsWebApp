using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст базы данных для проекта расчета материального баланса доменной плавки
    /// </summary>
    public class MatBalDBContext (DbContextOptions<MatBalDBContext> opts) : DbContext(opts)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
