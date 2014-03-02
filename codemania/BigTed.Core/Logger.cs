using System;
using System.Diagnostics;

namespace BigTed.Core
{
	public class Logger
	{
		public Logger ()
		{
		}

		public void Log(string msg) 
		{
			Debug.WriteLine (msg);
		}

		public void Log(string msg, params object[] p)
		{
			Log (string.Format (msg, p));
		}
	}
}

