#!/bin/bash

# /usr/local/share/dotnet/sdk/6.0.201/Microsoft.Common.CurrentVersion.targets(1220,5): error MSB3644: The reference 
# assemblies for Xamarin.Mac,Version=v2.0 were not found. To resolve this, install the Developer Pack (SDK/Targeting 
# Pack) for this framework version or retarget your application. You can download .NET Framework Developer Packs at 
# https://aka.ms/msbuild/developerpacks 
# [/Users/jasonracey/Files/Software/Projects/BandcampDownloader/BandcampDownloaderUI/BandcampDownloaderUI.csproj]
#dotnet build -c Release BandcampDownloader.sln

# deploy console
echo "Deploying Console..."
rm -Rf ../../Tools/BandcampDownloader
mkdir ../../Tools/BandcampDownloader
cp -R ./BandcampDownloaderConsole/bin/Release/netstandard2.1/ ../../Tools/BandcampDownloader

# deploy app
echo "Deploying UI..."
rm -Rf /Applications/Bandcamp\ Downloader.app
cp -Rf ./BandcampDownloaderUI/bin/Release/Bandcamp\ Downloader.app /Applications/Bandcamp\ Downloader.app

echo "Done!"
