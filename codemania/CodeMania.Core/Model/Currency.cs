using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace CodeMania.Core.Model
{
	public class Currency
	{
		public string BaseCurrency { get; set; }
		public List<CurrencyRate> Currencys { get; set; }
	}

	public class CurrencyRate {
		[PrimaryKey]
		public string Id { get; set; }
		public float Rate { get; set; }
	}
}

