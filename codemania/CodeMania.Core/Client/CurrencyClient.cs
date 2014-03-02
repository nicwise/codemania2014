using System;
using System.Net.Http;
using CodeMania.Core.Model;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CodeMania.Core.Client
{
	public class CurrencyClient
	{
		public static string ClientUrl { get; set; }

		public CurrencyClient ()
		{
			if (string.IsNullOrEmpty (ClientUrl))
				ClientUrl = "http://localhost:3000/codemania/rates?baseCurrency={0}";
		}

		public async Task<Currency> GetRates ()
		{

			var httpClient = new HttpClient ();
			HttpResponseMessage response = await httpClient.GetAsync (string.Format (ClientUrl, "USD"));
			if (response.IsSuccessStatusCode)
			{
				var res = await response.Content.ReadAsStringAsync ();
				return JsonConvert.DeserializeObject<Currency> (res);

			}
			
			return null;
		}

	}
}

