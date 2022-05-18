using System.Collections.Generic;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit.Abstractions;

namespace BFF.Contract.Tests.Support;

public static class BffPactVerifierConfig
{
    public static PactVerifierConfig Build(ITestOutputHelper output) => new()
    {
        Outputters = new List<IOutput>
        {
            new XUnitOutput(output)
        }
    };
}