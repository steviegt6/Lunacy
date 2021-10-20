using System;
using System.IO;
using System.Threading.Tasks;

namespace Lunacy.Patch;

public static class Program
{
    /* LunarClient Electron app folder structure:
     *  lunarclient (base)
     *  >/resources
     *    >/app.asar (what we want)
     *    >/app (directory we want to install to)
     *
     * Goal is to retrieve all files in lunarclient, and to extract the app.asar file.
     */
    public static async Task Main()
    {
        Console.WriteLine("Welcome to the Lunacy developer tool setup.");
        Console.WriteLine("Please input the base path of your Lunar Client installation:");
        string? basePath = Console.ReadLine();

        if (basePath is null)
            throw new Exception("Path input was null!");

        if (!Directory.Exists(basePath))
            throw new Exception("Folder does not exist.");

        if (!Directory.Exists(Path.Combine(basePath, "resources")))
            throw new Exception("No resources sub-folder found!");

        if (!File.Exists(Path.Combine(basePath, "resources", "app.asar")))
            throw new Exception("No ASAR archive found underneath resources!");

        Console.WriteLine("Path validated, coping all files to \"client/\", this may take a moment...");
        CopyDirectory(basePath, "client");

        Console.WriteLine("Copying complete!");
        Console.WriteLine("Proceeding to unpack the ASAR...");

        await Lunacy.Program.Main(new[]
        {
            "asar-unpack",
            "-p", Path.Combine("client", "resources", "app.asar"),
            "-d", Path.Combine("client", "resources", "app")
        });

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
    }

    // Recursively copy directory files
    private static void CopyDirectory(string source, string dest)
    {
        DirectoryInfo dir = new(source);
        DirectoryInfo[] dirs = dir.GetDirectories();
        FileInfo[] files = dir.GetFiles();

        Directory.CreateDirectory(dest);

        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(dest, file.Name);
            file.CopyTo(tempPath, false);
        }

        foreach (DirectoryInfo subDirectory in dirs)
        {
            string tempPath = Path.Combine(dest, subDirectory.Name);
            CopyDirectory(subDirectory.FullName, tempPath);
        }
    }
}