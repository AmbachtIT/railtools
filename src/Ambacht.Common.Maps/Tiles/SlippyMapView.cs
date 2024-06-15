using ProjNet.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Blazor.Svg;
using Ambacht.Common.Mathmatics;

namespace Ambacht.Common.Maps.Tiles
{
	public struct SlippyMapView
	{
		/// <summary>
		/// map coordinates the view is centered on
		/// </summary>
		public LatLng Coords { get; set; }

		/// <summary>
		/// Size of the viewport in pixels
		/// </summary>
		public Vector2<double> Size { get; set; }

		public Vector2<double> HalfSize => new(Size.X / 2, Size.Y / 2);

		/// <summary>
		/// Zoom
		/// </summary>
		/// <remarks>
		/// The minimum zoom level is 0.
		///
		/// The maximum zoom level depends on the tile set and usually is somewhere between 15 and 20. 
		/// </remarks>
		public double Zoom { get; set; }

		public int GetZoomLevel(int min, int max)
		{
			return (int)Math.Ceiling(Zoom).Clamp(min, max);
		}



		/// <summary>
		/// Tile size, in pixels
		/// </summary>
		public int TileSize { get; set; }

		/// <summary>
		/// Rotation angle, in degrees
		/// </summary>
		public double AngleDegrees { get; set; }

		/// <summary>
		/// Rotation angle, in degrees
		/// </summary>
		public double AngleRadians => MathUtil.DegreesToRadians(AngleDegrees);

		public float MetersPerPixelF => (float) SlippyMath.MetersPerPixel(Coords, Zoom, TileSize);


		public SlippyMapView Pan(Vector2<double> delta)
		{
			var currentCoords = LatLngToView(this.Coords);
			currentCoords -= delta;
			var newCoords = ViewToLatLng(currentCoords);
			return this with
			{
				Coords = newCoords
			};
		}

		/// <summary>
		/// Converts the lat/lng to view coordinates (x, y) = ([0-width], [y-height])
		/// </summary>
		/// <param name="coords"></param>
		/// <returns></returns>
		public Vector2<double> LatLngToView(LatLng coords)
		{
			var position = SlippyMath.LatLngToPixel(coords, Zoom, TileSize);
			position -= SlippyMath.LatLngToPixel(this.Coords, Zoom, TileSize);
			position = MathUtil.Rotate(position, MathUtil.DegreesToRadians(AngleDegrees));
			position += Size / 2;
			return position;
		}




		/// <summary>
		/// Converts the view coordinates (x, y) = ([0-width], [y-height]) to lat/lng 
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public LatLng ViewToLatLng(Vector2<double> position)
		{
			position -= Size / 2;
			position = MathUtil.Rotate(position, MathUtil.DegreesToRadians(-AngleDegrees));

			position += SlippyMath.LatLngToPixel(this.Coords, Zoom, TileSize);
			return SlippyMath.PixelToLatLng(position, Zoom, TileSize);
		}

		public IEnumerable<SlippyMapTile> GetVisibleTiles(int tileLevel)
		{
			if (TileSize <= 0)
			{
				yield break;
			}

			var tileCount = 1 << tileLevel;
			var center = SlippyMath.LatLngToPixel(this.Coords, tileLevel, TileSize);
			var left = (int) Math.Floor(center.X - Size.X / 1.4);
			var top = (int) Math.Floor(center.Y - Size.Y / 1.4);
			var sx = left / TileSize;
			var sy = top / TileSize;

			for (var dy = 0; dy <= Math.Ceiling(Size.Y * 1.5 / TileSize); dy++)
			{
				for (var dx = 0; dx <= Math.Ceiling(Size.X * 1.5 / TileSize); dx++)
				{
					var x = sx + dx;
					var y = sy + dy;
					if (x < tileCount && y < tileCount)
					{
						yield return new SlippyMapTile(x, y, tileLevel);
					}
				}
			}
		}

		public override string ToString()
		{
			return $"{Coords.Latitude}, {Coords.Longitude} z{Zoom} ({Size.X}, {Size.Y})";
		}



		public SlippyMapView Fit(LatLngBounds bounds, float margin)
		{
			if (Size.X <= 0 || Size.Y <= 0)
			{
				throw new InvalidOperationException("Unabled to determine zoom level, width and/or height are zero.");
			}

			var newCenter = bounds.Center();
			if (bounds.IsEmpty)
			{
				return this with
				{
					Coords = newCenter
				};
			}

			var boundsMeters = bounds.ApproximateSizeMeters() * (margin + 1);

			var newZoom = 25.0;
			while (newZoom > 0)
			{
				var metersPerPixel = SlippyMath.MetersPerPixel(Coords, newZoom, TileSize);
				var visibleWidth = metersPerPixel * Size.X;
				var visibleHeight = metersPerPixel * Size.Y;
				if (visibleWidth > boundsMeters.X && visibleHeight > boundsMeters.Y)
				{
					break;
				}
				newZoom -= .25;
			}
			return this with
			{
				Coords = newCenter,
				Zoom = newZoom
			};
		}



	}


}
