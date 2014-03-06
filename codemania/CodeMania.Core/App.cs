using System;
using BigTed.Core;
using CodeMania.Core.Client;
using SQLite.Net;

namespace CodeMania.Core
{
	public static class App
	{
		public static void Setup()
		{
			Container.RegisterSingleton<CurrencyClient>();
			Container.RegisterSingleton<CurrencySource>();
			Container.RegisterSingleton<CurrencyDatabase>();
			Container.RegisterSingleton<Logger>();

			var source = Container.Resolve<CurrencySource>();
			source.CurrencySet = new []
			{
				"USD", "NZD", "GBP", "JPY", "EUR", "AUD", "SKK", "INR", "CAD", "CHF", "DOG", "BTC"
			};
		}

		static SQLiteConnection conn = null;

		public static void SetSqlConnection(SQLiteConnection newConnection)
		{
			conn = newConnection;
			var db = Container.Resolve<CurrencyDatabase>();
			db.Connection = conn;
			db.RegisterTables();
		}

		public static CurrencyDatabase Database
		{
			get
			{
				return Container.Resolve<CurrencyDatabase>();
			}
		}
	}
}

