using Ambacht.Common.Maps.Tiles;
using Ambacht.Common.Mathmatics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Maps.Nts;
using NetTopologySuite.Geometries;

namespace Ambacht.Common.Maps.Projections
{
	public struct VectorMapView
	{

		/// <summary>
		/// Location the map is centered on, in world coordinates
		/// </summary>
		public Vector2<double> Center { get; set; }

		/// <summary>
		/// Size of the viewport in pixels
		/// </summary>
		public Vector2 Size { get; set; }

		public Vector2 HalfSize => Size / 2f;

		/// <summary>
		/// Scale, in pixels per unit
		/// </summary>
		public double Scale { get; set; }

		public Vector2 WorldToScreen(Coordinate coord)
		{
			var result = coord.ToVector2();
			result -= Center;
			result *= Scale;
			result += HalfSize.ToVector2D();
			return result.Cast<float>().ToVector2();
		}


		public VectorMapView Pan(Vector2 deltaPixels)
		{
			return this with
			{
				Center = Center - new Vector2<double>(deltaPixels.X / Scale, deltaPixels.Y / Scale)
			};
		}

		public VectorMapView Fit(Rectangle<double> bounds)
		{
			if (bounds.Width <= 0 || bounds.Height <= 0)
			{
				return this;
			}
			var scaleX = Size.X / bounds.Width;
			var scaleY = Size.Y / bounds.Height;
			var scale = Math.Min(scaleX, scaleY);
			return this with
			{
				Center = bounds.Center(),
				Scale = Math.Min(scaleX, scaleY)
			};
		}

	}
}
