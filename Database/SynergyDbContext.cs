using Microsoft.EntityFrameworkCore;
using Synergy.API.Entities;

namespace Synergy.API.Database;
public class SynergyDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Adventurer> Adventurers => Set<Adventurer>();

    public DbSet<Race> Races => Set<Race>();

    public DbSet<Class> Classes => Set<Class>();

    public DbSet<Party> Parties => Set<Party>();

    public SynergyDbContext(DbContextOptions<SynergyDbContext> options, IConfiguration configuration)
        : base(options)
        => _configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(
            connectionString: _configuration.GetConnectionString("SynergyDB"),
            serverVersion: ServerVersion.AutoDetect(_configuration.GetConnectionString("SynergyDB"))
        );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adventurer>()
            .HasKey(adventurer => adventurer.Id);
        modelBuilder.Entity<Adventurer>()
            .HasOne(adventurer => adventurer.Party)
            .WithMany(party => party.Members);
        modelBuilder.Entity<Adventurer>()
            .HasOne(adventurer => adventurer.Race);
        modelBuilder.Entity<Adventurer>()
            .HasOne(adventurer => adventurer.Class);

        modelBuilder.Entity<Party>()
            .HasKey(party => party.Id);
        modelBuilder.Entity<Party>()
            .HasMany(party => party.Members)
            .WithOne(adventurer => adventurer.Party);

        modelBuilder.Entity<Race>()
            .HasKey(race => race.Id);

        modelBuilder.Entity<Class>()
            .HasKey(@class => @class.Id);
    }
}