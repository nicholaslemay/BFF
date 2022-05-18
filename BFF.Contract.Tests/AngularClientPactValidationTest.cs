using BFF.Contract.Tests.Support;
using PactNet;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;
using Xunit.Abstractions;
using static BFF.Contract.Tests.Support.PactHelper;

namespace BFF.Contract.Tests;

public class AngularClientPactValidationTest
{
    private readonly ITestOutputHelper _output;
    private readonly WireMockServer _fakeCommunicationService;

    public AngularClientPactValidationTest(ITestOutputHelper output)
    {
        _output = output;
        _fakeCommunicationService = WireMockServer.Start(777);
        GivenCallsToCommunicationServiceSucced();
    }

    [Fact]
    public void RespectContractsWithAngularCLient()
    {
        var app = new BffContractTestApplication();
        app.Run();

        new PactVerifier(BffPactVerifierConfig.Build(_output))
            .ServiceProvider("BFF", BffContractTestApplication.BaseUrl)
            .HonoursPactWith("Angular")
            .PactUri($"{PactFolderLocation}/Angular/angular-bff.json")
            .Verify();
        
        _fakeCommunicationService.Stop();
    }

    private void GivenCallsToCommunicationServiceSucced()
    {
        _fakeCommunicationService.Given(Request
                .Create()
                .WithPath("/accountCreationConfirmation")
                .UsingPost())
            .RespondWith(Response
                .Create()
                .WithStatusCode(201));
    }
}