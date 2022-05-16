using System.Net.Http;
using System.Net.Http.Json;

namespace BFF.Component.Tests.Support;

public static class Extensions
{
    public static T ContentAs<T>(this HttpResponseMessage response) => 
        response.Content.ReadFromJsonAsync<T>().Result;
}