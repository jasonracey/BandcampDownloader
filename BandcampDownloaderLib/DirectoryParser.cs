namespace BandcampDownloaderLib;

public static class DirectoryParser
{
    private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();

    public static string GetDestinationDirectory(string? artist, string? album)
    {
        if (string.IsNullOrWhiteSpace(artist))
            throw new ArgumentNullException(nameof(artist));
        if (string.IsNullOrWhiteSpace(album))
            throw new ArgumentNullException(nameof(album));
        
        return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Downloads/{RemoveInvalidPathChars($"{artist} - {album}")}";
    }

    public static string GetDestinationFilePath(string? destinationDirectory, int trackNumber, string? trackName)
    {
        if (string.IsNullOrWhiteSpace(destinationDirectory))
            throw new ArgumentNullException(nameof(destinationDirectory));
        if (trackNumber <= 0)
            throw new ArgumentException("Must be greater than 0.", nameof(trackNumber));
        if (string.IsNullOrWhiteSpace(trackName))
            throw new ArgumentNullException(nameof(trackName));
        
        return $"{RemoveInvalidPathChars(destinationDirectory)}/{trackNumber.ToString("D2")} {RemoveInvalidPathChars(trackName)}.mp3";
    }

    public static string RemoveInvalidPathChars(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(nameof(path));
        
        return string.Join(string.Empty, path.Split(InvalidPathChars));    
    }
}