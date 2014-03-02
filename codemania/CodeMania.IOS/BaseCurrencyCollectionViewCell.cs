using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CodeMania.IOS
{
	public partial class BaseCurrencyCollectionViewCell : UICollectionViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("BaseCurrencyCollectionViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("BaseCurrencyCollectionViewCell");

		public BaseCurrencyCollectionViewCell (IntPtr handle) : base (handle)
		{
		}

		public static BaseCurrencyCollectionViewCell Create ()
		{
			return (BaseCurrencyCollectionViewCell)Nib.Instantiate (null, null) [0];
		}

		public void Setup(string baseCurrency, float amount) 
		{
			CurrencyLabel.Text = string.Format ("{0} - {1:0.00}", baseCurrency, amount);
		}

	}
}

