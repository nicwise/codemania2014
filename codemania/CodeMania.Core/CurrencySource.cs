using System;
using BigTed.Core;
using CodeMania.Core.Client;
using CodeMania.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMania.Core
{
	public class CurrencySource
	{
		public Logger Log { get; set; }

		public CurrencySource (Logger log)
		{
			Log = log;
		}

		public string[] CurrencySet { get; set; }

		public async void RefreshFromSource ()
		{
			var client = Container.Resolve<CurrencyClient> ();
			var rates = await client.GetRates ();

			if (rates == null)
			{
				Container.PublishAsync (new RefreshErrorMessage ("Latest rates not available"));
				Log.Log ("No rates returned - internet down?");
				return;
			}

			var database = Container.Resolve<CurrencyDatabase> ();
			await database.UpdateRates (rates);

			Log.Log ("Publishing a refresh");
			Container.PublishAsync (new CurrencyRefreshMessage ());
		}

		public async void GetCurrencyForBase (string baseCurrency)
		{

			var database = Container.Resolve<CurrencyDatabase> ();
			var usdRates = await database.GetRates ();

			var baseRates = await RebaseRates (usdRates, baseCurrency);

			Log.Log ("Currencies reloaded using " + baseCurrency + " as the base");
			Container.PublishAsync (new CurrencyHasReloadedMessage (FilterRates (baseRates, baseCurrency, CurrencySet)));

		}

		public Currency FilterRates (Currency source, string baseCurrency, params string[] validCurrency)
		{
			return new Currency () {
				BaseCurrency = source.BaseCurrency,
				Currencys = (from x in source.Currencys
				             where validCurrency.Contains (x.Id) && x.Id != baseCurrency
				             orderby x.Id
				             select x).ToList ()
			};

		}

		public async Task<Currency> RebaseRates (Currency usdRates, string baseCurrency)
		{
			var currency = new Currency {
				BaseCurrency = baseCurrency
			};

			currency.Currencys = (from rate in usdRates.Currencys
			                      select new CurrencyRate { Id = rate.Id, Rate = RebaseSingleCurrency (usdRates, baseCurrency, rate.Id) }).ToList ();

			return currency;
		}

		float RebaseSingleCurrency (Currency usdRates, string source, string dest)
		{
			if (source == dest)
				return 1;

			var sourceRate = usdRates.Currencys.FirstOrDefault (x => x.Id == source);
			var destRate = usdRates.Currencys.FirstOrDefault (x => x.Id == dest);


			//from USD to something
			if (source == usdRates.BaseCurrency)
			{
				if (destRate != null)
				{
					return destRate.Rate;
				}
				return -1;
			}

			//from something to USD
			if (dest == usdRates.BaseCurrency)
			{
				if (sourceRate != null)
				{
					return 1.0f / sourceRate.Rate;
				}
				return -1;
			}

			if (sourceRate != null && destRate != null)
			{
				var sourceInBase = 1.0f / sourceRate.Rate;
				return sourceInBase * destRate.Rate;
			} else
			{
				return -1;
			}

		}
	}
}

