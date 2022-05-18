using System.IO;

namespace BFF.Contract.Tests.Support;

public static class PactHelper
{
    public static string PactFolderLocation => $"{Directory.GetCurrentDirectory()}/../../../Pacts";
}