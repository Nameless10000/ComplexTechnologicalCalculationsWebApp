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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
