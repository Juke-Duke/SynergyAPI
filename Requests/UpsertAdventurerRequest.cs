using Synergy.API.Enums;

namespace Synergy.API.Requests;
public sealed record UpsertAdventurerRequest
(
    string Name,
    Rank Rank,
    string RaceName,
    string ClassName,
    int? PartyId
);