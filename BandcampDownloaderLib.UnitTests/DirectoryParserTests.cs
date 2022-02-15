using System;
using System.IO;
using NUnit.Framework;

namespace BandcampDownloaderLib.UnitTests;

[TestFixture]
public class DirectoryParserTests
{
    private const string MockAlbum = "MockAlbum";
    private const string MockArtist = "MockArtist";
    private const string MockDestinationDirectory = "/Home/Downloads/Foo - Bar/";
    private const int MockTrackNumber = 3;
    private const string MockTrackName = "MockTrackName";
    
    private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();
    private static readonly string InvalidPathCharsString = new(InvalidPathChars);
    private static readonly string HomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    [Test]
    public void GetDestinationDirectory_ValidatesArgs()
    {
        Assert.Throws<ArgumentNullException>(() => DirectoryParser.GetDestinationDirectory(null, MockAlbum));
        Assert.Throws<ArgumentNullException>(() => DirectoryParser.GetDestinationDirectory(MockArtist, null));
    }

    [Test]
    public void GetDestinationDirectory_CanGetDestinationDirectory_RemovesInvalidChars()
    {
        // arrange
        var mockArtistWithInvalidChars = $"{InvalidPathCharsString}{MockArtist}{InvalidPathCharsString}";
        var mockAlbumWithInvalidChars = $"{InvalidPathCharsString}{MockAlbum}{InvalidPathCharsString}";
        
        // act
        var result = DirectoryParser.GetDestinationDirectory(mockArtistWithInvalidChars, mockAlbumWithInvalidChars);
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual($"{HomePath}/Downloads/{MockArtist} - {MockAlbum}", result);
    }
    
    [Test]
    public void GetDestinationFilePath_ValidatesArgs()
    {
        Assert.Throws<ArgumentNullException>(() => DirectoryParser.GetDestinationFilePath(null, MockTrackNumber, MockTrackName));
        Assert.Throws<ArgumentException>(() => DirectoryParser.GetDestinationFilePath(MockDestinationDirectory, 0, MockTrackName));
        Assert.Throws<ArgumentNullException>(() => DirectoryParser.GetDestinationFilePath(MockDestinationDirectory, MockTrackNumber, null));
    }

    [Test]
    public void GetDestinationFilePath_CanGetDestinationFilePath_RemovesInvalidChars()
    {
        // arrange
        var mockDestinationDirectoryWithInvalidChars = $"{InvalidPathCharsString}{MockDestinationDirectory}{InvalidPathCharsString}";
        var mockTrackNameWithInvalidChars = $"{InvalidPathCharsString}{MockTrackName}{InvalidPathCharsString}";
        
        // act
        var result = DirectoryParser.GetDestinationFilePath(mockDestinationDirectoryWithInvalidChars, MockTrackNumber, mockTrackNameWithInvalidChars);
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual($"{MockDestinationDirectory}/{MockTrackNumber:D2} {MockTrackName}.mp3", result);
    }
    
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void RemoveInvalidPathChars_ValidatesArgs(string path)
    {
        Assert.Throws<ArgumentNullException>(() => DirectoryParser.RemoveInvalidPathChars(path));
    }

    [Test]
    public void RemoveInvalidPathChars_CanRemoveInvalidPathChars()
    {
        // arrange
        const string mockPath = "/MockPath";
        var pathWithInvalidChars = $"{InvalidPathCharsString}{mockPath}{InvalidPathCharsString}";
        
        // act
        var result = DirectoryParser.RemoveInvalidPathChars(pathWithInvalidChars);
        
        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(mockPath, result);
    }
}