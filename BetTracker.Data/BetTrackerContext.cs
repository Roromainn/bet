using Microsoft.EntityFrameworkCore;
using BetTracker.Core.Models;

namespace BetTracker.Data;

public class BetTrackerContext : DbContext
{
    public DbSet<Bookmaker> Bookmakers => Set<Bookmaker>();
    public DbSet<Offre> Offres => Set<Offre>();
    public DbSet<Calcul> Calculs => Set<Calcul>();
    public DbSet<Execution> Executions => Set<Execution>();
    public DbSet<MouvementBankroll> Mouvements => Set<MouvementBankroll>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(
            Path.GetDirectoryName(typeof(BetTrackerContext).Assembly.Location) ?? "",
            "..",
            "..",
            "BetTracker.db"
        );
        var absolutePath = Path.GetFullPath(dbPath);
        optionsBuilder.UseSqlite($"Data Source={absolutePath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Bookmaker 1 → N Offre
        modelBuilder.Entity<Bookmaker>()
            .HasMany(b => b.Offres)
            .WithOne(o => o.Bookmaker)
            .HasForeignKey(o => o.BookmakerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Bookmaker 1 → N MouvementBankroll
        modelBuilder.Entity<Bookmaker>()
            .HasMany(b => b.Mouvements)
            .WithOne(m => m.Bookmaker)
            .HasForeignKey(m => m.BookmakerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Offre 1 → N Calcul
        modelBuilder.Entity<Offre>()
            .HasMany(o => o.Calculs)
            .WithOne(c => c.Offre)
            .HasForeignKey(c => c.OffreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Offre 1 → N MouvementBankroll
        modelBuilder.Entity<Offre>()
            .HasMany(o => o.Mouvements)
            .WithOne(m => m.Offre)
            .HasForeignKey(m => m.OffreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Calcul 1 → 1 Execution
        modelBuilder.Entity<Calcul>()
            .HasOne(c => c.Execution)
            .WithOne(e => e.Calcul)
            .HasForeignKey<Execution>(e => e.CalculId)
            .OnDelete(DeleteBehavior.Cascade);

        // Convert decimal to double for SQLite (ref: https://learn.microsoft.com/en-us/ef/core/providers/sqlite/limitations)
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(decimal))
                {
                    property.SetColumnType("real");
                }
            }
        }

        // Seed Bookmakers
        modelBuilder.Entity<Bookmaker>().HasData(
            new Bookmaker { Id = 1, Nom = "Betclic", UrlLogo = "", Actif = false, DateActivation = DateTime.MinValue },
            new Bookmaker { Id = 2, Nom = "Winamax", UrlLogo = "", Actif = false, DateActivation = DateTime.MinValue },
            new Bookmaker { Id = 3, Nom = "Unibet", UrlLogo = "", Actif = false, DateActivation = DateTime.MinValue },
            new Bookmaker { Id = 4, Nom = "PMU", UrlLogo = "", Actif = false, DateActivation = DateTime.MinValue },
            new Bookmaker { Id = 5, Nom = "Bwin", UrlLogo = "", Actif = false, DateActivation = DateTime.MinValue },
            new Bookmaker { Id = 6, Nom = "NetBet", UrlLogo = "", Actif = false, DateActivation = DateTime.MinValue },
            new Bookmaker { Id = 7, Nom = "ZEbet", UrlLogo = "", Actif = false, DateActivation = DateTime.MinValue },
            new Bookmaker { Id = 8, Nom = "Parions Sport", UrlLogo = "", Actif = false, DateActivation = DateTime.MinValue }
        );
    }
}
