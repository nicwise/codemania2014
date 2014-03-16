using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using CodeMania.Core.Model;
using BigTed.Core;
using CodeMania.Core;
using TinyMessenger;
using System.Diagnostics;
using BigTed;
using System.Threading.Tasks;

namespace CodeMania.IOS
{
	[Register("CurrencyListCollectionViewController")]
	public partial class CurrencyListCollectionViewController : UICollectionViewController
	{
		ICurrencySource source;
		const string AMOUNTEDITSEGUE = "AmountEditSegue";

		public CurrencyListCollectionViewController(IntPtr handle) : base(handle)
		{
			source = Container.Resolve<ICurrencySource>();
		}

		partial void RefreshCurrency(NSObject sender)
		{
			Task.Factory.StartNew(() => {
				var source = Container.Resolve<ICurrencySource>();
				source.RefreshFromSource();
			});
		}

		partial void returned(UIStoryboardSegue segue)
		{
			AmountEditViewController sourceViewController = segue.SourceViewController as AmountEditViewController;

			if (sourceViewController != null)
			{
				CurrentBaseCurrencyAmount = sourceViewController.Amount;
			}
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == AMOUNTEDITSEGUE)
			{
				//could load stuff into the target segue
			}
		}

		TinyMessageSubscriptionToken reloadToken, refreshToken, errorToken;

		void SetupMessages()
		{
			reloadToken = Container.Subscribe<CurrencyHasReloadedMessage>(msg =>
			{

				CurrentCurrencyList = msg.NewCurrency;
				InvokeOnMainThread(() =>
				{
					CollectionView.ReloadData();
					if (CurrentCurrencyList.Currencys.Count > 0)
						CollectionView.ScrollToItem(NSIndexPath.FromItemSection(0, 0), UICollectionViewScrollPosition.Top, true);
				});
			});

			refreshToken = Container.Subscribe<CurrencyRefreshMessage>(msg =>
			{
				source.GetCurrencyForBase(CurrentBaseCurrency);
			});

			errorToken = Container.Subscribe<RefreshErrorMessage>(msg =>
			{
				InvokeOnMainThread(() =>
				{
					BTProgressHUD.ShowToast(msg.Message, false, 3000);
				});
			});
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			SetupMessages();
			Task.Factory.StartNew(() => {
				var source = Container.Resolve<ICurrencySource>();
				source.RefreshFromSource();
			});

		}

		Currency CurrentCurrencyList = null;
		string CurrentBaseCurrency = "USD";
		float CurrentBaseCurrencyAmount = 100f;

		public override int NumberOfSections(UICollectionView collectionView)
		{
			if (CurrentCurrencyList == null)
			{
				return 0;
			}
			else
			{
				return 1;
			}

		}

		public override int GetItemsCount(UICollectionView collectionView, int section)
		{
			if (CurrentCurrencyList == null)
			{
				return 0;
			}

			return CurrentCurrencyList.Currencys.Count;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{


			var cell = collectionView.DequeueReusableCell(QuickCurrencyCell.Key, indexPath) as QuickCurrencyCell;

			var rate = CurrentCurrencyList.Currencys[indexPath.Row];

			cell.Setup(rate, CurrentBaseCurrencyAmount);
			return cell;

		}

		public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
		{
			if (elementKind == UICollectionElementKindSectionKey.Header)
			{
				var header = collectionView.DequeueReusableSupplementaryView(UICollectionElementKindSection.Header, QuickCurrencyHeaderCell.Key, indexPath) as QuickCurrencyHeaderCell;
				header.Setup(CurrentBaseCurrency, CurrentBaseCurrencyAmount);

				UITapGestureRecognizer guesture = new UITapGestureRecognizer(() =>
				{
					HeaderTapped();
				})
				{
					NumberOfTapsRequired = 1
				};

				header.AddGestureRecognizer(guesture);

				return header;
			}

			return null;
		}

		public void HeaderTapped()
		{
			PerformSegue(AMOUNTEDITSEGUE, this);
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{

			var rate = CurrentCurrencyList.Currencys[indexPath.Row];

			CurrentBaseCurrency = rate.Id;
			CurrentBaseCurrencyAmount = 100f;

			Task.Factory.StartNew (() =>
			{
				source.GetCurrencyForBase (rate.Id);
			});
		}
	}
}

