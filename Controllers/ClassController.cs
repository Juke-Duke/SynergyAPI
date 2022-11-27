using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Synergy.API.Database;
using Synergy.API.Entities;

namespace Synergy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassController : ControllerBase
{
    private readonly SynergyDbContext _db;

    public ClassController(SynergyDbContext db)
        => _db = db;

    [HttpGet]
    public async Task<Response<IReadOnlyCollection<Class>>> GetAllClasses()
        => new(200, "OK - All available classes", await _db.Classes.ToListAsync());

    [HttpGet("{name}")]
    public async Task<Response<Class>> GetClassByName(string name)
    {
        var classToRead = await _db.Classes.FirstOrDefaultAsync(@class => @class.Name == name);

        return classToRead is not null
            ? new(200, $"OK - Class with name {name} found.", classToRead)
            : new(404, $"NotFound - Class with name {name} not found.", null);
    }
}