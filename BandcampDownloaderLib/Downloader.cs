using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BandcampDownloaderLib
{
	public class Downloader
	{
		public const string StreamBaseUrl = "https://t4.bcbits.com/stream/";
		
		private readonly object _downloadCompletionLock = new();

		private readonly IResourceService _resourceService;
		private readonly ITrackTagger _trackTagger;

		public ProcessingStatus? ProcessingStatus { get; private set; }

		public Downloader(
			IResourceService? resourceService,
			ITrackTagger? trackTagger)
		{
			_resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
			_trackTagger = trackTagger ?? throw new ArgumentNullException(nameof(trackTagger));
			SetState(0, 0);
		}

		public async Task DownloadTracksAsync(Uri? albumPageUri)
		{
			if (albumPageUri == null)
				throw new ArgumentNullException(nameof(albumPageUri));
			
			SetState(0, 0, "Parsing page...");

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

		    SetState(0, tracks.Length, "Downloading...");
		    
		    var downloadsCompleted = 0;
		    await Task.WhenAll(tracks.Select(async track =>
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

			    lock (_downloadCompletionLock)
			    {
				    downloadsCompleted++;
				    SetState(
					    countCompleted: downloadsCompleted, 
					    countTotal: tracks.Length, 
					    message: $"Downloaded {downloadsCompleted}/{tracks.Length} {track.TrackName}");
			    }
		    }));
		    
		    SetState(0, 0, "Done!");
		}
		
		private void SetState(int countCompleted, int countTotal, string? message = null)
		{
			ProcessingStatus = new ProcessingStatus(countCompleted, countTotal, message);
		}
	}	
}