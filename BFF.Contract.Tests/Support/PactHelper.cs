using System.IO;

namespace BFF.Contract.Tests;

public static class PactHelper
{
    public static string PactFolderLocation => $"{Directory.GetCurrentDirectory()}/../../../Pacts";
}