using Microsoft.EntityFrameworkCore;
using AquAnalyzerAPI.Models;

namespace AquAnalyzerAPI.Files
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<WaterMetrics> WaterMetrics { get; set; }
        public DbSet<Abnormality> Abnormalities { get; set; }
        public DbSet<WaterData> WaterData { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Analyst> Analysts { get; set; }
        public DbSet<VisualDesigner> VisualDesigners { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Visualisation> Visualisations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=database.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().UseTpcMappingStrategy();
            modelBuilder.Entity<Analyst>().ToTable("Analyst").Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<VisualDesigner>().ToTable("VisualDesigner").Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Bachelor>().ToTable("Bachelors").Property(b => b.Id).ValueGeneratedOnAdd();



        }
    }
}

