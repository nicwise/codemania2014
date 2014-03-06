using System;
using NUnit.Framework;
using CodeMania.Core.Client;
using CodeMania.Core.Model;
using BigTed.Core;

namespace CodeMania.Tests
{
	[TestFixture]
	public class ClientFixture
	{
		[TestFixtureSetUp]
		public void Setup()
		{

		}

		[Test]
		public async void ClientCanConnectAndGetResults()
		{
			var client = new CurrencyClient();

			Currency res = await client.GetRates();

			Assert.IsNotNull(res, "We shoud have got a result back");
			Assert.AreEqual("USD", res.BaseCurrency);
			Assert.Greater(res.Currencys.Count, 0, "should have more than 0 currencies");

		}

		[Test]
		public async void ClientCanConnectAndGetResultsWithNZD()
		{
			var client = new CurrencyClient();

			Currency res = await client.GetRates();

			Assert.IsNotNull(res, "We shoud have got a result back");
			Assert.AreEqual("USD", res.BaseCurrency);
			Assert.Greater(res.Currencys.Count, 0, "should have more than 0 currencies");

		}

		[Test]
		public async void ClientReturnsNullWithIncorrectUrl()
		{
			var client = new CurrencyClient();
			CurrencyClient.ClientUrl = "http://whatever.com/{0}";

			Currency res = await client.GetRates();

			Assert.IsNull(res, "the result should really be null");

		}
	}
}

