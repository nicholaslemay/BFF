using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BFF.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BFF.Component.Tests.Users;

public class BFFComponentTestApplication : WebApplicationFactory<Program>
{
}

public class CreateUserTest
{
    private readonly HttpClient _client;

    public CreateUserTest()
    {
        var application = new BFFComponentTestApplication();
        _client = application.CreateClient();
    }

    [Fact]
    public async Task CreatingAValidUser_ReturnsA_20OK()
    {
        var response = await _client.PostAsJsonAsync("/public/v2/users", new NewUserRequest("tony", "tony@hotmail.com", "male", "active"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<NewUserResponse>();
        body.id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task CreatinAUserWithAnInvalidRequest_ReturnsA_400BadRequest()
    {
        using var response = await _client.PostAsJsonAsync("/public/v2/users", new NewUserRequest("", "", "", ""));
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}