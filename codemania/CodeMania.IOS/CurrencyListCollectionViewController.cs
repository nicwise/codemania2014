using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using CodeMania.Core.Model;
using BigTed.Core;
using CodeMania.Core;
using TinyMessenger;
using System.Diagnostics;

namespace CodeMania.IOS
{
	[Register ("CurrencyListCollectionViewController")]
	public class CurrencyListCollectionViewController : UICollectionViewController
	{
		public CurrencyListCollectionViewController (IntPtr handle) : base (handle)
		{
		}



		TinyMessageSubscriptionToken reloadToken, refreshToken;



		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			reloadToken = Container.Subscribe<CurrencyHasReloadedMessage> ((msg) =>
			{
				CurrentCurrencyList = msg.NewCurrency;
				InvokeOnMainThread (() =>
				{
					CollectionView.ReloadData ();
					CollectionView.ScrollToItem(NSIndexPath.FromItemSection(0,0), UICollectionViewScrollPosition.Top, true);
				});
			});


			refreshToken = Container.Subscribe<CurrencyRefreshMessage> ((msg) =>
			{
				var source = Container.Resolve<CurrencySource> ();
				source.GetCurrencyForBase (CurrentBaseCurrency);
			});

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			var source = Container.Resolve<CurrencySource> ();
			source.GetCurrencyForBase (CurrentBaseCurrency);
		}

		Currency CurrentCurrencyList = null;
		string CurrentBaseCurrency = "USD";
		float CurrentBaseCurrencyAmount = 100f;

		public override int NumberOfSections (UICollectionView collectionView)
		{
			if (CurrentCurrencyList == null)
			{
				return 0;
			} else
			{
				return 1;
			}

		}

		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			if (CurrentCurrencyList == null)
			{
				return 0;
			}

			return CurrentCurrencyList.Currencys.Count;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{


			var cell = collectionView.DequeueReusableCell (QuickCurrencyCell.Key, indexPath) as QuickCurrencyCell;

			var rate = CurrentCurrencyList.Currencys [indexPath.Row];

			cell.Setup (rate, CurrentBaseCurrencyAmount);
			return cell;

		}

		public override UICollectionReusableView GetViewForSupplementaryElement (UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
		{
			if (elementKind == UICollectionElementKindSectionKey.Header)
			{
				var header = collectionView.DequeueReusableSupplementaryView (UICollectionElementKindSection.Header, QuickCurrencyHeaderCell.Key, indexPath) as QuickCurrencyHeaderCell;
				header.Setup (CurrentBaseCurrency, CurrentBaseCurrencyAmount);
				return header;
			}

			return null;
		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{

			var rate = CurrentCurrencyList.Currencys [indexPath.Row];

			CurrentBaseCurrency = rate.Id;
			CurrentBaseCurrencyAmount = 100f;

			var source = Container.Resolve<CurrencySource> ();
			source.GetCurrencyForBase (rate.Id);

		}
	}
}

