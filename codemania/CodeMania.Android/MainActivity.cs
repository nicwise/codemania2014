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

namespace CodeMania.Android
{
	[Activity(Label = "Quick Currency", MainLauncher = true)]
	public class MainActivity : Activity
	{
		ICurrencySource source;
		string CurrentBaseCurrency = "USD";
		float CurrentCurrencyValue = 100f;
		Currency CurrentCurrencyList;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);


			PlatformSetup.Setup();
			App.Setup();

			PlatformSetup.SetupDatabase();

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);


			source = Container.Resolve<ICurrencySource>();
			source.RefreshFromSource();

			SetupMessages();

		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			Container.Unsubscribe<CurrencyHasReloadedMessage>(reloadToken);
			Container.Unsubscribe<CurrencyRefreshMessage>(refreshToken);
			Container.Unsubscribe<RefreshErrorMessage>(errorToken);
		}

		TinyMessageSubscriptionToken reloadToken, refreshToken, errorToken;

		void SetupMessages()
		{
			reloadToken = Container.Subscribe<CurrencyHasReloadedMessage>(msg =>
			{
				CurrentCurrencyList = msg.NewCurrency;

				RunOnUiThread(() =>
				{
					foreach (var rate in CurrentCurrencyList.Currencys)
					{
						Console.WriteLine(rate.Id + " " + rate.Rate);
					}
				});
			});
			refreshToken = Container.Subscribe<CurrencyRefreshMessage>(msg =>
			{
				source.GetCurrencyForBase(CurrentBaseCurrency);
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
					source.RefreshFromSource();
					source.GetCurrencyForBase(CurrentBaseCurrency);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
	}
}


