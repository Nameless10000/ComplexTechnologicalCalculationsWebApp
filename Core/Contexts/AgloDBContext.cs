using Console;
using Core.Models.AglomMode;
using Core.Models.SlagMode;
using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных проекта расчета агломерационной шихты
    /// </summary>
    public class AgloDBContext (DbContextOptions<AgloDBContext> opts) : DbContext (opts)
    {
        public DbSet<ZolaOfCocksickDB> ZolaOfCocksicks { get; set; }
        public DbSet<ShihtaComponentDB> ShihtaComponents { get; set; }
        public DbSet<CocksickDB> Cocksicks { get; set; }
        public DbSet<FluxAdditionsDB> FluxAdditionss { get; set; }
        public DbSet<StartEnterDB> StartEnters { get; set; }
        public DbSet<AglomResponseDB> AglomResponses { get; set; }
        public DbSet<AglomRequestDB> AglomRequests { get; set; }
        public DbSet<ComponentInfoDB> Components { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ShihtaComponentDB>().HasOne(x => x.AglomRequest).WithMany(x => x.ShihtaComponents);
            modelBuilder.Entity<ComponentInfoDB>().HasOne(x => x.AglomResponse).WithMany(x => x.Components);
        }
    }
}
