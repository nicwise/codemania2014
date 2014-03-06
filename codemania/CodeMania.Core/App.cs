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
			Container.RegisterSingleton<ICurrencyClient, CurrencyClient>();
			Container.RegisterSingleton<ICurrencySource, WellingtonCurrencySource>();
			Container.RegisterSingleton<ICurrencyDatabase, CurrencyDatabase>();
			Container.RegisterSingleton<ILogger, Logger>();

			var source = Container.Resolve<ICurrencySource>();
			source.CurrencySet = new []
			{
				"USD", "NZD", "GBP", "JPY", "EUR", "AUD", "SKK", "INR", "CAD", "CHF", "DOG", "BTC"
			};
		}

		static SQLiteConnection conn = null;

		public static void SetSqlConnection(SQLiteConnection newConnection)
		{
			conn = newConnection;
			var db = Container.Resolve<ICurrencyDatabase>();
			db.Connection = conn;
			db.RegisterTables();
		}

		public static ICurrencyDatabase Database
		{
			get
			{
				return Container.Resolve<ICurrencyDatabase>();
			}
		}
	}
}

