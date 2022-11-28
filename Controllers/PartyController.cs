using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Synergy.API.Database;
using Synergy.API.Entities;
using Synergy.API.Views;
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
        public async Task<Response<IReadOnlyCollection<FullPartyDTO>>> GetAllParties()
            => new(200, "OK - All registered parties", await _db.Parties.Include(party => party.Members)
                                                                        .ThenInclude(member => member.Race)
                                                                        .Include(party => party.Members)
                                                                        .ThenInclude(member => member.Class)
                                                                        .Select(party => new FullPartyDTO
                                                                        {
                                                                            Id = party.Id,
                                                                            Name = party.Name,
                                                                            DateFounded = party.DateFounded,
                                                                            Adventurers = party.Members.Select(member => new AdventurerDTO
                                                                            {
                                                                                Id = member.Id,
                                                                                Name = member.Name,
                                                                                Rank = member.Rank,
                                                                                Race = member.Race,
                                                                                Class = member.Class,
                                                                                Party = party.Name
                                                                            }).ToList()
                                                                        })
                                                                        .ToListAsync());
        [HttpGet("{id}")]
        public async Task<Response<FullPartyDTO>> GetPartyById(int id)
        {
            var partyToRead = await _db.Parties.Include(party => party.Members)
                                               .ThenInclude(member => member.Race)
                                               .Include(party => party.Members)
                                               .ThenInclude(member => member.Class)
                                               .Select(party => new FullPartyDTO
                                               {
                                                   Id = party.Id,
                                                   Name = party.Name,
                                                   DateFounded = party.DateFounded,
                                                   Adventurers = party.Members.Select(member => new AdventurerDTO
                                                   {
                                                       Id = member.Id,
                                                       Name = member.Name,
                                                       Rank = member.Rank,
                                                       Race = member.Race,
                                                       Class = member.Class,
                                                       Party = party.Name
                                                   }).ToList()
                                               })
                                               .FirstOrDefaultAsync(party => party.Id == id);

            return partyToRead is not null
                ? new(200, $"OK - Party with id {id} found.", partyToRead)
                : new(404, $"NotFound - Party with id {id} not found.", null);
        }

        [HttpPost]
        public async Task<Response<FullPartyDTO>> CreatePartyByName([FromBody] UpsertPartyRequest request)
        {
            var partyToCreate = new Party
            {
                Name = request.Name,
                DateFounded = DateTime.Now
            };

            await _db.Parties.AddAsync(partyToCreate);
            await _db.SaveChangesAsync();

            var createdParty = new FullPartyDTO
            {
                Id = partyToCreate.Id,
                Name = partyToCreate.Name,
                DateFounded = partyToCreate.DateFounded
            };

            return new(201, $"Created - Party with name {request.Name} created.", createdParty);
        }

        [HttpPut("{id}")]
        public async Task<Response<FullPartyDTO>> UpdatePartyById(int id, [FromBody] UpsertPartyRequest request)
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

            var updatedParty = new FullPartyDTO
            {
                Id = partyToUpdate.Id,
                Name = partyToUpdate.Name,
                DateFounded = partyToUpdate.DateFounded,
                Adventurers = partyToUpdate.Members.Select(member => new AdventurerDTO
                {
                    Id = member.Id,
                    Name = member.Name,
                    Rank = member.Rank,
                    Race = member.Race,
                    Class = member.Class,
                    Party = partyToUpdate.Name
                }).ToList()
            };

            return new(200, $"OK - Party with id {id} updated.", updatedParty);
        }

        [HttpDelete("{id}")]
        public async Task<Response<FullPartyDTO>> DeletePartyById(int id)
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

            var deletedParty = new FullPartyDTO
            {
                Id = partyToDelete.Id,
                Name = partyToDelete.Name,
                DateFounded = partyToDelete.DateFounded,
                Adventurers = partyToDelete.Members.Select(member => new AdventurerDTO
                {
                    Id = member.Id,
                    Name = member.Name,
                    Rank = member.Rank,
                    Race = member.Race,
                    Class = member.Class,
                    Party = partyToDelete.Name
                }).ToList()
            };

            return new(200, $"OK - Party with id {id} deleted.", deletedParty);
        }
    }
}