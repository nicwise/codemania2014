using System;
using SQLite.Net;
using System.Threading.Tasks;
using CodeMania.Core.Model;
using System.Linq;
using System.Collections.Generic;

namespace CodeMania.Core
{
	public class CurrencyDatabase
	{
		public CurrencyDatabase ()
		{
		}

		public SQLiteConnection Connection { get; set; }

		public void RegisterTables ()
		{
			Connection.CreateTable<CurrencyRate> ();
		}

		public async Task<Currency> GetRates ()
		{

			var currency = new Currency ();
			currency.BaseCurrency = "USD";

			var table = Connection.Table<CurrencyRate> ();
			if (table.Count() > 0)
			{
				currency.Currencys = (from c in Connection.Table<CurrencyRate>()
					select c).ToList<CurrencyRate> ();
			} else {
				currency.Currencys = new List<CurrencyRate> ();
			}




			return currency;

		}

		public async Task UpdateRates (Currency newRates)
		{
			Connection.DeleteAll<CurrencyRate> ();
			Connection.InsertAll(newRates.Currencys);
		}
	}
}

