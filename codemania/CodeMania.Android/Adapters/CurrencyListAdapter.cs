using System;
using Android.Widget;
using CodeMania.Core.Model;
using Android.Content;
using System.Collections.Generic;
using Mono.Security;
using Android.Views;
using System.Collections;
using System.Reflection;
using Java.Lang.Reflect;
using Android;
using Android.Graphics.Drawables;

namespace CodeMania.Android
{
	public class CurrencyListAdapter : BaseAdapter
	{
		Currency currencies;
		float baseCurrencyAmount;
		LayoutInflater inflater;

		public CurrencyListAdapter(Context context, Currency rates, float baseCurrencyAmount)
		{
			this.currencies = rates;
			this.baseCurrencyAmount = baseCurrencyAmount;
			inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
		}

		public Currency Currencies
		{
			get { return currencies; }
			set
			{
				currencies = value;
				NotifyDataSetChanged();
			}
		}

		public float BaseCurrencyAmount
		{
			get { return baseCurrencyAmount; }
			set
			{
				baseCurrencyAmount = value;
				NotifyDataSetChanged();
			}
		}

		public override int Count
		{
			get
			{
				if (currencies == null)
					return 0;

				return currencies.Currencys.Count + 1;
			}
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		int FlagIdFromCurrencyName(string currency)
		{

			Type drawableType = typeof(Resource.Drawable);

			var field = drawableType.GetField(currency);

			if (field != null)
			{
				return (int)field.GetValue(null);
			}

			return -1;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			Console.WriteLine("line: " + position);
			if (position == 0)
			{
				if (convertView == null)
				{
					convertView = inflater.Inflate(Resource.Layout.HeaderView, parent, false);
				}
				var currencyName = (TextView)convertView.FindViewById(Resource.Id.currencyName);
				var currencyValue = (TextView)convertView.FindViewById(Resource.Id.currencyValue);
				var backgroundLayout = (RelativeLayout)convertView.FindViewById(Resource.Id.backgroundLayout);

				currencyName.Text = currencies.BaseCurrency;
				currencyValue.Text = BaseCurrencyAmount.FormatCurrency(currencies.BaseCurrency);
				backgroundLayout.SetBackgroundResource(FlagIdFromCurrencyName(currencies.BaseCurrency));

			}
			else
			{
				var thisRate = currencies.Currencys[position - 1];
				if (convertView == null)
				{
					convertView = inflater.Inflate(Resource.Layout.CurrencyView, parent, false);
				}
				var currencyName = (TextView)convertView.FindViewById(Resource.Id.currencyName);
				var currencyRate = (TextView)convertView.FindViewById(Resource.Id.currencyRate);
				var currencyValue = (TextView)convertView.FindViewById(Resource.Id.currencyValue);

				currencyName.Text = thisRate.Id;
				currencyRate.Text = thisRate.Rate.ToString();
				currencyValue.Text = (BaseCurrencyAmount * thisRate.Rate).FormatCurrency(thisRate.Id);

			}

			return convertView;


		}
	}
}

