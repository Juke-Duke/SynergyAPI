using System.Text.Json.Serialization;
using Synergy.API.Database;
using Synergy.API.Database.Seeding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.Converters
                .Add(new JsonStringEnumConverter()));

builder.Services.AddDbContext<SynergyDbContext>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<SynergyDbContext>();
//     RaceSeed.Seed(db);
//     ClassSeed.Seed(db);
// }

app.Run();
