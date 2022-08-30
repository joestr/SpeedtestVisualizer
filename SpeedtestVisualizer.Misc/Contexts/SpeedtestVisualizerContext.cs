using Microsoft.EntityFrameworkCore;
using SpeedtestVisualizer.Misc.Objects;

namespace SpeedtestVisualizer.Misc.Contexts;

public class SpeedtestVisualizerContext : DbContext
{
    public SpeedtestVisualizerContext(DbContextOptions<SpeedtestVisualizerContext> options) : base(options)
    {
    }

    public DbSet<SpeedtestResult> SpeedtestResults { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SpeedtestResult>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}