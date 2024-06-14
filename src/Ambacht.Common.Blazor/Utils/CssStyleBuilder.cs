using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Blazor.Utils
{
	public class CssStyleBuilder
	{

		private readonly StringBuilder _builder = new StringBuilder();
		private readonly IFormatProvider _neutral = CultureInfo.InvariantCulture;

		public string Build() => _builder.ToString();


		public CssStyleBuilder Add(string key, string value, bool enable = true)
		{
			if (enable)
			{
				_builder.Append($"{key}: {value}; ");
			}

			return this;
		}

	}
}
