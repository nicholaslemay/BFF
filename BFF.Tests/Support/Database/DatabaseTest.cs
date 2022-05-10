using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BFF.Support.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace BFF.Tests.Support.Database;

public class DatabaseCleaner{
    private readonly BffDb _db;

    public DatabaseCleaner(BffDb db) => _db = db;

    public void CleanDB() => CleanDBAsync().Wait();

    private async Task CleanDBAsync() => await _db.Database.ExecuteSqlRawAsync("DELETE FROM Users;");
}
public class BFFDatabaseTestApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<BffDb>));
            services.Remove(descriptor);
            services.AddSqlite<BffDb>($"Data Source={BFFTestDBHelper.DatabaseFolderLocation}bff.db;Cache=Shared");
        });
        return base.CreateHost(builder);
    }
}
public class DatabaseTestFixture : IDisposable
{
    public DatabaseTestFixture()
    {
        Application = new BFFDatabaseTestApplication();
        Client = Application.CreateClient();
        DB = Application.Services.CreateScope().ServiceProvider.GetRequiredService<BffDb>();
        DatabaseCleaner = new DatabaseCleaner(DB);
    }

    public void Dispose()
    {
        Application.Dispose();
    }

    public readonly HttpClient Client;
    public readonly BFFDatabaseTestApplication Application;
    public readonly DatabaseCleaner DatabaseCleaner;
    public readonly BffDb DB;
}

[CollectionDefinition("DatabaseTest")]
public class DatabaseTestCollection : ICollectionFixture<DatabaseTestFixture> { }

[Collection("DatabaseTest")]
public abstract class DatabaseTest
{
    protected readonly BffDb _db;

    protected DatabaseTest(DatabaseTestFixture fixture)
    {
        _db = fixture.DB;
        fixture.DatabaseCleaner.CleanDB();
    }
}