using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Ambacht.Common.Maps.Nts
{
	public abstract class NtsGeometryVisitor<T>
	{

		public T VisitGeometry(Geometry geometry) => geometry switch
		{
			MultiPolygon multi => VisitMulti(multi),
			GeometryCollection collection => VisitCollection(collection),
			NetTopologySuite.Geometries.Polygon polygon => VisitPolygon(polygon),
			Point point => VisitPoint(point),
			LinearRing linearRing => VisitLinearRing(linearRing),
			LineString lineString => VisitLineString(lineString),
			_ => throw new NotImplementedException()
		};

		protected virtual T VisitLineString(LineString lineString)
		{
			throw new NotImplementedException();
		}

		protected virtual T VisitLinearRing(LinearRing linearRing)
		{
			throw new NotImplementedException();
		}

		protected virtual T VisitPoint(Point point)
		{
			throw new NotImplementedException();
		}

		protected virtual T VisitPolygon(NetTopologySuite.Geometries.Polygon polygon)
		{
			throw new NotImplementedException();
		}

		protected virtual T VisitCollection(GeometryCollection collection)
		{
			throw new NotImplementedException();
		}

		protected virtual T VisitMulti(MultiPolygon multi)
		{
			throw new NotImplementedException();
		}
	}
}
