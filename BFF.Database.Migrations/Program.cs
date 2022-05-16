// See https://aka.ms/new-console-template for more information

using BFF.Database.Migrations;
using BFF.Support.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var app = new BFFDatabaseTestApplication();
var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<BffDb>();

new BffDatabaseMigration(db).Migrate();
public class BFFDatabaseTestApplication : WebApplicationFactory<BFF.Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("DatabaseMigration");
        return base.CreateHost(builder);
    }
}

