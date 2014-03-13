using System;
using System.Net.Http;
using CodeMania.Core.Model;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using BigTed.Core;
using System.Collections.Generic;

namespace CodeMania.Core.Client
{
	public interface ICurrencyClient
	{
		Task<Currency> GetRates();

		Task<CurrencyRate> GetDoge();
	}

	public class CurrencyClient : ICurrencyClient
	{
		//getting a key is free. Don't abuse mine too much.
		public static string ClientUrl = "http://openexchangerates.org/api/latest.json?app_id=705e4fac763b4613ac478aa2b78b97e3";

		public CurrencyClient()
		{
		}

		public async Task<Currency> GetRates()
		{
			try
			{
				var httpClient = new HttpClient();
				HttpResponseMessage response = await httpClient.GetAsync(ClientUrl);
				if (response.IsSuccessStatusCode)
				{
					var res = await response.Content.ReadAsStringAsync();
					var openExchangeData = JsonConvert.DeserializeObject<OpenExchangeModel>(res);

					var currency = new Currency();
					currency.BaseCurrency = openExchangeData.Base;
					foreach (var key in openExchangeData.Rates.Keys)
					{
						currency.Currencys.Add(new CurrencyRate
						{
							Id = key,
							Rate = openExchangeData.Rates[key]
						});
					}

					return currency;

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
			return null;
		}

		public async Task<CurrencyRate> GetDoge()
		{

			try
			{
				var httpClient = new HttpClient();
				HttpResponseMessage response = await httpClient.GetAsync("https://www.dogeapi.com/wow/?a=get_current_price");
				if (response.IsSuccessStatusCode)
				{
					var res = await response.Content.ReadAsStringAsync();
					return new CurrencyRate
					{
						Id = "DOG",
						Rate = 1 / float.Parse(res)
					};

				}


			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());

			}

			return new CurrencyRate
			{
				Id = "DOG",
				Rate = 1000.0f
			};
		}
	}
}

