using System;
using BigTed.Core;
using CodeMania.Core.Client;
using CodeMania.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMania.Core
{
	//A mock currency source, because I'm stuck at Wellington Airport without (working) WIFI
	//and my flight has been delayed again...
	public class WellingtonCurrencySource : CurrencySource
	{
		public override  async Task RefreshFromSource()
		{
			var rates = new Currency()
			{
				BaseCurrency = "USD",
				Currencys = new List<CurrencyRate>()
			};

			rates.Currencys.Add(new CurrencyRate
			{
				Id = "USD",
				Rate = 1.0f
			});

			rates.Currencys.Add(new CurrencyRate
			{
				Id = "NZD",
				Rate = 1.16f
			});

			rates.Currencys.Add(new CurrencyRate
			{
				Id = "DOG",
				Rate = 10.0f
			});

			rates.Currencys.Add(new CurrencyRate
			{
				Id = "BTC",
				Rate = 1 / 565f
			});

			var database = Container.Resolve<ICurrencyDatabase>();
			await database.UpdateRates(rates);


			Container.PublishAsync(new CurrencyRefreshMessage());
		}
	}
}

