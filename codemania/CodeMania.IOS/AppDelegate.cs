﻿using System;
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
		// class-level declarations
		UIWindow window;
		CurrencyListCollectionViewController rootController;
		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//spin up the ioc

			PlatformSetup.Setup ();
			App.Setup ();

			PlatformSetup.SetupDatabase ();

			var source = Container.Resolve<CurrencySource> ();
			source.RefreshFromSource ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			rootController = new CurrencyListCollectionViewController ();
			window.RootViewController = new UINavigationController(rootController);
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}
