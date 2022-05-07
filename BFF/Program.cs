var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/public/v2/users", () => new {Id = new Random().Next()});
app.Run();

public partial class Program { }