using System;
using System.IO;
using System.Threading.Tasks;

namespace Lunacy.Patch;

public static class Program
{
    public static string SrcCopy => "original";

    public static string Patched => Path.Combine("client", "resources", "app");

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
        Console.WriteLine("Welcome to the Lunacy developer tool.");

        Console.WriteLine(
            "Press 'c' to continue to the developer setup, 'p' to apply patches, 'd' to diff patches, or 'e' to exit"
        );

        char key = ReadInput();

        switch (char.ToLower(key))
        {
            case 'c':
                await PromptSetup();
                break;

            case 'p':
                await DoPatch();
                break;

            case 'd':
                await DoDiff();
                break;

            case 'e':
                break;
        }
    }

    private static async Task DoPatch()
    {
        Directory.CreateDirectory("Patches");

        await new LunacyDiffer().DiffFolders(
            new DirectoryInfo(SrcCopy),
            new DirectoryInfo(Patched),
            new DirectoryInfo("Patches")
        );

        await Main();
    }

    private static async Task DoDiff()
    {
        Directory.CreateDirectory("Patches");

        await new LunacyPatcher().Patch(
            new DirectoryInfo("Patches"),
            new DirectoryInfo("Patched")
        );

        await Main();
    }

    private static async Task PromptSetup()
    {
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
            "-d", Patched
        });

        Console.WriteLine("Creating copy in root repository...");
        CopyDirectory(Path.Combine("client", "resources", "app"), "original");

        await Main();
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

    public static char ReadInput() =>
        !Console.IsInputRedirected
            ? Console.ReadKey(true).KeyChar
            : (Console.ReadLine() ?? throw new Exception("Invalid input."))[0];
}