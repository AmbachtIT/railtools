using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;
using NetTopologySuite.Geometries;

namespace Ambacht.Common.Maps.Nts
{
	public class NtsPolygonBuilder
	{

		private readonly List<Coordinate> _coordinates = new();

		public NetTopologySuite.Geometries.Polygon Build()
		{
			if (_coordinates.Count != _coordinates.Distinct().Count())
			{
				throw new InvalidOperationException("Invalid polygon");
			}

			var coords = new Coordinate[_coordinates.Count + 1];
			_coordinates.CopyTo(coords);
			coords[^1] = coords[0];
			var result = new NetTopologySuite.Geometries.Polygon(new LinearRing(coords), NTSExtensions.GetNgGeometryServices().CreateGeometryFactory());

			var overlayname = result.Factory.GeometryServices.GeometryOverlay.ToString().ToLower();
			if (overlayname.Contains("legacy"))
			{
				throw new InvalidOperationException();
			}
			return result;
		}

		public NtsPolygonBuilder Add(IEnumerable<Vector2<float>> coordinates)
		{
			_coordinates.AddRange(coordinates.Select(c => c.ToCoordinate()));
			return this;
		}



		public NtsPolygonBuilder Add(IEnumerable<Vector2> coordinates)
		{
			_coordinates.AddRange(coordinates.Select(c => c.ToCoordinate()));
			return this;
		}


	}
}
