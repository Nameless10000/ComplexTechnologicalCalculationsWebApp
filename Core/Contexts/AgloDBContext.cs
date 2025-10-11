using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных проекта расчета агломерационной шихты
    /// </summary>
    public class AgloDBContext (DbContextOptions<AgloDBContext> opts) : DbContext (opts)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
