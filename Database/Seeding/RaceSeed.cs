using Synergy.API.Entities;

namespace Synergy.API.Database.Seeding;
public static class RaceSeed
{
    public static void Seed(SynergyDbContext db)
    {
        List<string[]> races = new()
        {
            new[] { "Human", "Thundorum City" },
            new[] { "Lush Elf", "Serinii Valush" },
            new[] { "Dwarf", "Stoneborn Mountain" },
            new[] { "Mist Elf", "Mistwood Forest" },
            new[] { "Giant", "Jotunheim" },
            new[] { "Orc", "Red Valley" }
        };

        foreach (var race in races)
            db.Races.Add(new Race
            {
                Name = race[0],
                Origin = race[1]
            });

        db.SaveChanges();
    }
}