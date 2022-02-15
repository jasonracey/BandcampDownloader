#!/bin/bash

dotnet build -c Release BandcampDownloader.sln
rm -rf ../../Tools/BandcampDownloader
mkdir ../../Tools/BandcampDownloader
cp -r ./BandcampDownloaderConsole/bin/Release/net6.0/ ../../Tools/BandcampDownloader

