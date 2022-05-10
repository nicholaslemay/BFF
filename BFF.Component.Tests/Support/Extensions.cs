using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace BFF.Component.Tests.Support;

public static class Extensions
{
    public static T ContentAs<T>(this HttpResponseMessage response) => 
        response.Content.ReadFromJsonAsync<T>().Result;
}