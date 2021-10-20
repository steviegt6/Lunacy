# assuming git is alr installed if you're using bash...
# todo: safety checks for git & .net like in the batch script

echo Restoring git submodules

git submodule update --init --recursive

echo installing jsnice and prettier
npm i -g jsnice
npm i -g prettier

echo ATTENTION: .NET 5.0 is required to build and run these tools, if you do not have .NET 5.0 installed, this will not work

# / should be safe for paths, maybe
dotnet build Lunacy.Patch/Lunacy.Patch.csproj --configuration Debug

dotnet Lunacy.Patch/bin/Debug/net5.0/Lunacy.Patch.dll