#region License
// Pending license, modified from Assaturers/Assatur (https://github.com/Assaturers/Assatur)
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiffPatch;
using ModdingToolkit.Diffing;
using Webmilio.Commons.Console;
using Webmilio.Commons.Extensions;

namespace Lunacy.Patch
{
    public class LunacyDiffer : IDiffer
    {
        public const string PatchExtension = ".patch";
        public const string DeleteExtension = ".d";
        public const string CreateExtension = ".c";

        // Currently only checks file extensions
        // TODO: Make this not suck?
        public readonly List<string> FilesToIgnoreDiff = new() { ".dll", ".pak", ".dat", ".map", ".exe", ".ico" };

        public async Task DiffFolders(DirectoryInfo origin, DirectoryInfo updated, DirectoryInfo patches)
        {
            // ensure all folders exist
            origin.Create();
            updated.Create();
            patches.Create();

            Console.WriteLine($"Diffing {origin} to {updated} using {patches}.");

            string[] originalFiles = Directory.GetFiles(origin.FullName, "*.*", SearchOption.AllDirectories);
            string[] updatedFiles = Directory.GetFiles(updated.FullName, "*.*", SearchOption.AllDirectories);

            List<string> strippedOriginal = SelectFilter(originalFiles, origin);
            List<string> strippedUpdated = SelectFilter(updatedFiles, updated);

            IList<string> toDiff = strippedOriginal.Where(x => !FilesToIgnoreDiff.Any(x.EndsWith)).ToList();
            IList<string> toCreate = strippedUpdated
                .Where(su => !strippedOriginal.Any(so => so.Equals(su, StringComparison.OrdinalIgnoreCase))).ToArray();
            IList<string> toDelete = strippedOriginal
                .Where(so => !strippedUpdated.Any(su => su.Equals(so, StringComparison.OrdinalIgnoreCase))).ToArray();

            patches.Recreate(true);

            LineMatchedDiffer differ = new() { MaxMatchOffset = 10 };
            await toDiff.DoEnumerableAsync(p => Diff(differ, origin.FullName, updated.FullName, patches.FullName, p));
            await toCreate.DoAsync(p => WriteCreatePatch(updated.FullName, patches.FullName, p));
            await toDelete.DoAsync(p => WriteDeletePatch(patches.FullName, p));

            await Task.CompletedTask;
        }

        private static List<string> SelectFilter(IList<string> collection, FileSystemInfo root)
        {
            List<string> items = new(collection.Count);

            collection.Do(i =>
            {
                string n = StripPath(i, root.FullName);

                if (n.StartsWith('.') || n.StartsWith("bin") || n.StartsWith("obj") || n.Contains("node_modules"))
                    return;

                items.Add(n);
            });

            return items;
        }

        private static string StripPath(string path, string root) => path.Remove(0, root.Length + 1);

        private static async Task Diff(Differ differ, string originalRoot, string destinationRoot, string patchRoot,
            string shortName)
        {
            try
            {
                // Console.WriteLine($"Diff data: {originalRoot}, {destinationRoot}. {patchRoot}, {shortName}");

                string destinationPath = Path.Combine(destinationRoot, shortName);

                if (!File.Exists(destinationPath))
                    return;

                PatchFile diff = differ.DiffFile(Path.Combine(originalRoot, shortName), destinationPath, 3,
                    includePaths: false);

                if (!diff.IsEmpty)
                {
                    shortName += PatchExtension;
                    await WriteDiffPatch(patchRoot, shortName, diff.ToString());
                }
            }
            catch (Exception e)
            {
                ConsoleHelper.WriteLineError($"Diff failed due to exception with {shortName}: {e}");
            }
        }


        private static async Task WriteDiffPatch(string destRoot, string file, string content) => await WritePatch(
            Path.Combine(destRoot, file), file,
            async p => await File.WriteAllTextAsync(p, content));

        private static async Task WriteCreatePatch(string destRoot, string patchesRoot, string file) =>
            await WritePatch(Path.Combine(destRoot, file), file,
                p => Task.Run(() =>
                {
                    string newFilePath = Path.Combine(patchesRoot, $"{file}{PatchExtension}{CreateExtension}");
                    Directory.CreateDirectory(Path.GetDirectoryName(newFilePath)!);
                    File.Copy(p, newFilePath);
                }));

        private static async Task WriteDeletePatch(string patchesRoot, string file) => await WritePatch(
            Path.Combine(patchesRoot, file), file,
            _ => Task.Run(() =>
                File.Create(Path.Combine(patchesRoot, $"{file}{PatchExtension}{DeleteExtension}")).Close()));

        private static async Task WritePatch(string file, string displayPath, Func<string, Task> action)
        {
            Console.WriteLine("Creating patch {0}... ", displayPath);

            try
            {
                DirectoryInfo patch = new(Path.GetDirectoryName(file)!);
                patch.Create();

                await action(file);
            }
            catch (Exception e)
            {
                ConsoleHelper.WriteLineError("Failed creating patch for {0}:\n{1}.", displayPath, e);
            }
        }
    }
}