# Lunacy - Lunar Client Launcher Mod
Lunacy is a simple mod for Lunar's Launcher, with the goal of making it more convenient and adding more features.
There isn't much to necessarily build on, but there's certainly ways to improve it nonetheless.

*"modification for lunar client launcher, who knows what it adds"*

### Project Overview
| Project | Description |
|---------|-------------|
| Lunacy.ASAR | ASAR archive unpacker |
| Lunacy | CLI task utility tool |
| Lunacy.Patch | Diffing and patching tool |


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

TODO: 
* Use JSNice to clean up unpacked code (also maybe make prettier formatting more consistent)
* add features
* plugins (why?)
* settings page
* remove advertisements (configurable)
* setting to disable cosmetics (backend implemented, just need a way to change it)
* win, mac (both vers.), & linux distros + testing (four variations, unfortunately)
* specify custom JRE
* st stuff

## But what about Lunar's Terms of Service? Does it infringe on terms you've agreed to?
Not at all. Lunar's ToS are seemingly vague on purpose, and the most it tells you to do is to not disable security features, abide by the laws of your country of residency, and not to use it for commerical or promotional use. Lunacy is a purely constructive piece of software that does not infringe on any of this. I am US-based and I believe it is safe to say that the US does not have any laws forbidding me from unpacking `.asar` files. There is no reverse-engineering that needs to be done and minimal code is revealed, much of which is useless and helps absolutely no one unless they're directly contributing to this project.
