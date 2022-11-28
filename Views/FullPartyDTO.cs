namespace Synergy.API.Views;
public sealed class FullPartyDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateFounded { get; set; }

    public ICollection<AdventurerDTO> Adventurers { get; set; } = new List<AdventurerDTO>();
}