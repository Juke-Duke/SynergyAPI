namespace Synergy.API.Entities;
public class Race
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required string Origin { get; set; }

    public required string Traits { get; set; }
}