using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BFF.Communications;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Models;
using Xunit;

namespace BFF.Tests.Communications;

public class CommunicationServiceClientContractTest
{
    private readonly IMockProviderService? _mockCommunicationService;
    private readonly PactBuilder _pactBuilder;

    public CommunicationServiceClientContractTest()
    {
        _pactBuilder = new PactBuilder(new PactConfig { PactDir = @"..\..\..\Communications\pacts", LogDir = @"...\..\..\Communications\logs" });
        
        _pactBuilder
            .ServiceConsumer("BFF")
            .HasPactWith("Communication API");
        _mockCommunicationService = _pactBuilder.MockService(777,host: IPAddress.Any);
    }

    [Fact]
    public async Task DoTheDew()
    {
        _mockCommunicationService
            .Given("Creating an account creation confirmation ")
            .UponReceiving("A valid POST request")
            .With(new ProviderServiceRequest
            {
                Method = HttpVerb.Post,
                Path = "/accountCreationConfirmation",
                Headers = new Dictionary<string, object>
                {
                    { "Content-Type", "application/json; charset=utf-8" }
                },
                Body = new
                {
                    email = "bob@hotmail.com",
                    name = "robert"
                }
            }).WillRespondWith(new ProviderServiceResponse
            {
                Status = 201
            });

        var client = new CommunicationServiceClient(NewHttpClient());
        await client.SendAccountCreationConfirmationAsync(new AccountCreationConfirmation("bob@hotmail.com", "robert"));
       
        _mockCommunicationService.VerifyInteractions();
        _pactBuilder.Build();
    }

    private static HttpClient NewHttpClient() => new HttpClient { BaseAddress = new Uri("http://localhost:777/") };
}