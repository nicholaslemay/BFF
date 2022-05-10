using System.IO;

namespace BFF.Contract.Tests.Database;

public class BffComponentTestDBHelper
{ 
    public static string DatabaseFolderLocation => $"{Directory.GetCurrentDirectory()}/../../../Support/Database/";
}