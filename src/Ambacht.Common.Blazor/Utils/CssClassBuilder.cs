using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Blazor.Utils
{
	public class CssClassBuilder
	{

		private readonly StringBuilder _builder = new StringBuilder();
		private readonly IFormatProvider _neutral = CultureInfo.InvariantCulture;

		public string Build() => _builder.ToString();


		public CssClassBuilder Add(string value, bool enable = true)
		{
			if (enable)
			{
				if (_builder.Length > 0)
				{
					_builder.Append(' ');
				}
				_builder.Append(value);
			}

			return this;
		}

	}
}
