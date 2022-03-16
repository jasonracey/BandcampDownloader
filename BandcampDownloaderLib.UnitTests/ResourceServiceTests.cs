using System;
using NUnit.Framework;

namespace BandcampDownloaderLib.UnitTests
{
    [TestFixture]
    public class ResourceServiceTests
    {
        private const string MockDestinationPath = "MockDestinationPath";
        private const string MockUrl = "https://www.url.com";
    
        private readonly IResourceService _resourceService;

        public ResourceServiceTests()
        {
            _resourceService = new ResourceService();
        }

        [Test]
        public void GetResourceStringAsync_ValidatesArgs()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _resourceService.GetResourceStringAsync(null));
        }
    
        [Test]
        public void DownloadResourceAsync_ValidatesArgs()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _resourceService.DownloadResourceAsync(null, MockDestinationPath));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _resourceService.DownloadResourceAsync(new Uri(MockUrl), null));
        }
    }
}