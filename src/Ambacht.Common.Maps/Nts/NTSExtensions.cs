using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Maps.Nts;
using Ambacht.Common.Maps.Projections;
using Ambacht.Common.Maps.Tiles;
using Ambacht.Common.Mathmatics;
using NetTopologySuite;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Prepared;
using NetTopologySuite.Operation.Distance;
using NetTopologySuite.Utilities;
using Point = NetTopologySuite.Geometries.Point;

namespace Ambacht.Common.Maps.Nts
{
    public static class NTSExtensions
    {

        public static double GetAreaKM2(this Geometry geom)
        {
            return geom.GetAreaM2() / 1_000_000;
        }

        public static double GetAreaM2(this Geometry geom)
        {
            var scale = GetScale(geom);
            var area = geom.Area * scale * scale;
            if (area > EarthSurfaceAreaM2 / 2)
            {
                area = EarthSurfaceAreaM2 - area;
            }

            return area;
        }

        private static double GetScale(Geometry geometry)
        {
            switch (geometry.Factory.SRID)
            {
                case 3857:
                    return GetWebMercatorScale(geometry);
                default:
                    return 1;
            }
        }

        private static double GetWebMercatorScale(Geometry geometry)
        {
            var y = geometry.Centroid.Y;
            var latRadians = Math.Atan(Math.Exp(y / EarthRadius)) * 2 - Math.PI / 2;
            var lat = 180 * latRadians / Math.PI;
            return Math.Cos(latRadians);
        }


        public static Vector2<double>? GetNearestPoint(this Geometry geometry, Vector2<double> v)
        {
	        if (geometry.IsEmpty)
	        {
		        return null;
	        }
	        var point = new Point(v.X, v.Y);
	        var op = new DistanceOp(geometry, point);
	        var nearestResult = op.NearestPoints();
	        if (nearestResult == null)
	        {
		        return null;
	        }
	        var nearest = nearestResult[0];
	        return new Vector2<double>(nearest[0], nearest[1]);
        }


        public static void Project(this Geometry geom, Projection projection)
        {
            geom.Apply(new ProjectionCoordinateFilter(projection));
        }


        public const double EarthSurfaceAreaM2 = 510e12;
        public const double EarthRadius = 6378137;


        public static double GetLengthKM(this Geometry geom)
        {
            return geom.GetLengthM() / 1_000;
        }


        public static double GetLengthM(this Geometry geom)
        {
            var scale = GetScale(geom);
            return geom.Length * scale;
        }



        public static double DistanceTo(this Point p1, Point p2)
        {
            return p1.Distance(p2) * GetScale(p1);
        }


        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2((float)p.X, (float)p.Y);
        }



        public static Coordinate ToCoordinate(this Vector2 v)
        {
            return new Coordinate(v.X, v.Y);
        }

        public static Coordinate ToCoordinate(this Vector2<float> v)
        {
            return new Coordinate(v.X, v.Y);
        }

        public static Coordinate ToCoordinate(this Vector2<double> v)
        {
            return new Coordinate(v.X, v.Y);
        }


        public static Vector2<double> ToVector2(this Coordinate p)
        {
            return new Vector2<double>(p.X, p.Y);
        }

        public static Vector2 ToVector2F(this Coordinate p)
        {
	        return new Vector2((float)p.X, (float)p.Y);
        }

		public static LatLng ToLatLng(this Coordinate p, Projection projection = null)
        {
            if (projection != null)
            {
                var result = projection.Invert(new Vector2<double>(p.X, p.Y));
                return result;
            }

            return new LatLng(p.Y, p.X);
        }

        public static Coordinate ToCoordinate(this LatLng p, Projection projection = null)
        {
	        if (projection != null)
	        {
		        var result = projection.Project(p).ToCoordinate();
		        return result;
	        }

	        return new Coordinate(p.Latitude, p.Longitude);
        }

		public static Point ToPoint(this Vector2 v)
        {
            return new Point(v.X, v.Y);
        }


