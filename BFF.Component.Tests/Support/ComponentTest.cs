using System;
using System.Net.Http;
using BFF.Database.Migrations;
using BFF.Support.Database;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Server;
using Xunit;

namespace BFF.Component.Tests.Support;

public class CompontentTestFixture : IDisposable
{
    public CompontentTestFixture()
    {
        Application = new BFFComponentTestApplication();
        Client = Application.CreateClient();
        DB = Application.Services.CreateScope().ServiceProvider.GetRequiredService<BffDb>();
        DatabaseCleaner = new DatabaseCleaner(DB);
        BffDatabaseMigration = new BffDatabaseMigration(DB);
        FakeCommunicationService = WireMockServer.Start(777);
    }

    public void Dispose()
    {
        FakeCommunicationService.Stop();
        Application.Dispose();
    }

    public readonly HttpClient Client;
    private readonly BFFComponentTestApplication Application;
    public readonly DatabaseCleaner DatabaseCleaner;
    public readonly BffDb DB;
    public readonly BffDatabaseMigration BffDatabaseMigration;
    public readonly WireMockServer FakeCommunicationService;
}

[CollectionDefinition("ComponentTest")]
public class ComponentTestCollection : ICollectionFixture<CompontentTestFixture> { }

[Collection("ComponentTest")]
public abstract class ComponentTest
{
    protected readonly HttpClient Client;
    protected readonly BffDb Db;
    protected readonly WireMockServer FakeCommunicationService;

    protected ComponentTest(CompontentTestFixture fixture)
    {
        Client = fixture.Client;
        Db = fixture.DB;
        FakeCommunicationService = fixture.FakeCommunicationService;
        
        FakeCommunicationService.ResetLogEntries();
        fixture.BffDatabaseMigration.Migrate();
        fixture.DatabaseCleaner.CleanDB();
    }
}