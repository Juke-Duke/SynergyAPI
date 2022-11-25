using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Synergy.API.Database;
using Synergy.API.Entities;

namespace Synergy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly SynergyDbContext _db;

        public PartyController(SynergyDbContext db)
            => _db = db;

        [HttpGet]
        public async Task<Response<IReadOnlyCollection<Party>>> GetAllParties()
            => new(200, "OK - All registered parties", await _db.Parties.ToListAsync());

        [HttpGet("{id}")]
        public async Task<Response<Party>> GetPartyById(int id)
        {
            var partyToRead = await _db.Parties.FirstOrDefaultAsync(party => party.Id == id);

            return partyToRead is not null
                ? new(200, $"OK - Party with id {id} found.", partyToRead)
                : new(404, $"NotFound - Party with id {id} not found.", null);
        }

        [HttpPost("{adventurerId}")]
        public async Task<Response<Party>> CreateParty(int adventurerId, [FromBody] Party party)
        {
            var leader = await _db.Adventurers.FindAsync(adventurerId);

            if (leader is null)
                return new(404, $"NotFound - Adventurer with id {adventurerId} not found to be set as leader.", null);

            party.Leader = leader;
            party.DateFounded = DateTime.Now;

            await _db.Parties.AddAsync(party);
            await _db.SaveChangesAsync();

            return new(201, $"Created - Party with id {party.Id} created with adventurer {leader.Name} as leader.", party);
        }

        [HttpPut("{id}/{adventurerId}")]
        public async Task<Response<Party>> UpdateParty(int id, int adventurerId, [FromBody] Party party)
        {
            var partyToUpdate = await _db.Parties.FindAsync(id);

            if (partyToUpdate is null)
                return new(404, $"NotFound - Party with id {id} not found.", null);

            var leader = await _db.Adventurers.FindAsync(adventurerId);

            if (leader is null)
                return new(404, $"NotFound - Adventurer with id {adventurerId} not found.", null);

            if (partyToUpdate.Leader.Id != leader.Id)
                return new(400, $"BadRequest - The leader Id given does not match the leader Id of the party.", null);

            _db.Parties.Update(partyToUpdate);
            await _db.SaveChangesAsync();

            return new(200, $"OK - Party with id {id} updated.", partyToUpdate);
        }

        [HttpDelete("{id}/{adventurerId}")]
        public async Task<Response<Party>> DeleteParty(int id, int adventurerId)
        {
            var partyToDelete = await _db.Parties.FindAsync(id);

            if (partyToDelete is null)
                return new(404, $"NotFound - Party with id {id} not found.", null);

            var leader = await _db.Adventurers.FindAsync(adventurerId);

            if (leader is null)
                return new(404, $"NotFound - Adventurer with id {adventurerId} not found.", null);

            if (partyToDelete.Leader.Id != leader.Id)
                return new(400, $"BadRequest - The leader Id given does not match the leader Id of the party.", null);

            _db.Parties.Remove(partyToDelete);
            await _db.SaveChangesAsync();

            return new(200, $"OK - Party with id {id} deleted.", partyToDelete);
        }
    }
}