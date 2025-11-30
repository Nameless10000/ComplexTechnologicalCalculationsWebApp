using Microsoft.EntityFrameworkCore;
using Core.Models.GasDynamic;

namespace Core.Contexts
{
    /// <summary>
    /// Контекст для базы данных проекта расчета газодинамического режима доменной плавки
    /// </summary>
    public class GasDynamicDBContext (DbContextOptions<GasDynamicDBContext> opts) : DbContext(opts)
    {
        public DbSet<BlastFurnaceInputModel> BlastFurnaceInputModels { get; set; }
        public DbSet<BlastFurnanceOutputModel> BlastFurnanceOutputModels { get; set; }
        public DbSet<CompositionParameters> CompositionParameters { get; set; }
        public DbSet<FuelAndBlastParameters> FuelAndBlastParameters { get; set; }
        public DbSet<FurnaceGeometry> FurnaceGeometries { get; set; }
        public DbSet<ThermalAndPressureParameters> ThermalAndPressureParameters { get; set; }
        public DbSet<MaterialProperties> MaterialProperties { get; set; }
        public DbSet<ProductionParameters> ProductionParameters { get; set; }
        public DbSet<BlastParameters> BlastParameters { get; set; }
        public DbSet<CarbonBalance> CarbonBalances { get; set; }
        public DbSet<ChargeAndPacking> ChargeAndPackings { get; set; }
        public DbSet<FurnaceGeometryOutput> FurnaceGeometryOutputs { get; set; }
        public DbSet<HearthGas> HearthGases { get; set; }
        public DbSet<HydrodynamicsLower> HydrodynamicsLowers { get; set; }
        public DbSet<AglomInputModel> AglomInputModels { get; set; }
        public DbSet<KoksContent> KoksContents { get; set; }
        public DbSet<AglomContent> AglomContents { get; set; }
        public DbSet<OkatContent> OkatContents { get; set; }
        public DbSet<EntityModel> EntityModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Связь один ко многим: AglomInputModel -> KoksContent
            modelBuilder.Entity<KoksContent>()
                .HasOne(k => k.AglomInputModel)
                .WithMany(a => a.KoksContents)
                .HasForeignKey(k => k.AglomInputModelId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Связь один ко многим: AglomInputModel -> AglomContent
            modelBuilder.Entity<AglomContent>()
                .HasOne(a => a.AglomInputModel)
                .WithMany(a => a.AglomContents)
                .HasForeignKey(a => a.AglomInputModelId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Связь один ко многим: AglomInputModel -> OkatContent
            modelBuilder.Entity<OkatContent>()
                .HasOne(o => o.AglomInputModel)
                .WithMany(a => a.OkatContents)
                .HasForeignKey(o => o.AglomInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnaceInputModel -> CompositionParameters
            modelBuilder.Entity<CompositionParameters>()
                .HasOne(c => c.BlastFurnaceInputModel)
                .WithOne(b => b.Composition)
                .HasForeignKey<CompositionParameters>(c => c.BlastFurnaceInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnaceInputModel -> FuelAndBlastParameters
            modelBuilder.Entity<FuelAndBlastParameters>()
                .HasOne(f => f.BlastFurnaceInputModel)
                .WithOne(b => b.FuelAndBlast)
                .HasForeignKey<FuelAndBlastParameters>(f => f.BlastFurnaceInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnaceInputModel -> FurnaceGeometry
            modelBuilder.Entity<FurnaceGeometry>()
                .HasOne(f => f.BlastFurnaceInputModel)
                .WithOne(b => b.Geometry)
                .HasForeignKey<FurnaceGeometry>(f => f.BlastFurnaceInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnaceInputModel -> ThermalAndPressureParameters
            modelBuilder.Entity<ThermalAndPressureParameters>()
                .HasOne(t => t.BlastFurnaceInputModel)
                .WithOne(b => b.ThermalAndPressure)
                .HasForeignKey<ThermalAndPressureParameters>(t => t.BlastFurnaceInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnaceInputModel -> MaterialProperties
            modelBuilder.Entity<MaterialProperties>()
                .HasOne(m => m.BlastFurnaceInputModel)
                .WithOne(b => b.Materials)
                .HasForeignKey<MaterialProperties>(m => m.BlastFurnaceInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnaceInputModel -> ProductionParameters
            modelBuilder.Entity<ProductionParameters>()
                .HasOne(p => p.BlastFurnaceInputModel)
                .WithOne(b => b.Production)
                .HasForeignKey<ProductionParameters>(p => p.BlastFurnaceInputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnanceOutputModel -> BlastParameters
            modelBuilder.Entity<BlastParameters>()
                .HasOne(b => b.BlastFurnanceOutputModel)
                .WithOne(o => o.BlastParameters)
                .HasForeignKey<BlastParameters>(b => b.BlastFurnanceOutputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnanceOutputModel -> CarbonBalance
            modelBuilder.Entity<CarbonBalance>()
                .HasOne(c => c.BlastFurnanceOutputModel)
                .WithOne(o => o.CarbonBalance)
                .HasForeignKey<CarbonBalance>(c => c.BlastFurnanceOutputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnanceOutputModel -> ChargeAndPacking
            modelBuilder.Entity<ChargeAndPacking>()
                .HasOne(c => c.BlastFurnanceOutputModel)
                .WithOne(o => o.ChargeAndPacking)
                .HasForeignKey<ChargeAndPacking>(c => c.BlastFurnanceOutputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnanceOutputModel -> FurnaceGeometryOutput
            modelBuilder.Entity<FurnaceGeometryOutput>()
                .HasOne(f => f.BlastFurnanceOutputModel)
                .WithOne(o => o.FurnaceGeometry)
                .HasForeignKey<FurnaceGeometryOutput>(f => f.BlastFurnanceOutputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному: BlastFurnanceOutputModel -> HearthGas
            modelBuilder.Entity<HearthGas>()
                .HasOne(h => h.BlastFurnanceOutputModel)
                .WithOne(o => o.HearthGas)
                .HasForeignKey<HearthGas>(h => h.BlastFurnanceOutputModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одно: BlastFurnanceOutputModel -> HydrodynamicsLower
            modelBuilder.Entity<HydrodynamicsLower>()
                .HasOne(h => h.BlastFurnanceOutputModel)
                .WithOne(o => o.HydrodynamicsLower)
                .HasForeignKey<HydrodynamicsLower>(h => h.BlastFurnanceOutputModelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
