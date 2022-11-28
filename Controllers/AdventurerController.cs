using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Synergy.API.Database;
using Synergy.API.Entities;
using Synergy.API.Views;
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
    public async Task<Response<IReadOnlyCollection<FullAdventurerDTO>>> GetAllAdventurers()
        => new(200, "OK - All registered adventurers", await _db.Adventurers
                                                                .Include(adventurer => adventurer.Race)
                                                                .Include(adventurer => adventurer.Class)
                                                                .Select(adventurer => new FullAdventurerDTO
                                                                {
                                                                    Id = adventurer.Id,
                                                                    Name = adventurer.Name,
                                                                    Rank = adventurer.Rank,
                                                                    Race = adventurer.Race,
                                                                    Class = adventurer.Class,
                                                                    Party = adventurer.Party != null
                                                                        ? new PartyDTO
                                                                        {
                                                                            Id = adventurer.Party.Id,
                                                                            Name = adventurer.Party.Name,
                                                                            DateFounded = adventurer.Party.DateFounded
                                                                        }
                                                                        : null
                                                                })
                                                                .ToListAsync());

    [HttpGet("{id}")]
    public async Task<Response<FullAdventurerDTO>> GetAdventurerById(int id)
    {
        var adventurerToRead = await _db.Adventurers
                                        .Include(adventurer => adventurer.Race)
                                        .Include(adventurer => adventurer.Class)
                                        .Select(adventurer => new FullAdventurerDTO
                                        {
                                            Id = adventurer.Id,
                                            Name = adventurer.Name,
                                            Rank = adventurer.Rank,
                                            Race = adventurer.Race,
                                            Class = adventurer.Class,
                                            Party = adventurer.Party != null
                                                ? new PartyDTO
                                                {
                                                    Id = adventurer.Party.Id,
                                                    Name = adventurer.Party.Name,
                                                    DateFounded = adventurer.Party.DateFounded
                                                }
                                                : null
                                        })
                                        .FirstOrDefaultAsync(adventurer => adventurer.Id == id);

        return adventurerToRead is not null
            ? new(200, $"OK - Adventurer with Id {id} found.", adventurerToRead)
            : new(404, $"NotFound - Adventurer with Id {id} not found.", null);
    }

    [HttpPost]
    public async Task<Response<FullAdventurerDTO>> CreateAdventurer([FromBody] UpsertAdventurerRequest request)
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

        var createdAdventurer = new FullAdventurerDTO
        {
            Name = adventurerToCreate.Name,
            Rank = adventurerToCreate.Rank,
            Race = adventurerRace,
            Class = adventurerClass,
            Party = adventurerToCreate.Party != null
                ? new PartyDTO
                {
                    Id = adventurerToCreate.Party.Id,
                    Name = adventurerToCreate.Party.Name,
                    DateFounded = adventurerToCreate.Party.DateFounded
                }
                : null
        };

        return new(201, $"Created - Adventurer with Id {adventurerToCreate.Id} created.", createdAdventurer);
    }

    [HttpPut("{id}")]
    public async Task<Response<FullAdventurerDTO>> UpdateAdventurerById(int id, [FromBody] UpsertAdventurerRequest request)
    {
        var adventurerToUpdate = await _db.Adventurers.FindAsync(id);

        if (adventurerToUpdate is null)
            return new(404, $"NotFound - Adventurer with Id {id} not found.", null);

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

        var updatedAdventurer = new FullAdventurerDTO
        {
            Name = adventurerToUpdate.Name,
            Rank = adventurerToUpdate.Rank,
            Race = adventurerRace,
            Class = adventurerClass,
            Party = adventurerToUpdate.Party != null
                ? new PartyDTO
                {
                    Id = adventurerToUpdate.Party.Id,
                    Name = adventurerToUpdate.Party.Name,
                    DateFounded = adventurerToUpdate.Party.DateFounded
                }
                : null
        };

        return new(200, $"OK - Adventurer with Id {id} updated.", updatedAdventurer);
    }

    [HttpDelete("{id}")]
    public async Task<Response<FullAdventurerDTO>> DeleteAdventurerById(int id)
    {
        var adventurerToDelete = await _db.Adventurers.Include(adventurer => adventurer.Race)
                                                      .Include(adventurer => adventurer.Class)
                                                      .Include(adventurer => adventurer.Party)
                                                      .FirstOrDefaultAsync(adventurer => adventurer.Id == id);

        if (adventurerToDelete is null)
            return new(404, $"NotFound - Adventurer with Id {id} not found.", null);

        _db.Adventurers.Remove(adventurerToDelete);
        await _db.SaveChangesAsync();

        var deletedAdventurer = new FullAdventurerDTO
        {
            Name = adventurerToDelete.Name,
            Rank = adventurerToDelete.Rank,
            Race = adventurerToDelete.Race,
            Class = adventurerToDelete.Class,
            Party = adventurerToDelete.Party != null
                ? new PartyDTO
                {
                    Id = adventurerToDelete.Party.Id,
                    Name = adventurerToDelete.Party.Name,
                    DateFounded = adventurerToDelete.Party.DateFounded
                }
                : null
        };

        return new(200, $"OK - Adventurer with Id {id} deleted.", deletedAdventurer);
    }
}