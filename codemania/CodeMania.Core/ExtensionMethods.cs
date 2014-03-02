using System;
using System.Globalization;

namespace CodeMania
{
	public static class ExtensionMethods
	{
		public static string FormatCurrency(this float value, string currencyName)
		{
			string locale = "";
			switch(currencyName)
			{
				case "EUR":
					locale = "fr-FR";
					break;
				case "GBP":
					locale = "en-GB";
					break;
				case "CHF":
					locale = "de-CH";
					break;
				case "NZD":
					locale = "en-NZ";
					break;
				case "AUD":
					locale = "en-AU";
					break;
				case "CAD":
					locale = "en-CA";
					break;
				case "INR":
					locale = "hi-IN";
					break;
				case "JPY":
					locale = "ja-JP";
					break;
				case "USD":
					locale = "en-US";
					break;
			}



			if (string.IsNullOrEmpty(locale))
			{
				return string.Format ("{0:0.00} {1}", value, currencyName);
			}

			var culture = new CultureInfo(locale);

			return string.Format(culture, "{0:c}", value);
		}
	}
}

