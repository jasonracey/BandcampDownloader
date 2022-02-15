namespace BandcampDownloaderLib;

public class BandcampDownloader
{
	public const string StreamBaseUrl = "https://t4.bcbits.com/stream/";

	private readonly IResourceService _resourceService;
	private readonly ITrackTagger _trackTagger;

	public BandcampDownloader(
		IResourceService? resourceService,
		ITrackTagger? trackTagger)
	{
		_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
		_trackTagger = trackTagger ?? throw new ArgumentNullException(nameof(trackTagger));
	}

	public async Task DownloadTracksAsync(Uri? albumPageUri)
	{
		if (albumPageUri == null)
			throw new ArgumentNullException(nameof(albumPageUri));

		var albumPage = await _resourceService
			.GetResourceStringAsync(albumPageUri)
			.ConfigureAwait(false);

	    var tracks = TrackParser
		    .GetTracks(albumPage, StreamBaseUrl)
		    .ToArray();
	    
	    var (album, artist) = AlbumAndArtistParser.GetAlbumAndArtist(albumPage);

	    var destinationDirectory = DirectoryParser.GetDestinationDirectory(artist, album);
	    if (!Directory.Exists(destinationDirectory))
		    Directory.CreateDirectory(destinationDirectory);
	    
	    Console.WriteLine($"Downloading {album}");
	    Console.WriteLine();

	    foreach (var track in tracks)
	    {
		    var filePath = DirectoryParser.GetDestinationFilePath(destinationDirectory, track.TrackNumber, track.TrackName);
		    if (File.Exists(filePath))
			    File.Delete(filePath);
		    
		    // download
		    await _resourceService
			    .DownloadResourceAsync(track.TrackUri, filePath)
			    .ConfigureAwait(false);

		    // tag file
		    _trackTagger.TagTrack(
			    filePath, 
			    album, 
			    artist, 
			    track.TrackName, 
			    track.TrackNumber,
                tracks.Length);

		    // print status
		    Console.WriteLine($"[✓] {track.TrackName}");
	    }
	    
	    Console.WriteLine();
	    Console.WriteLine($"Downloaded to {destinationDirectory}");
	}
}