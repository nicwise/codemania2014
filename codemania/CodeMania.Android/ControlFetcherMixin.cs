using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Android.Views;
using System;
using System.Linq;
using Android.App;

/*
 * 
 * From https://gist.github.com/paulcbetts/8212078
 * Use it like this:
 * 
 public TextView Login {
        get { return this.GetControl<TextView>(); }
    }
 
    protected override void OnCreate(Bundle bundle)
    {
        SetContentView(Resource.Layout.Login);
 
        Login.Text = "Foo";
    }
 */

namespace CodeMania.Android
{
	public static class ControlFetcherMixin
	{
		static readonly Dictionary<string, int> controlIds;
		static ConditionalWeakTable<object, Dictionary<string, View>> viewCache = new ConditionalWeakTable<object, Dictionary<string, View>> ();

		static ControlFetcherMixin ()
		{
			// NB: This is some hacky shit, but on MonoAndroid at the moment, 
			// this is always the entry assembly.
			var assm = AppDomain.CurrentDomain.GetAssemblies () [1];
			var resources = assm.GetModules ().SelectMany (x => x.GetTypes ()).First (x => x.Name == "Resource");

			controlIds = resources.GetNestedType ("Id").GetFields ()
			.Where (x => x.FieldType == typeof(int))
			.ToDictionary (k => k.Name.ToLowerInvariant (), v => (int)v.GetRawConstantValue ());
		}

		public static T GetControl<T> (this Activity This, [CallerMemberName]string propertyName = null)
		where T : View
		{
			return (T)getCachedControl (propertyName, This,
				() => This.FindViewById (controlIds [propertyName.ToLowerInvariant ()]));
		}

		public static T GetControl<T> (this View This, [CallerMemberName]string propertyName = null)
		where T : View
		{
			return (T)getCachedControl (propertyName, This,
				() => This.FindViewById (controlIds [propertyName.ToLowerInvariant ()]));
		}

		static View getCachedControl (string propertyName, object rootView, Func<View> fetchControlFromView)
		{
			var ret = default(View);
			var ourViewCache = viewCache.GetOrCreateValue (rootView);

			if (ourViewCache.TryGetValue (propertyName, out ret))
			{
				return ret;
			}

			ret = fetchControlFromView ();

			ourViewCache.Add (propertyName, ret);
			return ret;
		}
	}
}