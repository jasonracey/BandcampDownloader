using System;
using Moq;
using NUnit.Framework;

namespace BandcampDownloaderLib.UnitTests
{
    [TestFixture]
    public class DownloaderTests
    {
        private const string MockAlbumPage = "<html><body>mock page</body></html>";
        private static readonly Uri MockUri = new ("https://www.bandcamp.com");
        
        private readonly Downloader _target;
        private readonly Mock<IResourceService> _mockResourceService;
        private readonly Mock<ITrackTagger> _mockTrackTagger;

        public DownloaderTests()
        {
            _mockResourceService = new Mock<IResourceService>();
            _mockTrackTagger = new Mock<ITrackTagger>();
            _target = new Downloader(
                _mockResourceService.Object,
                _mockTrackTagger.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _mockResourceService
                .Setup(mock => mock.GetResourceStringAsync(It.IsAny<Uri>()))
                .ReturnsAsync(MockAlbumPage);
        }
        
        [Test]
        public void Constructor_ValidatesArgs()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new Downloader(null, _mockTrackTagger.Object);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new Downloader(_mockResourceService.Object, null);
            });
        }

        [Test]
        public void DownloadTracksAsync_ValidatesArgs()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _target.DownloadTracksAsync(null));
        }

        [Test]
        public void DownloadTracksAsync_NoTracksFoundOnAlbumPage_Throws()
        {
            // arrange
            _mockResourceService
                .Setup(mock => mock.GetResourceStringAsync(It.IsAny<Uri>()))
                .ReturnsAsync("<html>wrong page</html>");
            
            // act
            var thrown = Assert.ThrowsAsync<DownloaderException>(async () => await _target.DownloadTracksAsync(MockUri));
            
            // assert
            Assert.IsNotNull(thrown);
            Assert.AreEqual(ExceptionReason.NoTracksFoundOnPage, thrown?.Message);
        }
    }
}