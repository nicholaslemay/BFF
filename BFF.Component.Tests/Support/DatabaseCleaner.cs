using System.Threading.Tasks;
using BFF.Support.Database;
using Microsoft.EntityFrameworkCore;

namespace BFF.Component.Tests.Support;

public class DatabaseCleaner{
    private readonly BffDb _db;

    public DatabaseCleaner(BffDb db) => _db = db;

    public void CleanDB() => CleanDBAsync().Wait();

    private async Task CleanDBAsync() => await _db.Database.ExecuteSqlRawAsync("DELETE FROM Users;");
}