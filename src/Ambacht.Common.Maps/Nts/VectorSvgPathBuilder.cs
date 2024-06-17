using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Maps.Projections;
using NetTopologySuite.Geometries;

namespace Ambacht.Common.Maps.Nts
{
	public class VectorSvgPathBuilder(VectorMapView view) : NtsGeometryVisitor<string>
	{
		protected override string VisitLinearRing(LinearRing linearRing) => VisitLineString(linearRing);

		protected override string VisitCollection(GeometryCollection collection)
		{
			return string.Join(" ", collection.Select(VisitGeometry));
		}

		protected override string VisitMulti(MultiPolygon multi) => VisitCollection(multi);

		protected override string VisitPoint(Point point)
		{
			var builder = new StringBuilder();
			builder.Append("M");
			builder.WriteSvgCoordinate(view.WorldToScreen(point.Coordinate));
			return builder.ToString();
		}

		protected override string VisitPolygon(NetTopologySuite.Geometries.Polygon polygon)
		{
			var builder = new StringBuilder();
			builder.Append(VisitLineString(polygon.ExteriorRing));
			foreach (var inner in polygon.InteriorRings)
			{
				builder.Append(" ");
				builder.Append(VisitLineString(inner));
			}
			return builder.ToString();
		}

		protected override string VisitLineString(LineString lineString) => GetSvgPath(lineString.Coordinates);


		private string GetSvgPath(Coordinate[] coords)
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
				builder.WriteSvgCoordinate(view.WorldToScreen(coord));
			}

			return builder.ToString();
		}


	}
}
