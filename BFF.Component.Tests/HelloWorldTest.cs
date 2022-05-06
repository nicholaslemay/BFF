using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BFF.Component.Tests;

public class BFFComponentTestApplication : WebApplicationFactory<Program>
{
}

public class HelloWorldTest
{
    [Fact]
    public async Task CanGetAHelloWorld()
    {
        await using var application = new BFFComponentTestApplication();
        using var client = application.CreateClient();
        using var response = await client.GetAsync("/");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Hello World!", await response.Content.ReadAsStringAsync());
    }
}