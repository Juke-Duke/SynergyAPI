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
    public async Task<ActionResult<IReadOnlyCollection<Adventurer>>> GetAllAdventurers()
        => Ok(await _db.Adventurers.ToListAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Adventurer>> GetAdventurerById(int id)
    {
        var adventurerToRead = await _db.Adventurers.FindAsync();

        return adventurerToRead is not null ? Ok(adventurerToRead) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Adventurer>> CreateAdventurer(Adventurer adventurer)
    {
        await _db.Adventurers.AddAsync(adventurer);
        await _db.SaveChangesAsync();

        return Created($"api/adventurer/{adventurer.Id}", adventurer);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Adventurer>> UpdateAdventurerById(int id, Adventurer adventurer)
    {
        var adventurerToUpdate = await _db.Adventurers.FindAsync(id);

        if (adventurerToUpdate is null)
            return NotFound();

        _db.Adventurers.Update(adventurerToUpdate);
        await _db.SaveChangesAsync();

        return Ok(adventurer);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Adventurer>> DeleteAdventureById(int id)
    {
        var adventurerToDelete = await _db.Adventurers.FindAsync(id);

        if (adventurerToDelete is null)
            return NotFound();

        _db.Adventurers.Remove(adventurerToDelete);
        await _db.SaveChangesAsync();

        return Ok(adventurerToDelete);
    }
}