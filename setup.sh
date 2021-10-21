#!/bin/bash

if ! type "git" > /dev/null; then
	echo "Please install git before running this script (https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)"
	exit 1
fi

echo "Restoring git submodules"
git submodule update --init --recursive

if ! type "npm" > /dev/null; then
	echo "Please install npm before running this script (https://docs.npmjs.com/downloading-and-installing-node-js-and-npm)"
	exit 1
fi

echo Globally installing jsnice and prettier with npm
sudo npm i -g jsnice
sudo npm i -g prettier

if ! type "dotnet" > /dev/null || ! dotnet --list-sdks | grep -q '5.0'; then
	echo "Please install .NET 5.0 before running this script (https://dotnet.microsoft.com/download/dotnet/5.0)"
	exit 1
fi

# should be safe for paths, maybe
dotnet build Lunacy.Patch/Lunacy.Patch.csproj --configuration Debug
dotnet Lunacy.Patch/bin/Debug/net5.0/Lunacy.Patch.dll
