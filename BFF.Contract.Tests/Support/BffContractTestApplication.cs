using System;
using System.Linq;
using System.Net;
using BFF.Contract.Tests.Database;
using BFF.Support.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BFF.Contract.Tests;

public class BffContractTestApplication : WebApplicationFactory<Program>
{
    public static string BaseUrl => "http://localhost:56566";
    
    private bool _disposed;
    private IHost? _host;

    public void Run() => EnsureServer();

    public override IServiceProvider Services
    {
        get
        {
            EnsureServer();
            return _host!.Services!;
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureKestrel(
            options => options.Listen(IPAddress.Any, 56566));
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<BffDb>));
            services.Remove(descriptor);
            services.AddSqlite<BffDb>($"Data Source={BffContractTestDBHelper.DatabaseFolderLocation}bff.db;Cache=Shared");
        });
        
        // Create the host for TestServer now before we
        // modify the builder to use Kestrel instead.
        var testHost = builder.Build();

        // Modify the host builder to use Kestrel instead
        // of TestServer so we can listen on a real address.
        // Create and start the Kestrel server before the test server,
        // otherwise due to the way the deferred host builder works
        // for minimal hosting, the server will not get "initialized
        // enough" for the address it is listening on to be available.
        builder.ConfigureWebHost((p) => p.UseKestrel());
        _host = builder.Build();
        _host.Start();
        
        //testHost.Start();
        return testHost;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (_disposed) return;
        
        if (disposing) 
            _host?.Dispose();

        _disposed = true;
    }

    private void EnsureServer()
    {
        // This forces WebApplicationFactory to bootstrap the server
        using var _ = CreateDefaultClient();
    }
}