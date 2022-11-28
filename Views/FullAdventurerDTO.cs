using Synergy.API.Entities;
using Synergy.API.Enums;

namespace Synergy.API.Views;
public sealed class FullAdventurerDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Rank Rank { get; set; }

    public Race Race { get; set; } = null!;

    public Class Class { get; set; } = null!;

    public PartyDTO? Party { get; set; }
}