@echo off

where git >NUL

if NOT ["%errorlevel%"] == ["0"] (
	echo git not found on PATH, make sure git is installed and you cloned the repository
    pause
    exit /b %errorlevel%
)

echo Restoring git submodules

git submodule update --init --recursive

if NOT ["%errorlevel%"] == ["0"] (
	echo issue encountered while restoring git submodules
    pause
    exit /b %errorlevel%
)

where dotnet >NUL

if NOT ["%errorlevel%"]==["0"] (
	echo dotnet not found on PATH, ensure that .NET 5 is installed
    pause
    exit /b %errorlevel%
)

echo building Lunacy.Path
dotnet build Lunacy.Patch/Lunacy.Patch.csproj --configuration Debug

if NOT ["%errorlevel%"]==["0"] (
	echo issue encountered while building Lunacy.Patch
    pause
    exit /b %errorlevel%
)

dotnet Lunacy.Patch/bin/Debug/net5.0/Lunacy.Patch.dll