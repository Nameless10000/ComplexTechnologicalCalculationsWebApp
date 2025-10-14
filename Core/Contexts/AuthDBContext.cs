using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core.Models.Auth;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для авторизации пользователей
    /// </summary>
    public class AuthDBContext : IdentityDbContext<
        User, Role, int,
        IdentityUserClaim<int>, UserRoles, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    /*
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRoles> UsersRoles { get; set; }
    */
}
