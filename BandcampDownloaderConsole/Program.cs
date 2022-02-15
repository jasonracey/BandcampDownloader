using BandcampDownloaderLib;

var resourceService = new ResourceService();
var trackTagger = new TrackTagger();

Console.WriteLine();
Console.WriteLine("Bandcamp Downloader");
Console.WriteLine("by https://github.com/jasonracey");
Console.WriteLine();

var input = args[0];

if (string.IsNullOrWhiteSpace(input) || !Uri.TryCreate(input, UriKind.Absolute, out var uri))
{
    Console.WriteLine("Please specify a valid url.");
}
else
{
    var bandcampDownloader = new BandcampDownloader(
        resourceService,
        trackTagger);
    
    await bandcampDownloader.DownloadTracksAsync(uri);
    
    Console.WriteLine();
    Console.WriteLine("Thanks for using Bandcamp Downloader.");
    Console.WriteLine();
}