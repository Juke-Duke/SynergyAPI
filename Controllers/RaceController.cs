using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Synergy.API.Database;
using Synergy.API.Entities;

namespace Synergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RaceController : ControllerBase
{
    private readonly SynergyDbContext _db;

    public RaceController(SynergyDbContext db)
        => _db = db;

    [HttpGet]
    public async Task<Response<IReadOnlyCollection<Race>>> GetAllRaces()
        => new(200, "OK - All available races", await _db.Races.ToListAsync());

    [HttpGet("{name}")]
    public async Task<Response<Race>> GetRaceByName(string name)
    {
        var raceToRead = await _db.Races.FirstOrDefaultAsync(race => race.Name == name);

        return raceToRead is not null
            ? new(200, $"OK - Race with name {name} found.", raceToRead)
            : new(404, $"NotFound - Race with name `{name}` not found.", null);
    }
}