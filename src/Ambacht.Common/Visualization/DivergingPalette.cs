using Ambacht.Common.Mathmatics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Visualization
{
	public class DivergingPalette(string[] colorsNegative, string[] colorsPositive, double minValue, double maxValue) : IPalette<double>
	{


		public string GetColor(double value)
		{
			if (value < 0)
			{
				return GetColor(colorsNegative, -minValue, -value);
			}

			return GetColor(colorsPositive, maxValue, value);
		}

		private string GetColor(string[] colors, double max, double value)
		{
			var alpha = MathUtil.ReverseLerp(0, max, value).Clamp(0, 1);
			var index = (int)Math.Round(alpha * (colors.Length - 1));
			return colors[index];
		}


		public static DivergingPalette Create(double minValue, double maxValue)
		{
			if (minValue > maxValue)
			{
				throw new InvalidOperationException();
			}

			return new DivergingPalette(ColorBrewer.Purple9, ColorBrewer.Blues9, minValue, maxValue);
		}

		public static DivergingPalette CreateRedGreenInverted(double minValue, double maxValue)
		{
			if (minValue > maxValue)
			{
				throw new InvalidOperationException();
			}

			return new DivergingPalette(ColorBrewer.Greens9, ColorBrewer.Reds9, minValue, maxValue);
		}


		public DivergingPalette Transform(Func<string, string> transformColor) => new DivergingPalette(
					colorsNegative.Select(transformColor).ToArray(), 
					colorsPositive.Select(transformColor).ToArray(), 
					minValue, 
					maxValue);

	}
}
