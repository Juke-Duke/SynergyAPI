using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Synergy.API.Database;
using Synergy.API.Entities;
using Synergy.API.Requests;

namespace Synergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdventurerController : ControllerBase
{
    private readonly SynergyDbContext _db;

    public AdventurerController(SynergyDbContext db)
        => _db = db;

    [HttpGet]
    public async Task<Response<IReadOnlyCollection<Adventurer>>> GetAllAdventurers()
        => new(200, "OK - All registered adventurers", await _db.Adventurers
                                                                .Include(adventurer => adventurer.Race)
                                                                .Include(adventurer => adventurer.Class)
                                                                .Include(adventurer => adventurer.Party)
                                                                .ToListAsync());

    [HttpGet("{id}")]
    public async Task<Response<Adventurer>> GetAdventurerById(int id)
    {
        var adventurerToRead = await _db.Adventurers
                                        .Include(adventurer => adventurer.Race)
                                        .Include(adventurer => adventurer.Class)
                                        .Include(adventurer => adventurer.Party)
                                        .FirstOrDefaultAsync(adventurer => adventurer.Id == id);

        return adventurerToRead is not null
            ? new(200, $"OK - Adventurer with Id {id} found.", adventurerToRead)
            : new(404, $"NotFound - Adventurer with Id {id} not found.", null);
    }

    [HttpPost]
    public async Task<Response<Adventurer>> CreateAdventurer([FromBody] UpsertAdventurerRequest request)
    {
        var adventurerRace = _db.Races.FirstOrDefault(race => race.Name == request.RaceName);
        var adventurerClass = _db.Classes.FirstOrDefault(@class => @class.Name == request.ClassName);

        if (adventurerRace is null)
            return new(404, $"NotFound - Race with name {request.RaceName} does not exist.", null);

        if (adventurerClass is null)
            return new(404, $"NotFound - Class with name {request.ClassName} does not exist.", null);

        if (request.PartyId is not null)
        {
            var party = await _db.Parties.FindAsync(request.PartyId);

            if (party is null)
                return new(404, $"NotFound - Party with Id {request.PartyId} not found.", null);
        }

        var adventurerToCreate = new Adventurer
        {
            Name = request.Name,
            Rank = request.Rank,
            Race = adventurerRace,
            Class = adventurerClass,
            Party = request.PartyId is not null ? await _db.Parties.FindAsync(request.PartyId) : null
        };

        await _db.Adventurers.AddAsync(adventurerToCreate);
        await _db.SaveChangesAsync();

        return new(201, $"Created - Adventurer with Id {adventurerToCreate.Id} created.", adventurerToCreate);
    }

    [HttpPut("{id}")]
    public async Task<Response<Adventurer>> UpdateAdventurerById(int id, [FromBody] UpsertAdventurerRequest request)
    {
        var adventurerToUpdate = await _db.Adventurers.FindAsync(id);

        if (adventurerToUpdate is null)
            return new Response<Adventurer>(404, $"NotFound - Adventurer with Id {id} not found.", null);

        var adventurerRace = _db.Races.FirstOrDefault(race => race.Name == request.RaceName);
        var adventurerClass = _db.Classes.FirstOrDefault(@class => @class.Name == request.ClassName);

        if (adventurerRace is null)
            return new(404, $"NotFound - Race with name {request.RaceName} does not exist.", null);

        if (adventurerClass is null)
            return new(404, $"NotFound - Class with name {request.ClassName} does not exist.", null);

        if (request.PartyId is not null)
        {
            var party = await _db.Parties.FindAsync(request.PartyId);

            if (party is null)
                return new(400, $"BadRequest - Party with Id {request.PartyId} not found.", null);
        }

        adventurerToUpdate.Name = request.Name;
        adventurerToUpdate.Rank = request.Rank;
        adventurerToUpdate.Race = adventurerRace;
        adventurerToUpdate.Class = adventurerClass;
        adventurerToUpdate.Party = request.PartyId is not null ? await _db.Parties.FindAsync(request.PartyId) : null;

        _db.Adventurers.Update(adventurerToUpdate);
        await _db.SaveChangesAsync();

        return new(200, $"OK - Adventurer with Id {id} updated.", adventurerToUpdate);
    }

    [HttpDelete("{id}")]
    public async Task<Response<Adventurer>> DeleteAdventurerById(int id)
    {
        var adventurerToDelete = await _db.Adventurers.Include(adventurer => adventurer.Race)
                                                      .Include(adventurer => adventurer.Class)
                                                      .Include(adventurer => adventurer.Party)
                                                      .FirstOrDefaultAsync(adventurer => adventurer.Id == id);

        if (adventurerToDelete is null)
            return new Response<Adventurer>(404, $"NotFound - Adventurer with Id {id} not found.", null);

        _db.Adventurers.Remove(adventurerToDelete);
        await _db.SaveChangesAsync();

        return new(200, $"OK - Adventurer with Id {id} deleted.", adventurerToDelete);
    }
}