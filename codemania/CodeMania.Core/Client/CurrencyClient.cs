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
	public class CurrencyClient
	{
		public static string ClientUrl { get; set; }
		public Logger Log { get; set; }

		public CurrencyClient (Logger log)
		{
			if (string.IsNullOrEmpty (ClientUrl))
				ClientUrl = "http://localhost:3000/codemania/rates?baseCurrency={0}";

			Log = log;
		}

		public async Task<Currency> GetRates ()
		{
			try
			{
				var httpClient = new HttpClient ();
				HttpResponseMessage response = await httpClient.GetAsync (string.Format (ClientUrl, "USD"));
				if (response.IsSuccessStatusCode)
				{
					var res = await response.Content.ReadAsStringAsync ();
					return JsonConvert.DeserializeObject<Currency> (res);

				}
			} catch (Exception ex)
			{
				Log.Log (ex);
			}
			return null;
		}

		public async Task<CurrencyRate> GetDoge()
		{

			try
			{
				var httpClient = new HttpClient ();
				HttpResponseMessage response = await httpClient.GetAsync ("https://www.dogeapi.com/wow/?a=get_current_price");
				if (response.IsSuccessStatusCode)
				{
					var res = await response.Content.ReadAsStringAsync ();
					return new CurrencyRate {
						Id = "DOG",
						Rate = 1/float.Parse (res)
					};

				}


			} catch (Exception ex)
			{
				Log.Log (ex);

			}

			return new CurrencyRate {
				Id = "DOG",
				Rate = 1000.0f
			};
		}

	}
}

