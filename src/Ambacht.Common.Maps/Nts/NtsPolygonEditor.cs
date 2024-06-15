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

	public abstract class NtsGeometryEditor
	{

		public static NtsGeometryEditor Create(Geometry geom) => geom switch
		{
			MultiPolygon multi => NtsMultiPolygonEditor.Create(multi),
			NetTopologySuite.Geometries.Polygon poly => NtsPolygonEditor.Create(poly),
			_ => throw new NotImplementedException()
		};
		

		public abstract Geometry CreateGeometry();

		public abstract void Translate(Vector2 delta);
	}


	public class NtsMultiPolygonEditor : NtsGeometryEditor
	{

		public static NtsMultiPolygonEditor Create(MultiPolygon multi)
		{
			return new NtsMultiPolygonEditor()
			{
				Polygons =
					multi.Geometries.Cast<NetTopologySuite.Geometries.Polygon>()
						.Select(p => NtsPolygonEditor.Create(p))
						.ToList()
			};
		}

		public List<NtsPolygonEditor> Polygons { get; set; } = new();

		public override Geometry CreateGeometry()
		{
			return 
				new 
					MultiPolygon(Polygons.Select(p => p.CreateGeometry()).Cast<NetTopologySuite.Geometries.Polygon>()
				.ToArray());

		}

		public override void Translate(Vector2 delta)
		{
			foreach (var polygon in Polygons)
			{
				polygon.Translate(delta);
			}

		}
	}

	

	public class NtsPolygonEditor : NtsGeometryEditor
	{

		public static NtsPolygonEditor Create(NetTopologySuite.Geometries.Polygon poly)
		{
			return new NtsPolygonEditor()
			{
				Shell = NtsLinearRingEditor.Create(poly.Shell),
				Holes = poly.Holes.Select(NtsLinearRingEditor.Create).ToList()
			};
		}

		public NtsLinearRingEditor Shell { get; set; } = new NtsLinearRingEditor();

		public List<NtsLinearRingEditor> Holes { get; set; } = new List<NtsLinearRingEditor>();

		public override Geometry CreateGeometry()
		{
			return new NetTopologySuite.Geometries.Polygon(
				Shell.CreateLinearRing(),
				Holes.Select(h => h.CreateLinearRing()).ToArray()
			);
		}

		public override void Translate(Vector2 delta)
		{
			Shell.Translate(delta);
			foreach (var hole in Holes)
			{
				hole.Translate(delta);
			}
		}
	}

	public class NtsLinearRingEditor : NtsGeometryEditor
	{

		public static NtsLinearRingEditor Create(LinearRing ring) => Create(ring.Coordinates);

		private static NtsLinearRingEditor Create(Coordinate[] coordinates) => new NtsLinearRingEditor()
		{
			Coordinates = coordinates.Take(coordinates.Length - 1).ToList()
		};

		public List<Coordinate> Coordinates { get; set; } = new List<Coordinate>();

		public LinearRing CreateLinearRing() => new LinearRing(Coordinates.Concat(Coordinates.Take(1)).ToArray());


		public override Geometry CreateGeometry()
		{
			throw new NotImplementedException();
		}

		public override void Translate(Vector2 delta)
		{
			var dbl = delta.ToVector2D();
			for (var i = 0; i < Coordinates.Count; i++)
			{
				Coordinates[i] = (Coordinates[i].ToVector2() + dbl).ToCoordinate();
			}
			
		}
	}

}
