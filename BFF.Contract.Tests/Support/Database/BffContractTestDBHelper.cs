using System.IO;

namespace BFF.Contract.Tests.Support.Database;

public static class BffContractTestDbHelper
{ 
    public static string DatabaseFolderLocation => $"{Directory.GetCurrentDirectory()}/../../../Support/Database/";
}