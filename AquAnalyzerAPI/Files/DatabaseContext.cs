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

            modelBuilder.Entity<WaterMetrics>()
                .HasMany(w => w.Visualisations)
                .WithMany(v => v.MetricsUsed)
                .UsingEntity(
                    "WaterMetricsVisualisation",
                    l => l.HasOne(typeof(VisualisationData)).WithMany().HasForeignKey("VisualisationId"),
                    r => r.HasOne(typeof(WaterMetrics)).WithMany().HasForeignKey("WaterMetricsId"));

            modelBuilder.Entity<WaterData>()
                .HasMany(w => w.Visualisations)
                .WithMany(v => v.RawDataUsed)
                .UsingEntity(
                    "WaterDataVisualisation",
                    l => l.HasOne(typeof(VisualisationData)).WithMany().HasForeignKey("VisualisationId"),
                    r => r.HasOne(typeof(WaterData)).WithMany().HasForeignKey("WaterDataId"));

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


            // Remove one-to-one relationship, make it one-to-many
            entity.HasOne(a => a.WaterData)
                .WithMany(w => w.Abnormalities) // Update WaterData model to have collection
                .HasForeignKey(a => a.WaterDataId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

        });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(n => n.Message).IsRequired();
                entity.Property(n => n.Type).IsRequired();
                entity.Property(n => n.CreatedAt).IsRequired();

                entity.HasOne(n => n.Abnormality)
                    .WithMany()
                    .HasForeignKey(n => n.AbnormalityId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<VisualisationData>(entity =>
            {
                entity.Property(v => v.Type).IsRequired();

                entity.HasOne(v => v.Report)
                    .WithMany()
                    .HasForeignKey(v => v.ReportId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}