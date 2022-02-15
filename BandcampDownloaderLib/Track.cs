namespace BandcampDownloaderLib;

public class Track
{
    public string TrackName { get; }
    public int TrackNumber { get; }
    public Uri TrackUri { get; }

    public Track(string trackName, int trackNumber, Uri trackUri)
    {
        if (string.IsNullOrWhiteSpace(trackName))
            throw new ArgumentNullException(nameof(trackName));
        if (trackNumber <= 0)
            throw new ArgumentException("Must be greater than 0", nameof(trackNumber));
        if (trackUri == null)
            throw new ArgumentNullException(nameof(trackUri));

        TrackName = trackName;
        TrackNumber = trackNumber;
        TrackUri = trackUri;
    }
}