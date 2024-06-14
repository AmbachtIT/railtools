using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;

namespace Ambacht.Common.Visualization
{
	public class LinearPalette(string[] colors, double minValue, double maxValue) : IPalette<double>
	{

		public string GetColor(double value)
		{
			var alpha = MathUtil.ReverseLerp(minValue, maxValue, value).Clamp(0, 1);
			var index = (int)Math.Round(alpha * (colors.Length - 1));
			return colors[index];
		}


		public LinearPalette Transform(Func<string, string> transformColor) => new LinearPalette(
			colors.Select(transformColor).ToArray(),
			minValue,
			maxValue);

	}
}
