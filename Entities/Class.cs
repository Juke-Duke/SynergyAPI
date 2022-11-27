using Synergy.API.Enums;

namespace Synergy.API.Entities;
public sealed class Class
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Role Role { get; set; }

    public Resource Resource { get; set; }
}