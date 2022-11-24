namespace Synergy.API.Entities;
public class Party
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required Adventurer Leader { get; set; }

    public required DateTime DateFounded { get; set; }
}