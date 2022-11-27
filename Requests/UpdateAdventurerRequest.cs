using Synergy.API.Enums;

namespace Synergy.API.Requests;
public sealed record UpdateAdventurerRequest
(
    string Name,
    Rank Rank,
    string RaceName,
    string ClassName,
    int? PartyId
);