using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BFF.Component.Tests.Support;
using BFF.Users;
using FluentAssertions;
using Xunit;

namespace BFF.Component.Tests.Users;

public class CreateUserTest : ComponentTest
{
    public CreateUserTest(CompontentTestFixture fixture) : base(fixture) { }

    [Fact]
    public async Task CreatingAUserWithAnInvalidRequest_ReturnsA_400BadRequest()
    {
        using var response = await _client.PostAsJsonAsync("/public/v2/users", new NewUserRequest("", "", "", ""));
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreatingAValidUser_ReturnsA_20OK()
    {
        var response = await _client.PostAsJsonAsync("/public/v2/users", new NewUserRequest("tony", "tony@hotmail.com", "male", "active"));

        response.Should().HaveStatusCode(HttpStatusCode.OK);
        
        var responseBody = response.ContentAs<NewUserResponse>();
        responseBody.id.Should().BeGreaterThan(0);
        
        _db.Users.Count().Should().Be(1);
        var storedUser = _db.Users.First();
        storedUser.Email.Should().Be("tony@hotmail.com");
        responseBody.id.Should().Be(storedUser.Id);
    }
}