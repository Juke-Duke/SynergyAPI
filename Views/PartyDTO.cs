namespace Synergy.API.Views;
public sealed class PartyDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateFounded { get; set; }
}