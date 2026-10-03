using Core.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных для хранения данных авторизации пользователей
    /// </summary>
    public class AuthDBContext(DbContextOptions<AuthDBContext> opts) : IdentityDbContext<
        User, Role, int,
        IdentityUserClaim<int>,
        UserRoles, IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>(opts)
    {
        public DbSet<Core.Models.Calculations.CalculationRecord> CalculationHistory { get; set; }
        public DbSet<Core.Models.Calculations.OutboxMessage> CalculationOutbox { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var history = modelBuilder.Entity<Core.Models.Calculations.CalculationRecord>();
            history.Property(x => x.Module).HasMaxLength(32);
            history.Property(x => x.RequestId).HasMaxLength(200);
            history.Property(x => x.CorrelationId).HasMaxLength(200);
            history.Property(x => x.Status).HasMaxLength(20);
            history.Property(x => x.RequestJson).HasColumnType("jsonb");
            history.Property(x => x.ResponseJson).HasColumnType("jsonb");
            history.HasIndex(x => new { x.UserId, x.Module, x.CreatedAt });
            history.HasOne<Core.Models.Calculations.CalculationRecord>().WithMany().HasForeignKey(x => x.SourceCalculationId).OnDelete(DeleteBehavior.Restrict);
            var outbox = modelBuilder.Entity<Core.Models.Calculations.OutboxMessage>();
            outbox.Property(x => x.Payload).HasColumnType("jsonb");
            outbox.Property(x => x.LastError).HasMaxLength(1000);
            outbox.HasIndex(x => new { x.ProcessedAt, x.NextRetryAt });
            outbox.HasIndex(x => x.CalculationId).IsUnique();
            outbox.HasOne(x => x.Calculation).WithOne().HasForeignKey<Core.Models.Calculations.OutboxMessage>(x => x.CalculationId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
