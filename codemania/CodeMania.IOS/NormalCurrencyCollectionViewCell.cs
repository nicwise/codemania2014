using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using CodeMania.Core.Model;

namespace CodeMania.IOS
{
	public partial class NormalCurrencyCollectionViewCell : UICollectionViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("NormalCurrencyCollectionViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("NormalCurrencyCollectionViewCell");

		public NormalCurrencyCollectionViewCell (IntPtr handle) : base (handle)
		{
		}

		public static NormalCurrencyCollectionViewCell Create ()
		{
			return (NormalCurrencyCollectionViewCell)Nib.Instantiate (null, null) [0];
		}

		public void Setup (CurrencyRate currencyRate, float baseAmount)
		{
			CurrencyLabel.Text = string.Format ("{0}: {1:0.00} - {2:0.00}", currencyRate.Id, currencyRate.Rate,
				baseAmount * currencyRate.Rate);
		}
	}
}

