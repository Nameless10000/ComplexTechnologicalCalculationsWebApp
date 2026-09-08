using Core.Models.Furnace;
using Microsoft.EntityFrameworkCore;

namespace Core.Contexts;

public class FurnaceDBContext(
    DbContextOptions<FurnaceDBContext> opts)
    : DbContext(opts)
{
    public DbSet<FurnaceCalculationModel> CalculationModels { get; set; }
}