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
		public static string ClientUrl { get; set; }

		public CurrencyClient()
		{
			if (string.IsNullOrEmpty(ClientUrl))
				ClientUrl = "http://localhost:3000/codemania/rates?baseCurrency={0}";

		}

		public async Task<Currency> GetRates()
		{
			try
			{
				var httpClient = new HttpClient();
				HttpResponseMessage response = await httpClient.GetAsync(string.Format(ClientUrl, "USD"));
				if (response.IsSuccessStatusCode)
				{
					var res = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<Currency>(res);

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

