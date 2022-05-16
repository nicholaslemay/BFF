using BFF.Support.Database;
using BFF.Support.Database.Migrations;
using BFF.Users;
using FluentValidation;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<Program>(lifetime: ServiceLifetime.Transient);

Console.WriteLine($"Data Source={DBHelper.DatabaseFolderLocation}bff.db");
builder.Services.AddSqlite<BffDb>($"Data Source={DBHelper.DatabaseFolderLocation}bff.db;Cache=Shared");

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.ValidateNoPendingMigrations()
   .MapUserEndpoints()
   .Run();

namespace BFF
{
   public partial class Program { }
}


