using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using CodeMania.Core;
using BigTed.Core;

namespace CodeMania.IOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window
		{
			get;
			set;
		}
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//spin up the ioc

			PlatformSetup.Setup ();
			App.Setup ();

			PlatformSetup.SetupDatabase ();

			var source = Container.Resolve<CurrencySource> ();
			source.RefreshFromSource ();

			return true;
		}
	}
}

