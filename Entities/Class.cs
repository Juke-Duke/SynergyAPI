using Synergy.API.Enums;

namespace Synergy.API.Entities;
public sealed class Class
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required Role Role { get; set; }

    public required Resource Resource { get; set; }

    public required string Abilities { get; set; }
}