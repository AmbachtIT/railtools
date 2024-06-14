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
	public class SvgTransformBuilder
	{
		private readonly StringBuilder _builder = new StringBuilder();
		private readonly IFormatProvider _neutral = CultureInfo.InvariantCulture;

		public string Build() => _builder.ToString();


		public SvgTransformBuilder Translate(double x, double y)
		{
			_builder.AppendFormat(_neutral, "translate({0} {1})", x, y);
			return this;
		}

		public SvgTransformBuilder Translate(double v) => Scale(v, v);

		public SvgTransformBuilder Translate(Vector2 v) => Scale(v.X, v.Y);

		public SvgTransformBuilder Translate(Vector2<double> v) => Translate(v.X, v.Y);


		public SvgTransformBuilder Scale(double x, double y)
		{
			_builder.AppendFormat(_neutral, "scale({0} {1})", x, y);
			return this;
		}

		public SvgTransformBuilder Scale(double v) => Scale(v, v);

		public SvgTransformBuilder Scale(Vector2 v) => Scale(v.X, v.Y);

		public SvgTransformBuilder Scale(Vector2<double> v) => Scale(v.X, v.Y);


		public SvgTransformBuilder Rotate(double angleDegrees)
		{
			_builder.AppendFormat(_neutral, "rotate({0})", angleDegrees);
			return this;
		}


	}
}
