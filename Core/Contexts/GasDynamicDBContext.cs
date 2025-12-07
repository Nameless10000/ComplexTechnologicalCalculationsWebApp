using Core.Models.GasDynamic;
using Core.Models.GasDynamic.Aglom.Inputs;
using Core.Models.GasDynamic.Aglom.Outputs;
using Core.Models.GasDynamic.Base.Inputs;
using Core.Models.GasDynamic.Base.Outputs;
using Microsoft.EntityFrameworkCore;
using FurnaceGeometry = Core.Models.GasDynamic.Base.Inputs.FurnaceGeometry;
using FurnaceGeometryOutput = Core.Models.GasDynamic.Base.Outputs.FurnaceGeometry;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных проекта расчета газодинамического режима доменной плавки
    /// </summary>
    public class GasDynamicDBContext(DbContextOptions<GasDynamicDBContext> opts) : DbContext(opts)
    {
        public DbSet<AglomContent> AglomContents { get; set; }
        public DbSet<KoksContent> KoksContents { get; set; }
        public DbSet<OkatContent> OkatContents { get; set; }
        public DbSet<AglomInputModel> AglomInputs { get; set; }
        public DbSet<AglomOutputModel> AglomOutputs { get; set; }
        public DbSet<BlastFurnaceInputModel> BlastFurnaceInputs { get; set; }
        public DbSet<CompositionParameters> CompositionParameters { get; set; }
        public DbSet<FuelAndBlastParameters> FuelAndBlastParameters { get; set; }
        public DbSet<FurnaceGeometry> FurnaceGeometryInputs { get; set; }
        public DbSet<MaterialProperties> MaterialProperties { get; set; }
        public DbSet<ProductionParameters> ProductionParameters { get; set; }
        public DbSet<ThermalAndPressureParameters> ThermalAndPressureParameters { get; set; }
        public DbSet<BlastFurnanceOutputModel> BlastFurnanceOutputs { get; set; }
        public DbSet<BlastParameters> BlastParameters { get; set; }
        public DbSet<CarbonBalance> CarbonBalances { get; set; }
        public DbSet<ChargeAndPacking> ChargeAndPackings { get; set; }
        public DbSet<FurnaceGeometryOutput> FurnaceGeometryOutputs { get; set; }
        public DbSet<HearthGas> HearthGases { get; set; }
        public DbSet<HydrodynamicsLower> HydrodynamicsLowers { get; set; }
        public DbSet<HydrodynamicsUpper> HydrodynamicsUppers { get; set; }
        public DbSet<IntermediateGas1000> IntermediateGases1000 { get; set; }
        public DbSet<MaterialConsumption> MaterialConsumptions { get; set; }
        public DbSet<ThermalParameters> ThermalParameters { get; set; }
        public DbSet<TopGas> TopGases { get; set; }
        public DbSet<RequestModelV2> CalculationInputs { get; set; }
        public DbSet<ResponseModelV2> CalculationOutputs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AglomContent>()
                .HasOne(x => x.AglomInputModel)
                .WithMany(x => x.AglomContents)
                .HasForeignKey(x => x.AglomInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KoksContent>()
                .HasOne(x => x.AglomInputModel)
                .WithMany(x => x.KoksContents)
                .HasForeignKey(x => x.AglomInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OkatContent>()
                .HasOne(x => x.AglomInputModel)
                .WithMany(x => x.OkatContents)
                .HasForeignKey(x => x.AglomInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}