        public static Rectangle<double> GetBoundingRectangle(this IEnumerable<Geometry> geometries)
        {
	        return RectangleUtil.Cover(geometries.Select(g => g.GetBoundingRectangle<double>()));
        }

		public static IEnumerable<Vector2<T>> GetBoundary<T>(this Geometry geometry) where T : IFloatingPoint<T>
        {
            return geometry switch
            {
	            NetTopologySuite.Geometries.MultiPolygon multi => multi.Coordinates.GetBoundary<T>(),
				NetTopologySuite.Geometries.Polygon poly => poly.GetBoundary<T>(),
                _ => throw new InvalidOperationException()
            };
        }


        public static IEnumerable<Vector2<T>> GetBoundary<T>(this NetTopologySuite.Geometries.Polygon polygon)
            where T : IFloatingPoint<T>
        {
            return polygon.ExteriorRing.GetBoundary<T>();
        }

        public static IEnumerable<Vector2<T>> GetBoundary<T>(this LineString line)
            where T : IFloatingPoint<T>
        {
            return
                line
                    .Coordinates
                    .GetBoundary<T>();
        }

        public static IEnumerable<Vector2<T>> GetBoundary<T>(this IEnumerable<Coordinate> coords)
	        where T : IFloatingPoint<T>
        {
	        return
				coords
					.Select(c => new Vector2<double>(c[0], c[1]).Cast<T>());
        }



		public static Polygon CreatePolygon(this Geometry geometry, Projection projection)
        {
            return geometry switch
            {
                NetTopologySuite.Geometries.Polygon poly => poly.CreatePolygon(projection),
                LineString line => new Polygon()
                {
                    Points = line.CreatePolygon(projection)
                },
                _ => throw new InvalidOperationException()
            };
        }

        public static IEnumerable<Polygon> ToMultiPolygon(this Geometry geometry, Projection projection)
        {
            return geometry switch
            {
                NetTopologySuite.Geometries.Polygon poly => new[] { poly.CreatePolygon(projection) },
                MultiPolygon poly => poly.Geometries
                    .Select(g => g.CreatePolygon(projection)).ToArray(),
                LineString line => new[]
                {
                    new Polygon()
                    {
                        Points = line.CreatePolygon(projection)
                    }
                },
                _ => throw new InvalidOperationException()
            };
        }


        public static Polygon CreatePolygon(this NetTopologySuite.Geometries.Polygon polygon, Projection projection)
        {
            return new Polygon()
            {
                Points = polygon.ExteriorRing.CreatePolygon(projection),
                Holes = polygon.InteriorRings.Select(line => new Polygon()
                {
                    Points = line.CreatePolygon(projection)
                }).ToList()
            };
        }

        public static List<LatLng> CreatePolygon(this LineString line, Projection projection)
        {
            return
                line
                    .Coordinates
                    .Select(projection.Invert)
                    .ToList();
        }



        public static LatLng Invert(this Projection projection, Coordinate coord)
        {
            return projection.Invert(new Vector2<double>(coord.X, coord.Y));
        }



        public static Rectangle<T> GetBoundingRectangle<T>(this Geometry geometry)
            where T : IFloatingPoint<T>, IMinMaxValue<T>
        {
            if (geometry == null)
            {
                return Rectangle<T>.Empty;
            }

            return RectangleUtil.Cover(geometry.GetBoundary<T>());
        }


        public static bool Contains(this Geometry geometry, Vector2 v) => geometry.Contains(new Point(v.X, v.Y));

        public static bool Contains(this Geometry geometry, Vector2<double> v) =>
            geometry.Contains(new Point(v.X, v.Y));



        public static string GetSvgPath<T>(this NetTopologySuite.Geometries.Polygon polygon, WorldView<T> view)
            where T : IFloatingPoint<T>, IMinMaxValue<T>, ITrigonometricFunctions<T>
        {
            return polygon?.ExteriorRing?.GetSvgPath(view);
        }

		#region VectorMapView projection

