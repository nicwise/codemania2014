using System;
using TinyMessenger;
using CodeMania.Core.Model;

namespace CodeMania.Core
{
	public class CurrencyRefreshMessage : TinyMessageBase
	{
		public CurrencyRefreshMessage () : base (new object ())
		{

		}
	}

	public class CurrencyHasReloadedMessage : TinyMessageBase
	{
		public CurrencyHasReloadedMessage (Currency newCurrency) : base (newCurrency)
		{
			NewCurrency = newCurrency;
		}

		public Currency NewCurrency { get; set; }
	}

	public class RefreshErrorMessage : TinyMessageBase
	{
		public RefreshErrorMessage (string message) : base (message)
		{
			Message = message;
		}

		public string Message { get; set; }
	}
}

