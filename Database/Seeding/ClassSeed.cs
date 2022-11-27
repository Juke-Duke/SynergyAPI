using Synergy.API.Entities;
using Synergy.API.Enums;

namespace Synergy.API.Database.Seeding;
public static class ClassSeed
{
    public static void Seed(SynergyDbContext db)
    {
        if (db.Classes.Any())
            return;

        List<string[]> classes = new()
        {
            new[] { "Warrior", "Damage", "Fervor" },
            new[] { "Marksman", "Damage", "Focus" },
            new[] { "Mage", "Damage", "Mana" },
            new[] { "Cleric", "Healer", "Faith" },
            new[] { "Paladin", "Tank", "Faith" },
            new[] { "Rogue", "Damage", "Energy" },
            new[] { "Reaper", "Damage", "Soul" },
        };

        foreach (var @class in classes)
            db.Classes.Add(new Class
            {
                Name = @class[0],
                Role = Enum.Parse<Role>(@class[1]),
                Resource = Enum.Parse<Resource>(@class[2])
            });

        db.SaveChanges();
    }
}