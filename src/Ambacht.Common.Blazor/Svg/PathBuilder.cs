using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Blazor.Svg
{
	public class PathBuilder
	{

		private readonly StringBuilder _builder = new StringBuilder();
		private readonly IFormatProvider _neutral = CultureInfo.InvariantCulture;


		public string Build() => _builder.ToString();

		public PathBuilder Move(Vector2 v) => Command("M", v);

		public PathBuilder Line(Vector2 v) => Command("L", v);


		public PathBuilder Command(string command, Vector2 v)
		{
			_builder.Append(command);
			_builder.Append(v.X.ToString(_neutral));
			_builder.Append(' ');
			_builder.Append(v.Y.ToString(_neutral));
			_builder.Append(' ');
			return this;
		}


		public PathBuilder Close()
		{
			_builder.Append("Z ");
			return this;
		}

	}
}
