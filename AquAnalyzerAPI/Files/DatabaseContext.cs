using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Files
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Abnormality> Abnormalities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Analyst> Analysts { get; set; }
        public DbSet<VisualDesigner> VisualDesigners { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<VisualisationData> Visualisations { get; set; }
        public DbSet<WaterData> WaterData { get; set; }
        public DbSet<WaterMetrics> WaterMetrics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=AquAnalyzerAPI.database.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().UseTpcMappingStrategy();

            modelBuilder.Entity<Analyst>()
                .ToTable("Analyst")
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<VisualDesigner>()
                .ToTable("VisualDesigner")
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.GeneratedDate).IsRequired();

                entity.HasOne(r => r.VisualDesigner)
                    .WithMany(v => v.GeneratedReports)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(r => r.Visualisations)
                    .WithOne(v => v.Report)
                    .HasForeignKey(v => v.ReportId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<VisualisationData>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.ReportId).IsRequired();

                entity.OwnsOne(v => v.ChartConfig, config =>
                {
                    config.Property(c => c.Title).IsRequired();
                    config.Property(c => c.XAxisLabel).IsRequired();
                    config.Property(c => c.YAxisLabel).IsRequired();
                    config.Property(c => c.ColorScheme).IsRequired();
                });

                // Many-to-Many relationships
                entity.HasMany(v => v.MetricsUsed)
                    .WithMany(m => m.Visualisations)
                    .UsingEntity(
                        "VisualisationWaterMetrics",
                        l => l.HasOne(typeof(WaterMetrics)).WithMany().HasForeignKey("WaterMetricsId"),
                        r => r.HasOne(typeof(VisualisationData)).WithMany().HasForeignKey("VisualisationId"));

                entity.HasMany(v => v.RawDataUsed)
                    .WithMany(w => w.Visualisations)
                    .UsingEntity(
                        "VisualisationWaterData",
                        l => l.HasOne(typeof(WaterData)).WithMany().HasForeignKey("WaterDataId"),
                        r => r.HasOne(typeof(VisualisationData)).WithMany().HasForeignKey("VisualisationId"));
            });

            modelBuilder.Entity<WaterData>(entity =>
            {
                entity.HasOne(w => w.WaterMetrics)
                    .WithMany(m => m.WaterData)
                    .HasForeignKey(w => w.WaterMetricsId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(w => w.Location).IsRequired();
                entity.Property(w => w.SourceType).IsRequired();
                entity.Property(w => w.Timestamp).IsRequired();
            });

            modelBuilder.Entity<WaterMetrics>(entity =>
            {
                entity.Property(w => w.DateGeneratedOn).IsRequired();

                entity.HasOne(w => w.Abnormality)
                    .WithOne(a => a.WaterMetrics)
                    .HasForeignKey<Abnormality>(a => a.WaterMetricsId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Abnormality>(entity =>
         {
             entity.Property(a => a.Timestamp).IsRequired();
             entity.Property(a => a.Description).IsRequired();
             entity.Property(a => a.Type).IsRequired();

             entity.HasKey(e => e.Id);
             entity.Property(e => e.Id)
                 .ValueGeneratedOnAdd();


             entity.HasOne(a => a.WaterData)
                 .WithMany(w => w.Abnormalities)
                 .HasForeignKey(a => a.WaterDataId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.SetNull);

         });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(n => n.Abnormality)
                    .WithMany()
                    .HasForeignKey(n => n.AbnormalityId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
