using System;
using System.Collections.Generic;

namespace CodeMania
{
	public class OpenExchangeModel
	{
		public OpenExchangeModel()
		{
			Rates = new Dictionary<string, float>();
		}

		public string Base { get; set; }

		public Dictionary<string, float> Rates { get; set; }
	}
}

