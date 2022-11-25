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
    {
        optionsBuilder.UseMySql(
            connectionString: _configuration.GetConnectionString("SynergyDB"),
            serverVersion: ServerVersion.AutoDetect(_configuration.GetConnectionString("SynergyDB"))
        );
    }
}