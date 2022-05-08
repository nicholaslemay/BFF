using BFF.Users;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<Program>(lifetime: ServiceLifetime.Transient);

var app = builder.Build();
app.MapUserEndpoints()
   .Run();

public partial class Program { }
