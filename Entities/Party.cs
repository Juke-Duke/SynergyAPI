namespace Synergy.API.Entities;
public sealed class Party
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateFounded { get; set; }

    public ICollection<Adventurer> Members { get; set; } = new List<Adventurer>();
}