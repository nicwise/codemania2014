using System;
using System.Diagnostics;

namespace BigTed.Core
{
	public interface ILogger
	{
		void Log(string msg);

		void Log(string msg, params object[] p);

		void Log(Exception ex);
	}

	public class Logger : ILogger
	{
		public Logger()
		{
		}

		public void Log(string msg)
		{
			Debug.WriteLine(msg);
		}

		public void Log(string msg, params object[] p)
		{
			Log(string.Format(msg, p));
		}

		public void Log(Exception ex)
		{
			Log(ex.Message);
			Log(ex.ToString());
		}
	}
}

