using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Synergy.API.Database;
using Synergy.API.Entities;

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
        => new(200, "OK - All registered adventurers", await _db.Adventurers.ToListAsync());

    [HttpGet("{id}")]
    public async Task<Response<Adventurer>> GetAdventurerById(int id)
    {
        var adventurerToRead = await _db.Adventurers.FindAsync();

        return adventurerToRead is not null
            ? new(200, $"OK - Adventurer with ID {id} found.", adventurerToRead)
            : new(404, $"NotFound - Adventurer with ID {id} not found.", null);
    }

    [HttpPost]
    public async Task<Response<Adventurer>> CreateAdventurer([FromBody] Adventurer adventurer)
    {
        await _db.Adventurers.AddAsync(adventurer);
        await _db.SaveChangesAsync();

        return new(201, $"Created - Adventurer with ID {adventurer.Id} created.", adventurer);
    }

    [HttpPut("{id}")]
    public async Task<Response<Adventurer>> UpdateAdventurerById(int id, [FromBody] Adventurer adventurer)
    {
        var adventurerToUpdate = await _db.Adventurers.FindAsync(id);

        if (adventurerToUpdate is null)
            return new Response<Adventurer>(404, $"NotFound - Adventurer with ID {id} not found.", null);

        _db.Adventurers.Update(adventurer);
        await _db.SaveChangesAsync();

        return new(200, $"OK - Adventurer with ID {id} updated.", adventurer);
    }

    [HttpDelete("{id}")]
    public async Task<Response<Adventurer>> DeleteAdventurerById(int id)
    {
        var adventurerToDelete = await _db.Adventurers.FindAsync(id);

        if (adventurerToDelete is null)
            return new Response<Adventurer>(404, $"NotFound - Adventurer with ID {id} not found.", null);

        _db.Adventurers.Remove(adventurerToDelete);
        await _db.SaveChangesAsync();

        return new(200, $"OK - Adventurer with ID {id} deleted.", adventurerToDelete);
    }
}