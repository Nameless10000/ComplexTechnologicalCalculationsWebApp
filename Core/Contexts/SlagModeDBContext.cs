using Core.Models.SlagMode;
using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных проекта расчета шлакового режима доменной плавки
    /// </summary>
    public class SlagModeDBContext(DbContextOptions<SlagModeDBContext> opts) : DbContext(opts)
    {
        public DbSet<Slag> Slags {get; set;}
        public DbSet<ChargeComponent> ChargeComponents {get; set;}
        public DbSet<CastIron> CastIrons {get; set;}
        public DbSet<Request> Requests {get; set;}
        public DbSet<Response> Responses {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Request>().HasMany(x => x.Components).WithMany(x => x.Requests);
            modelBuilder.Entity<Response>().HasOne(x => x.Request).WithOne(x => x.Response);
            
        }
    }
}
