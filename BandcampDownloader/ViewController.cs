using System;
using System.Globalization;
using AppKit;
using BandcampDownloaderLib;
using Foundation;

namespace BandcampDownloader
{
    public partial class ViewController : NSViewController
    {
        private static readonly ResourceService ResourceService = new ResourceService();
        private static readonly TrackTagger TrackTagger = new TrackTagger();
        private static readonly BandcampDownloaderLib.BandcampDownloader BandcampDownloader = new BandcampDownloaderLib.BandcampDownloader(
            ResourceService,
            TrackTagger);
        
        private NSTimer _timer;
        
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            Progress.Indeterminate = false;
            Status.StringValue = string.Empty;
            URL.Changed += (sender, e) => UrlChanged();
            
            SetIdleState();
        }

        partial void ClearClicked(NSButton sender)
        {
            URL.StringValue = string.Empty;
            SetIdleState();
        }

        async partial void DownloadClicked(NSButton sender)
        {
            SetBusyState();
            StartTimer();

            await BandcampDownloader.DownloadTracksAsync(new Uri(URL.StringValue));
            
            StopTimer();
            SetIdleState();
        }

        private static double GetPercentCompleted()
        {
            var completed = BandcampDownloader?.ProcessingStatus?.CountCompleted ?? 0.0D;
            var total = BandcampDownloader?.ProcessingStatus?.CountTotal ?? 0.0D ;
            return total == 0.0D 
                ? 0.0D 
                : 100 * completed / total;
        }

        private static string GetStatusMessage()
        {
            return BandcampDownloader?.ProcessingStatus?.Message ?? string.Empty;
        }

        private void SetBusyState()
        {
            URL.Editable = false;
            URL.Enabled = false;
            Progress.Hidden = false;
            Clear.Enabled = false;
            Download.Enabled = false;
        }

        private void SetIdleState()
        {
            URL.StringValue = string.Empty;
            URL.Editable = true;
            URL.Enabled = true;
            Progress.DoubleValue = 0.0D;
            Progress.Hidden = true;
            Clear.Enabled = false;
            Download.Enabled = false;
        }
        
        private void StartTimer()
        {
            StopTimer();

            // Need to use a short interval to get Status to update to final message
            // before the call to DownloadTracksAsync returns and stops the timer.
            const double seconds = 0.01D;
            
            _timer = NSTimer.CreateRepeatingScheduledTimer(seconds, _ => {
                Progress.DoubleValue = GetPercentCompleted();
                Status.StringValue = GetStatusMessage();
            });
        }

        private void StopTimer()
        {
            if (_timer == null) return;
            _timer.Invalidate();
            _timer.Dispose();
            _timer = null;
        }

        private void UrlChanged()
        {
            Clear.Enabled = UrlHasText();
            Download.Enabled = UrlIsValid();
        }

        private bool UrlHasText()
        {
            return !string.IsNullOrWhiteSpace(URL.StringValue);
        }

        private bool UrlIsValid()
        {
            return Uri.TryCreate(URL.StringValue, UriKind.Absolute, out _);
        }
    }
}
