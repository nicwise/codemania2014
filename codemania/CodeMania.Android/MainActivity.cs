using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CodeMania.Core;
using BigTed.Core;
using Android.Text.Style;
using TinyMessenger;
using CodeMania.Core.Model;
using Android;
using Java.Lang;
using System.Threading.Tasks;

namespace CodeMania.Android
{
	[Activity (Label = "Quick Currency", MainLauncher = true)]
	public class MainActivity : Activity
	{
		ICurrencySource source;
		string CurrentBaseCurrency = "USD";
		float CurrentCurrencyValue = 100f;
		Currency CurrentCurrencyList;

		//using ControlFetcherMixin - the property name has to match the R.id.xxxx of the item
		GridView currencyGridView
		{
			get { return this.GetControl<GridView> (); }
		}

		TextView currencyName
		{
			get { return this.GetControl<TextView> (); }
		}

		TextView currencyValue
		{
			get { return this.GetControl<TextView> (); }
		}

		RelativeLayout headerLayout
		{
			get { return this.GetControl<RelativeLayout> (); }
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);


			PlatformSetup.Setup ();
			App.Setup ();

			PlatformSetup.SetupDatabase ();

			SetupUI ();
			SetupMessages ();

			Task.Factory.StartNew (async () =>
			{
				source = Container.Resolve<ICurrencySource> ();
				await source.RefreshFromSource ();
			});



		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			Container.Unsubscribe<CurrencyHasReloadedMessage> (reloadToken);
			Container.Unsubscribe<CurrencyRefreshMessage> (refreshToken);
			Container.Unsubscribe<RefreshErrorMessage> (errorToken);
		}

		void SetupUI ()
		{

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//grab the header stuff
			//Normally you'd be using 
			//currencyName = (TextView)FindViewById (Resource.Id.currencyName);
			//for all the resources, but thanks to ControlFetcherMixin, you dont need to

			//and set it's values
			SetHeader ();

			//configure the grid
			currencyGridView.Adapter = new CurrencyListAdapter (this, CurrentCurrencyList, CurrentCurrencyValue);
			currencyGridView.ItemClick += (sender, e) =>
			{
				var adapter = (currencyGridView.Adapter as CurrencyListAdapter);

				var newCurrency = adapter.Currencies.Currencys [e.Position].Id;
				source.GetCurrencyForBase (newCurrency);
				currencyGridView.SmoothScrollToPosition (0);
			};

			headerLayout.Click += (object sender, EventArgs e) =>
			{
				SelectNewCurrencyAmount ();
			};



		}

		void SetHeader ()
		{
			currencyName.Text = CurrentBaseCurrency;
			currencyValue.Text = CurrentCurrencyValue.FormatCurrency (CurrentBaseCurrency);
			headerLayout.SetBackgroundResource (FlagIdFromCurrencyName (CurrentBaseCurrency));

		}
		//Need to map from "USD" to Resource.Drawable.USD
		int FlagIdFromCurrencyName (string currency)
		{

			Type drawableType = typeof(Resource.Drawable);

			var field = drawableType.GetField (currency);

			if (field != null)
			{
				return (int)field.GetValue (null);
			}

			return -1;
		}

		void SelectNewCurrencyAmount ()
		{
			var inflator = (LayoutInflater)this.GetSystemService (Context.LayoutInflaterService);


			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			builder.SetTitle ("New Currency Amount");
			builder.SetView (inflator.Inflate (Resource.Layout.NewCurrencyDialog, null));
			builder.SetPositiveButton ("OK", (object sender, DialogClickEventArgs e) =>
			{
				var d = (AlertDialog)sender;

				var textbox = (EditText)d.FindViewById (Resource.Id.newCurrencyAmount);

				string val = textbox.Text;

				float floatVal = 0;
				if (float.TryParse (val, out floatVal))
				{
					var adapter = (currencyGridView.Adapter as CurrencyListAdapter);
					CurrentCurrencyValue = floatVal;
					adapter.BaseCurrencyAmount = floatVal;
					SetHeader ();

				}
			});
			builder.SetNegativeButton ("Cancel", (object sender, DialogClickEventArgs e) =>
			{

			});


			AlertDialog dialog = builder.Create ();

			dialog.Show ();

		}

		TinyMessageSubscriptionToken reloadToken, refreshToken, errorToken;

		void SetupMessages ()
		{
			//A reload is when the currency info for display is changed.
			// Doesn't usually mean the database has updated, but it might.
			// Usually a result of the base currency changing
			reloadToken = Container.Subscribe<CurrencyHasReloadedMessage> (msg =>
			{
				CurrentCurrencyList = msg.NewCurrency;
				CurrentBaseCurrency = msg.NewCurrency.BaseCurrency;

				RunOnUiThread (() =>
				{
					(currencyGridView.Adapter as CurrencyListAdapter).Currencies = msg.NewCurrency;
					SetHeader ();
				});
			});

			//A refresh is when the database is updated from the source service
			refreshToken = Container.Subscribe<CurrencyRefreshMessage> (async msg =>
			{
				RunOnUiThread (() =>
				{
					Toast.MakeText (this, "Currencies updated", ToastLength.Short).Show ();
				});

				await source.GetCurrencyForBase (CurrentBaseCurrency);
			});

			//something went wrong. Cop out and just tell the user
			errorToken = Container.Subscribe<RefreshErrorMessage> (msg =>
			{
				RunOnUiThread (() =>
				{
					Toast.MakeText (this, msg.Message, ToastLength.Short).Show ();
				});
			});
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.main_activity_actions, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_refresh:
					Task.Factory.StartNew (async () =>
					{
						await source.RefreshFromSource ();
						await source.GetCurrencyForBase (CurrentBaseCurrency);
					});

					return true;
				default:
					return base.OnOptionsItemSelected (item);
			}
		}
	}
}


