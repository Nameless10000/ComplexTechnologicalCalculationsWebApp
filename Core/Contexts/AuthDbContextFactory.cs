using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Core.Contexts;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDBContext>
{
    public AuthDBContext CreateDbContext(string[] args) => new(new DbContextOptionsBuilder<AuthDBContext>()
        .UseNpgsql("Host=localhost;Database=AuthDB;Username=postgres").Options);
}
