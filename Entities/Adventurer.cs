using Synergy.API.Enums;

namespace Synergy.API.Entities;
public sealed class Adventurer
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required Rank Rank { get; set; }

    public required Race Race { get; set; }

    public required Class Class { get; set; }

    public required Party Party { get; set; }
}