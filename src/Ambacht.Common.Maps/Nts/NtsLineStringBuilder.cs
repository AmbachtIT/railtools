using Ambacht.Common.Mathmatics;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries.Implementation;

namespace Ambacht.Common.Maps.Nts
{
	public class NtsBuilder
	{

		private readonly List<Coordinate> _coordinates = new();

		public LineString CreateLineString()
		{
			if (_coordinates.Count != _coordinates.Distinct().Count())
			{
				throw new InvalidOperationException("Invalid polygon");
			}

			var coords = new Coordinate[_coordinates.Count];
			_coordinates.CopyTo(coords);

			var result = new LineString(new CoordinateArraySequence(coords), NTSExtensions.GetNgGeometryServices().CreateGeometryFactory());
			return result;
		}

		public LinearRing CreateLinearRing()
		{
			if (_coordinates.Count != _coordinates.Distinct().Count())
			{
				throw new InvalidOperationException("Invalid polygon");
			}

			var coords = new Coordinate[_coordinates.Count + 1];
			_coordinates.CopyTo(coords);
			coords[^1] = coords[0];

			var result = new LinearRing(new CoordinateArraySequence(coords), NTSExtensions.GetNgGeometryServices().CreateGeometryFactory());
			return result;
		}



		public NtsBuilder Add(Vector2 coordinate)
		{
			_coordinates.Add(coordinate.ToCoordinate());
			return this;
		}

		public NtsBuilder Add(IEnumerable<Vector2<float>> coordinates)
		{
			_coordinates.AddRange(coordinates.Select(c => c.ToCoordinate()));
			return this;
		}

	}
}
