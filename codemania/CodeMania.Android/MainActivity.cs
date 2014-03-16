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
	[Activity(Label = "Quick Currency", MainLauncher = true)]
	public class MainActivity : Activity
	{
		ICurrencySource source;
		string CurrentBaseCurrency = "USD";
		float CurrentCurrencyValue = 100f;
		Currency CurrentCurrencyList;
		ListView currencyListView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);


			PlatformSetup.Setup();
			App.Setup();

			PlatformSetup.SetupDatabase();

			SetupUI();
			SetupMessages();

			Task.Factory.StartNew (() =>
			{
				source = Container.Resolve<ICurrencySource> ();
				source.RefreshFromSource ();
			});



		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			Container.Unsubscribe<CurrencyHasReloadedMessage>(reloadToken);
			Container.Unsubscribe<CurrencyRefreshMessage>(refreshToken);
			Container.Unsubscribe<RefreshErrorMessage>(errorToken);
		}

		void SetupUI()
		{

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			currencyListView = (ListView)FindViewById(Resource.Id.currencyListView);
			currencyListView.Adapter = new CurrencyListAdapter(this, CurrentCurrencyList, CurrentCurrencyValue);
			currencyListView.ItemClick += (sender, e) =>
			{
				var adapter = (currencyListView.Adapter as CurrencyListAdapter);
				if (e.Position == 0)
				{
					SelectNewCurrencyAmount();
				}
				else
				{
					var newCurrency = adapter.Currencies.Currencys[e.Position - 1].Id;
					source.GetCurrencyForBase(newCurrency);
				}
			};
		}

		void SelectNewCurrencyAmount()
		{
			var inflator = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);


			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetTitle("New Currency Amount");
			builder.SetView(inflator.Inflate(Resource.Layout.NewCurrencyDialog, null));
			builder.SetPositiveButton("OK", (object sender, DialogClickEventArgs e) =>
			{
				var d = (AlertDialog)sender;

				var textbox = (EditText)d.FindViewById(Resource.Id.newCurrencyAmount);

				string val = textbox.Text;

				float floatVal = 0;
				if (float.TryParse(val, out floatVal))
				{
					var adapter = (currencyListView.Adapter as CurrencyListAdapter);
					CurrentCurrencyValue = floatVal;
					adapter.BaseCurrencyAmount = floatVal;
				}
			});
			builder.SetNegativeButton("Cancel", (object sender, DialogClickEventArgs e) =>
			{

			});


			AlertDialog dialog = builder.Create();

			dialog.Show();

		}

		TinyMessageSubscriptionToken reloadToken, refreshToken, errorToken;

		void SetupMessages()
		{
			reloadToken = Container.Subscribe<CurrencyHasReloadedMessage>(msg =>
			{
				CurrentCurrencyList = msg.NewCurrency;

				RunOnUiThread(() =>
				{
					(currencyListView.Adapter as CurrencyListAdapter).Currencies = msg.NewCurrency;
				});
			});
			refreshToken = Container.Subscribe<CurrencyRefreshMessage>(async msg => 
			{
				await source.GetCurrencyForBase(CurrentBaseCurrency);
			});

			errorToken = Container.Subscribe<RefreshErrorMessage>(msg =>
			{
				RunOnUiThread(() =>
				{
					Toast.MakeText(this, msg.Message, ToastLength.Short).Show();
				});
			});
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.main_activity_actions, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_refresh:
					Task.Factory.StartNew (() =>
					{
						source.RefreshFromSource();
						source.GetCurrencyForBase(CurrentBaseCurrency);
					});

					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
	}
}


