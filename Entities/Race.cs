namespace Synergy.API.Entities;
public sealed class Race
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Origin { get; set; } = string.Empty;
}