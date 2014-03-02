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
	[Register ("BaseCurrencyCollectionViewCell")]
	partial class BaseCurrencyCollectionViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel CurrencyLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CurrencyLabel != null) {
				CurrencyLabel.Dispose ();
				CurrencyLabel = null;
			}
		}
	}
}
