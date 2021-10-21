using System;
using System.IO;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Lunacy.ASAR;
using Spectre.Console;

namespace Lunacy.Commands
{
    [Command("asar-unpack", Description = "Unpacks an ASAR archive.")]
    public class AsarUnpackerCommand : ICommand
    {
        [CommandOption("path", 'p', Description = "ASAR archive file path.", IsRequired = true)]
        public string AsarPath { get; init; } = null!;

        [CommandOption("dest", 'd', Description = "ASAR output directory path.", IsRequired = true)]
        public string OutputDestination { get; protected set; } = null!;

        protected ProgressTask? Progress;

        public async ValueTask ExecuteAsync(IConsole console)
        {
            AnsiConsole.WriteLine($"Extracting ASAR path at {AsarPath} to {OutputDestination}...");

            AsarExtractor extractor = new();
            extractor.FileExtracted += OnFileExtracted;

            if (!OutputDestination.EndsWith(Path.DirectorySeparatorChar))
                OutputDestination += Path.DirectorySeparatorChar;

            await AnsiConsole.Progress()
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new SpinnerColumn()
                ).StartAsync(async x =>
                {
                    Progress = x.AddTask("Extracting ASAR package...");

                    await extractor.ExtractAll(new AsarArchive(AsarPath), OutputDestination, true);
                });
        }

        private void OnFileExtracted(object? sender, AsarExtractEvent e)
        {
            if (Progress is null)
                return;

            // Progress.Description = $"Extracting: {e.File.Path}";
            Progress.MaxValue = e.Total;
            Progress.Value = e.Index;
        }
    }
}