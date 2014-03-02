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
	public class CurrencyListCollectionViewController : UICollectionViewController
	{
		public CurrencyListCollectionViewController () : this (new UICollectionViewFlowLayout () {
			ItemSize = new SizeF (320, 50)
		})
		{
			Title = "Quick Currency";

			NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Refresh, (e, o) =>
			{
				var source = Container.Resolve<CurrencySource> ();
				source.GetCurrencyForBase (CurrentBaseCurrency);
			});
		}

		TinyMessageSubscriptionToken reloadToken, refreshToken;

		public CurrencyListCollectionViewController (UICollectionViewLayout layout) : base (layout)
		{
			reloadToken = Container.Subscribe<CurrencyHasReloadedMessage> ((msg) =>
			{
				CurrentCurrencyList = msg.NewCurrency;
				InvokeOnMainThread (() =>
				{
					CollectionView.ReloadData ();
				});
			});


			refreshToken = Container.Subscribe<CurrencyRefreshMessage> ((msg) =>
			{
				var source = Container.Resolve<CurrencySource> ();
				source.GetCurrencyForBase (CurrentBaseCurrency);
			});
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			CollectionView.RegisterNibForCell (BaseCurrencyCollectionViewCell.Nib, BaseCurrencyCollectionViewCell.Key);
			CollectionView.RegisterNibForCell (NormalCurrencyCollectionViewCell.Nib, NormalCurrencyCollectionViewCell.Key);
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
				return 2;
			}

		}

		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			if (CurrentCurrencyList == null)
			{
				return 0;
			}

			switch (section)
			{
				case 0:
					return 1;
				case 1:
					return CurrentCurrencyList.Currencys.Count;
			}

			return 0;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0)
			{
				var cell = collectionView.DequeueReusableCell (BaseCurrencyCollectionViewCell.Key, indexPath) as BaseCurrencyCollectionViewCell;

				cell.Setup (CurrentCurrencyList.BaseCurrency, CurrentBaseCurrencyAmount);
				return cell;
			}

			var normalCell = collectionView.DequeueReusableCell (NormalCurrencyCollectionViewCell.Key, indexPath) as NormalCurrencyCollectionViewCell;
			normalCell.Setup (CurrentCurrencyList.Currencys [indexPath.Row], CurrentBaseCurrencyAmount);
			return normalCell;

		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0)
			{

			}

			if (indexPath.Section == 1)
			{
				var rate = CurrentCurrencyList.Currencys [indexPath.Row];

				CurrentBaseCurrency = rate.Id;
				CurrentBaseCurrencyAmount = 100f;

				var source = Container.Resolve<CurrencySource> ();
				source.GetCurrencyForBase (rate.Id);
			}
		}
	}
}

