// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace CodeMania.IOS
{
	[Register ("QuickCurrencyCell")]
	partial class QuickCurrencyCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel CurrencyNameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel CurrencyValueLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel ExchangeRateValueLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CurrencyNameLabel != null) {
				CurrencyNameLabel.Dispose ();
				CurrencyNameLabel = null;
			}

			if (ExchangeRateValueLabel != null) {
				ExchangeRateValueLabel.Dispose ();
				ExchangeRateValueLabel = null;
			}

			if (CurrencyValueLabel != null) {
				CurrencyValueLabel.Dispose ();
				CurrencyValueLabel = null;
			}
		}
	}
}
