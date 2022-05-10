using System.Linq;
using BFF.Support.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static BFF.Contract.Tests.Database.BffComponentTestDBHelper;

namespace BFF.Component.Tests.Support;

public class BFFComponentTestApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<BffDb>));
            services.Remove(descriptor);
            services.AddSqlite<BffDb>($"Data Source={DatabaseFolderLocation}bff.db;Cache=Shared");
        });
        return base.CreateHost(builder);
    }
}