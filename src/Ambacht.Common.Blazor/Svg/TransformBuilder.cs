using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;

namespace Ambacht.Common.Blazor.Svg
{
	public class TransformBuilder
	{

		private readonly StringBuilder _builder = new StringBuilder();
		private readonly IFormatProvider _neutral = CultureInfo.InvariantCulture;

		public string Build() => _builder.ToString();

		public TransformBuilder Rotate(double degrees)
		{
			Append($"rotate({degrees.ToString(_neutral)}deg)");
			return this;
		}

		public TransformBuilder RotateRadians(float radians) => Rotate(MathUtil.RadiansToDegreesF(radians));

		public TransformBuilder Translate(Vector2 v) => Translate(v.X.ToString(_neutral), v.Y.ToString(_neutral));

		public TransformBuilder Translate(string x, string y)
		{
			Append($"translate({x}, {y})");
			return this;
		}


		private void Append(string str)
		{
			if (_builder.Length > 0)
			{
				_builder.Append(' ');
			}

			_builder.Append(str);
		}

		public TransformBuilder Scale(double amount)
		{
			Append($"scale({amount.ToString(_neutral)})");
			return this;
		}
	}
}
