using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Core.Contexts;
public class AgloDBContextFactory : IDesignTimeDbContextFactory<AgloDBContext> { public AgloDBContext CreateDbContext(string[] args) => new(new DbContextOptionsBuilder<AgloDBContext>().UseNpgsql("Host=localhost;Database=AgloDBContext;Username=postgres").Options); }
public class SlagModeDBContextFactory : IDesignTimeDbContextFactory<SlagModeDBContext> { public SlagModeDBContext CreateDbContext(string[] args) => new(new DbContextOptionsBuilder<SlagModeDBContext>().UseNpgsql("Host=localhost;Database=SlagModeDBContext;Username=postgres").Options); }
public class GasDynamicDBContextFactory : IDesignTimeDbContextFactory<GasDynamicDBContext> { public GasDynamicDBContext CreateDbContext(string[] args) => new(new DbContextOptionsBuilder<GasDynamicDBContext>().UseNpgsql("Host=localhost;Database=GasDynamicDBContext;Username=postgres").Options); }
public class FurnaceDBContextFactory : IDesignTimeDbContextFactory<FurnaceDBContext> { public FurnaceDBContext CreateDbContext(string[] args) => new(new DbContextOptionsBuilder<FurnaceDBContext>().UseNpgsql("Host=localhost;Database=FurnaceDBContext;Username=postgres").Options); }