		public static string GetSvgPath(this Geometry geometry, VectorMapView view)
		{
			return geometry switch
			{
				Point point => point.GetSvgPath(view),
				LineString line => line.GetSvgPath(view),
				NetTopologySuite.Geometries.Polygon poly => poly.GetSvgPath(view),
				GeometryCollection coll => string.Join(" ",
					coll.Geometries.Select(g => g.GetSvgPath(view))),
				_ => throw new InvalidOperationException()
			};
		}

		public static string GetSvgPath(this NetTopologySuite.Geometries.Polygon polygon, VectorMapView view)
		{
			return polygon?.ExteriorRing?.GetSvgPath(view);
		}

		public static string GetSvgPath(this LineString line, VectorMapView view) => line.Coordinates.GetSvgPath(view);


		public static string GetSvgPath(this Coordinate[] coords, VectorMapView view)
		{
			var builder = new StringBuilder();

			foreach (var coord in coords)
			{
				if (builder.Length == 0)
				{
					builder.Append("M");
				}
				else
				{
					builder.Append(" L");
				}

				builder.Append(' ');
				builder.Write(coord.ToVector2F());
			}

			return builder.ToString();
		}

		public static string GetSvgPath(this Point point, VectorMapView view)
		{
			var builder = new StringBuilder();
			builder.Append("M");
			builder.Write(point.Coordinate.ToVector2F());
			return builder.ToString();
		}

		public static void Write(this StringBuilder builder, Vector2 pos)
		{
			builder.Append(pos.X.ToString(_neutral));
			builder.Append(" ");
			builder.Append(pos.Y.ToString(_neutral));
		}


		#endregion

		public static string GetSvgPath(this Geometry geometry, SlippyMapView view, Projection projection = null)
        {
            return geometry switch
            {
                Point point => point.GetSvgPath(view, projection),
                LineString line => line.GetSvgPath(view, projection),
                NetTopologySuite.Geometries.Polygon poly => poly.GetSvgPath(view, projection),
                GeometryCollection coll => string.Join(" ",
                    coll.Geometries.Select(g => g.GetSvgPath(view, projection))),
                _ => throw new InvalidOperationException()
            };
        }

        public static string GetSvgPath(this NetTopologySuite.Geometries.Polygon polygon, SlippyMapView view, Projection projection)
        {
            return polygon?.ExteriorRing?.GetSvgPath(view, projection);
        }


        public static string GetSvgPath(this LatLngBounds rect, SlippyMapView view)
        {
            var builder = new StringBuilder();

            foreach (var coord in new[]
                     {
                         rect.NorthEast, rect.NorthWest, rect.SouthWest, rect.SouthEast, rect.NorthEast
                     })
            {
                if (builder.Length == 0)
                {
                    builder.Append("M");
                }
                else
                {
                    builder.Append(" L");
                }

                builder.Append(' ');
                builder.Write(coord, view);
            }

            return builder.ToString();
        }

        public static string GetSvgPath<T>(this LineString line, WorldView<T> view)
            where T : IFloatingPoint<T>, IMinMaxValue<T>, ITrigonometricFunctions<T>
        {
            var builder = new StringBuilder();

            foreach (var coord in line.Coordinates)
            {
                if (builder.Length == 0)
                {
                    builder.Append("M");
                }
                else
                {
                    builder.Append(" L");
                }

                builder.Append(' ');
                builder.Write(coord, view);
            }

            return builder.ToString();
        }

        public static string GetSvgPath(this LineString line, SlippyMapView view, Projection projection) => line.Coordinates.GetSvgPath(view, projection);


        public static string GetSvgPath(this Coordinate[] coords, SlippyMapView view, Projection projection)
        {
            var builder = new StringBuilder();

            foreach (var coord in coords)
            {
                if (builder.Length == 0)
                {
                    builder.Append("M");
                }
                else
                {
                    builder.Append(" L");
                }

                builder.Append(' ');
                builder.Write(coord.ToLatLng(projection), view);
            }

            return builder.ToString();
        }

