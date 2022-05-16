using System.Threading.Tasks;
using PactNet;
using Xunit;
using Xunit.Abstractions;
using static BFF.Contract.Tests.PactHelper;

namespace BFF.Contract.Tests;

public class AngularClientPactValidationTest
{
    private readonly ITestOutputHelper output;

    public AngularClientPactValidationTest(ITestOutputHelper output) => this.output = output;

    [Fact]
    public async Task RespectContractsWithAngularCLient()
    {
        var app = new BffContractTestApplication();
        app.Run();

        new PactVerifier(BffPactVerifierConfig.Build(output))
            .ServiceProvider("BFF", BffContractTestApplication.BaseUrl)
            .HonoursPactWith("Angular")
            .PactUri($"{PactFolderLocation}/Angular/angular-bff.json")
            .Verify();
    }
}