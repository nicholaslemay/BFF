using BFF.Support.Database;
using BFF.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<Program>(lifetime: ServiceLifetime.Transient);


builder.Services.AddSqlite<BffDb>($"Data Source=/Users/nick/RiderProjects/BDC/BFF/BFF/Support/Database/bff.db;Cache=Shared");

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();
await EnsureDb(app.Services, app.Logger);




app.MapUserEndpoints()
   .Run();

async Task EnsureDb(IServiceProvider services, ILogger logger)
{
   await using var db = services.CreateScope().ServiceProvider.GetRequiredService<BffDb>();
   if (db.Database.IsRelational())
   {
      logger.LogInformation("Migrating database...");
      await db.Database.MigrateAsync();
      logger.LogInformation("Migrated database");
   }
}
public partial class Program { }
