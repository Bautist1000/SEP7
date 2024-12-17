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
        public DbSet<Visualisation> Visualisations { get; set; }
        public DbSet<WaterData> WaterData { get; set; }
        public DbSet<WaterMetrics> WaterMetrics { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=AquAnalyzerAPI.database.db"); // Make sure this path is correct
            }
}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().UseTpcMappingStrategy();
            modelBuilder.Entity<Analyst>().ToTable("Analyst").Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<VisualDesigner>().ToTable("VisualDesigner").Property(s => s.Id).ValueGeneratedOnAdd();


            modelBuilder.Entity<WaterMetrics>()
                       .HasMany(w => w.Visualisations)
                       .WithMany(v => v.MetricsUsed)
                       .UsingEntity(
                           "WaterMetricsVisualisation",
                           l => l.HasOne(typeof(Visualisation)).WithMany().HasForeignKey("VisualisationId"),
                           r => r.HasOne(typeof(WaterMetrics)).WithMany().HasForeignKey("WaterMetricsId"));

            modelBuilder.Entity<WaterData>()
                    .HasMany(w => w.Visualisations)
                    .WithMany(v => v.RawDataUsed)
                    .UsingEntity(
                         "WaterDataVisualisation",
                         l => l.HasOne(typeof(Visualisation)).WithMany().HasForeignKey("VisualisationId"),
                        r => r.HasOne(typeof(WaterData)).WithMany().HasForeignKey("WaterDataId"));
        }
    }
}

