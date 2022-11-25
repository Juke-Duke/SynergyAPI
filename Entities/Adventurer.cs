using Synergy.API.Enums;

namespace Synergy.API.Entities;
public sealed class Adventurer
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Rank Rank { get; set; }

    public Race Race { get; set; } = null!;

    public Class Class { get; set; } = null!;

    public Party? Party { get; set; }
}