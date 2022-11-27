using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Synergy.API.Database;
using Synergy.API.Entities;
using Synergy.API.Requests;

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
            => new(200, "OK - All registered parties", await _db.Parties.Include(party => party.Members)
                                                                        .ThenInclude(member => member.Race)
                                                                        .Include(party => party.Members)
                                                                        .ThenInclude(member => member.Class)
                                                                        .ToListAsync());
        [HttpGet("{id}")]
        public async Task<Response<Party>> GetPartyById(int id)
        {
            var partyToRead = await _db.Parties.Include(party => party.Members)
                                               .ThenInclude(member => member.Race)
                                               .Include(party => party.Members)
                                               .ThenInclude(member => member.Class)
                                               .FirstOrDefaultAsync(party => party.Id == id);

            return partyToRead is not null
                ? new(200, $"OK - Party with id {id} found.", partyToRead)
                : new(404, $"NotFound - Party with id {id} not found.", null);
        }

        [HttpPost]
        public async Task<Response<Party>> CreatePartyByName([FromBody] UpsertPartyRequest request)
        {
            var partyToCreate = new Party
            {
                Name = request.Name,
                DateFounded = DateTime.Now
            };

            await _db.Parties.AddAsync(partyToCreate);
            await _db.SaveChangesAsync();

            return new(201, $"Created - Party with name {request.Name} created.", partyToCreate);
        }

        [HttpPut("{id}")]
        public async Task<Response<Party>> UpdatePartyById(int id, [FromBody] UpsertPartyRequest request)
        {
            var partyToUpdate = await _db.Parties.Include(party => party.Members)
                                                 .ThenInclude(member => member.Race)
                                                 .Include(party => party.Members)
                                                 .ThenInclude(member => member.Class)
                                                 .FirstOrDefaultAsync(party => party.Id == id);

            if (partyToUpdate is null)
                return new(404, $"NotFound - Party with id {id} not found.", null);

            partyToUpdate.Name = request.Name;

            _db.Parties.Update(partyToUpdate);
            await _db.SaveChangesAsync();

            return new(200, $"OK - Party with id {id} updated.", partyToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<Response<Party>> DeletePartyById(int id)
        {
            var partyToDelete = await _db.Parties.Include(party => party.Members)
                                                 .ThenInclude(member => member.Race)
                                                 .Include(party => party.Members)
                                                 .ThenInclude(member => member.Class)
                                                 .FirstOrDefaultAsync(party => party.Id == id);

            if (partyToDelete is null)
                return new(404, $"NotFound - Party with id {id} not found.", null);

            _db.Parties.Remove(partyToDelete);
            await _db.SaveChangesAsync();

            return new(200, $"OK - Party with id {id} deleted.", partyToDelete);
        }
    }
}