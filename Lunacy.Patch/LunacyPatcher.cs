#region License
// Pending license, modified from Assaturers/Assatur (https://github.com/Assaturers/Assatur)
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DiffPatch;
using ModdingToolkit.Diffing;
using ModdingToolkit.Magicka.Finding;
using ModdingToolkit.Patching;
using Webmilio.Commons.Extensions;

namespace Lunacy.Patch
{
    public class LunacyPatcher : IPatcher
    {
        public async Task Patch(DirectoryInfo patches, DirectoryInfo destination)
        {
            // ensure directories exist
            patches.Create();
            destination.Create();

            Console.WriteLine($"Patching {destination} using {patches}.");

            FileInfo[] files = patches.GetFiles("*.patch*", SearchOption.AllDirectories);

            if (files.Length == 0)
                return;

            List<Task> tasks = new(files.Length);
            files.Do(f => tasks.Add(Patch(destination, patches, f)));

            await Task.WhenAll(tasks);
        }

        public static async Task Patch(DirectoryInfo destination, DirectoryInfo patches, FileInfo patch)
        {
            string shortName = patch.FullName[(patches.FullName.Length + 1)..];

            DirectoryInfo destFolder = destination.Combine(Path.GetDirectoryName(shortName));
            destFolder.Create();

            string extension = Path.GetExtension(shortName);
            FileInfo destFile = new(destFolder.CombineString(Path.GetFileNameWithoutExtension(shortName)));

            Console.WriteLine($"Applying {shortName}");

            if (extension.Equals(StandardDiffer.PatchExtension))
            {
                // This is probably inefficient and could be replaced with Streams (Readers/Writers) but the current
                // implementation of DiffPatch kinda sucks :/
                FilePatcher patchFile = FilePatcher.FromPatchFile(patch.ToString());
                string[] lines =
                    new Patcher(patchFile.PatchFile.Patches, await File.ReadAllLinesAsync(destFile.ToString()))
                        .Patch(default)
                        .ResultLines;

                await File.WriteAllLinesAsync(destFile.FullName, lines);
            }
            else
            {
                destFile = new FileInfo(Path.Combine(
                    destFile.DirectoryName!,
                    Path.GetFileNameWithoutExtension(destFile.Name)));

                switch (extension)
                {
                    case StandardDiffer.CreateExtension:
                        patch.CopyTo(destFile.ToString(), true);
                        break;

                    case StandardDiffer.DeleteExtension:
                    {
                        if (destFile.Exists)
                            destFile.Delete();

                        break;
                    }
                }
            }
        }

        public static async Task StandardPatch(IPatcher patcher, ILocationStore loc) =>
            await patcher.Patch(loc.Patches, loc.DecompAssatur);
    }
}