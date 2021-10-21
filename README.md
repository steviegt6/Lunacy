# Lunacy - Lunar Client Launcher Mod
Lunacy is a simple mod for Lunar's Launcher, with the goal of making it more convenient and adding more features.
There isn't much to necessarily build on, but there's certainly ways to improve it nonetheless.

*"modification for lunar client launcher, who knows what it adds"*

### Project Overview
Lunacy.ASAR - ASAR archive unpacker
Lunacy - CLI utility tool
Lunacy.Patch - Diffing and patching tool

C# program for JS lol!!!

## Building
If you want to use this and you aren't looking to contribute, sit tight. Releases will crop up soon enough.

If you want to build the program yourself, it's quite simple; ensure you meet the prerequisites before attempting to build:
1. You have a production copy of Lunar Client installed, and it's the latest version.
2. You have npm installed and it's located on the path.
3. You have a .NET 5.0 SDK.
4. Your computer can run bash or batch files.

If your computer can do all of this, you're set.

You do not need to open any `.sln` (`solution`) files, a start script has been provided which you should run instead.

If you are on Windows, it is recommended you use the batch (`.bat`) script, instead of attempting to run it in git bash due to git bash's unfortunate quality of redirecting console output and input, causing `Console.ReadKey` to not work as intended.
\*Nix users are safe.

Once you have ran the script, wait patiently until everything is finished installing and building, then press `c` to start the developer set-up, this will prompt you for the installation path to your Lunar Client Launcer installation, which varies depending on your OS.

The program will proceed to copy files to a development workspace located in the `client` folder, and will create a copy that you **should not touch** located in a folder titled `original`, this is used for diffing against patches.

After copying is complete, press `p` to patch your files.

Once patching is complete, navigate to `client/resources/app/`, which is where all of the files you'll often be editing are located. Feel free to modify them with whatever text editor you please. Once you are finished, press `d` in the set-up program to diff your files. You can them commit them to git.

Please be sure to keep your diffs as small as possible in order to expose as little unpacked and prettified code as you can.

## Contributing
Contributing is simple, and PRs don't have a specific format.
Here's a list of big TO-DOs:

TODO: Use JSNice to clean up unpacked code
TODO: add features
TODO: plugins (why?)
TODO: settings page
TODO: remove advertisements (configurable)
TODO: setting to disable cosmetics