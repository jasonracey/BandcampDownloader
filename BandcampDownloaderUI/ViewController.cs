using System;
using AppKit;
using BandcampDownloaderLib;
using Foundation;

namespace BandcampDownloaderUI
{
    public partial class ViewController : NSViewController
    {
        private static readonly Downloader Downloader = new Downloader(
            new ResourceService(),
            new TrackTagger());
        
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

            try
            {
                await Downloader.DownloadTracksAsync(new Uri(URL.StringValue));
                SetIdleState();
            }
            catch (Exception e)
            {
                SetErrorState($"Error: {e.Message}");
            }
            finally
            {
                StopTimer();
            }
        }

        private static double GetPercentCompleted()
        {
            var completed = Downloader?.ProcessingStatus?.CountCompleted ?? 0.0D;
            var total = Downloader?.ProcessingStatus?.CountTotal ?? 0.0D ;
            return total == 0.0D 
                ? 0.0D 
                : 100 * completed / total;
        }

        private static string GetStatusMessage()
        {
            return Downloader?.ProcessingStatus?.Message ?? string.Empty;
        }

        private void SetBusyState()
        {
            URL.StringValue = string.Empty;
            URL.Editable = false;
            URL.Enabled = false;
            Progress.DoubleValue = 0.0D;
            Progress.Hidden = false;
            Status.StringValue = string.Empty;
            Status.TextColor = NSColor.White;
            Clear.Enabled = false;
            Download.Enabled = false;
        }

        private void SetErrorState(string message)
        {
            URL.StringValue = string.Empty;
            URL.Editable = true;
            URL.Enabled = true;
            Progress.DoubleValue = 0.0D;
            Progress.Hidden = true;
            Status.StringValue = message;
            Status.TextColor = NSColor.Red;
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
            Status.StringValue = string.Empty;
            Status.TextColor = NSColor.White;
            Clear.Enabled = false;
            Download.Enabled = false;
        }
        
        private void StartTimer()
        {
            StopTimer();

            const double seconds = 0.25D;
            
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
