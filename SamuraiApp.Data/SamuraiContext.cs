using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data;

public class SamuraiContext : DbContext
{
    public DbSet<Samurai> Samurais { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Battle> Battles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-Q44S7N8;Database=applicationdb1;User Id=sa;Password=admin;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Samurai>()
            .HasMany(s => s.Battles)
            .WithMany(b => b.Samurais)
            .UsingEntity<BattleSamurai>
            (bs => bs.HasOne<Battle>().WithMany(),
             bs => bs.HasOne<Samurai>().WithMany())
            .Property(bs => bs.DateJoined)
            .HasDefaultValueSql("getdate()");
    }
}
