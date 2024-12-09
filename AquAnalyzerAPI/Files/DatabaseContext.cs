using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DbSet<WaterMetrics> WaterMetrics { get; set; }
    public DbSet<Abnormality> Abnormalities { get; set; }
    public DbSet<WaterData> WaterData { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Analyst> Analysts { get; set; }
    public DbSet<VisualDesigner> VisualDesigners { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("YourConnectionStringHere");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships and constraints here if needed
    }
}