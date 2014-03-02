using System;
using CodeMania.Core.Client;
using System.IO;
using CodeMania.Core;

namespace CodeMania.IOS
{
	public class PlatformSetup
	{
		public static void Setup()
		{
			TinyIoCWrapper.Init ();

			//the IOS simulator can't use localhost!
			CurrencyClient.ClientUrl = "http://192.168.1.184:3000/codemania/rates?baseCurrency={0}";
		}

		public static void SetupDatabase()
		{
			var sqliteFilename = "codemania.db3";
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			var path = Path.Combine(documentsPath, sqliteFilename);

			var plat = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
			var conn = new SQLite.Net.SQLiteConnection(plat, path, true);

			Console.WriteLine (path);

			App.SetSqlConnection (conn);

		}
	}
}

