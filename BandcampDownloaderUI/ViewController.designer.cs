// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BandcampDownloaderUI
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton Clear { get; set; }

		[Outlet]
		AppKit.NSButton Download { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator Progress { get; set; }

		[Outlet]
		AppKit.NSTextField Status { get; set; }

		[Outlet]
		AppKit.NSTextField URL { get; set; }

		[Action ("ClearClicked:")]
		partial void ClearClicked (AppKit.NSButton sender);

		[Action ("DownloadClicked:")]
		partial void DownloadClicked (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Clear != null) {
				Clear.Dispose ();
				Clear = null;
			}

			if (Download != null) {
				Download.Dispose ();
				Download = null;
			}

			if (Progress != null) {
				Progress.Dispose ();
				Progress = null;
			}

			if (Status != null) {
				Status.Dispose ();
				Status = null;
			}

			if (URL != null) {
				URL.Dispose ();
				URL = null;
			}
		}
	}
}
