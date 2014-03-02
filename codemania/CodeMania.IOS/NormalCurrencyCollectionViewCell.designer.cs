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
	[Register ("NormalCurrencyCollectionViewCell")]
	partial class NormalCurrencyCollectionViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView CurrencyFlag { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel CurrencyLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CurrencyFlag != null) {
				CurrencyFlag.Dispose ();
				CurrencyFlag = null;
			}

			if (CurrencyLabel != null) {
				CurrencyLabel.Dispose ();
				CurrencyLabel = null;
			}
		}
	}
}