        public static string GetSvgPath(this Point point, SlippyMapView view, Projection projection)
        {
            var builder = new StringBuilder();
            builder.Append("M");
            builder.Write(point.Coordinate.ToLatLng(projection), view);
            return builder.ToString();
        }



        public static void Write<T>(this StringBuilder builder, Coordinate coord, WorldView<T> view)
            where T : IFloatingPoint<T>, IMinMaxValue<T>, ITrigonometricFunctions<T>
        {
            var pos = view.WorldToScreen(new Vector2<double>(coord.X, coord.Y).Cast<T>());
            builder.Append(pos.ToString("X Y", _neutral));
        }


        public static void Write(this StringBuilder builder, LatLng coord, SlippyMapView view)
        {
            var pos = view.LatLngToView(coord);
            builder.Append(pos.X.ToString(_neutral));
            builder.Append(" ");
            builder.Append(pos.Y.ToString(_neutral));
        }


        public static IPreparedGeometry ToPrepared(this Geometry geom)
        {
	        return new PreparedGeometryFactory().Create(geom);
        }

		private static readonly IFormatProvider _neutral = CultureInfo.InvariantCulture;



        public static LineSegment CreateLineSegment(Vector2 v1, Vector2 v2) => new LineSegment(v1.X, v1.Y, v2.X, v2.Y);





		public static readonly PrecisionModel DefaultPrecisionModel = new PrecisionModel(1000);


		public static NtsGeometryServices GetNgGeometryServices()
		{
			var curGeometryService = NetTopologySuite.NtsGeometryServices.Instance;
			return new NetTopologySuite.NtsGeometryServices(
				curGeometryService.DefaultCoordinateSequenceFactory,
				NTSExtensions.DefaultPrecisionModel,
				curGeometryService.DefaultSRID,
				GeometryOverlay.NG, // RH: use 'Next Gen' overlay generator
				curGeometryService.CoordinateEqualityComparer);
		}

		public static Geometry CreateGeometry(this Rectangle<float> rect) => new NetTopologySuite.Geometries.Polygon(new LinearRing(new[]
		{
			rect.TopLeft().ToCoordinate(),
			rect.TopRight().ToCoordinate(),
			rect.BottomRight().ToCoordinate(),
			rect.BottomLeft().ToCoordinate(),
			rect.TopLeft().ToCoordinate()
		}), GetNgGeometryServices().CreateGeometryFactory());



		public static LineSegment GetClosestSegment(this Geometry geom, Coordinate c) => geom switch
		{
			MultiPolygon multi => GetClosestSegment(multi, c),
			NetTopologySuite.Geometries.Polygon poly => GetClosestSegment(poly, c),
			_ => throw new NotImplementedException()
		};

		public static LineSegment GetClosestSegment(this MultiPolygon multi, Coordinate c)
		{
			return 
				multi
					.Geometries
					.Select(g => g.GetClosestSegment(c))
					.MinBy(s => s.Distance(c));
		}



		public static LineSegment GetClosestSegment(this NetTopologySuite.Geometries.Polygon polygon, Coordinate c)
		{
			return
				polygon
					.Rings()
					.Select(r => r.GetClosestSegment(c))
					.MinBy(s => s.Distance(c));

		}

		public static LineSegment GetClosestSegment(this NetTopologySuite.Geometries.LineString lines, Coordinate c)
		{
			return
				lines
					.Segments()
					.MinBy(s => s.Distance(c));
		}

		public static IEnumerable<LineSegment> Segments(this LineString lines)
		{
			for (var i = 0; i < lines.Count - 1; i++)
			{
				yield return new LineSegment(lines[i], lines[i + 1]);
			}
		}

		public static IEnumerable<LineString> Rings(this NetTopologySuite.Geometries.Polygon polygon)
		{
			yield return polygon.ExteriorRing;
			foreach (var ring in polygon.InteriorRings)
			{
				yield return ring;
			}
		}


	}
}
