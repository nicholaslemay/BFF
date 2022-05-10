using System;
using System.Net.Http;
using BFF.Support.Database;
using Microsoft.Extensions.DependencyInjection;
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
    }

    public void Dispose()
    {
        Application.Dispose();
    }

    public readonly HttpClient Client;
    public readonly BFFComponentTestApplication Application;
    public readonly DatabaseCleaner DatabaseCleaner;
    public readonly BffDb DB;
}

[CollectionDefinition("ComponentTest")]
public class ComponentTestCollection : ICollectionFixture<CompontentTestFixture> { }

[Collection("ComponentTest")]
public abstract class ComponentTest
{
    protected readonly HttpClient _client;
    protected readonly BffDb _db;

    protected ComponentTest(CompontentTestFixture fixture)
    {
        _client = fixture.Client;
        _db = fixture.DB;
        fixture.DatabaseCleaner.CleanDB();
    }
}