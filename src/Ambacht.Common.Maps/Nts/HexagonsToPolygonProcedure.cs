using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;
using NetTopologySuite.Geometries;

namespace Ambacht.Common.Maps.Nts
{

	/// <summary>
	/// This class assumes all hex cells are connected!
	/// </summary>
	public class HexagonsToPolygonProcedure
	{


		
		public HexagonsToPolygonProcedure(IEnumerable<HexCell> cells)
		{
			_cells = cells.ToHashSet();
		}


		private readonly HashSet<HexCell> _cells;
		private HashSet<HexEdge> _fringe = new HashSet<HexEdge>();


		public Vector2 Center { get; set; } = Vector2.Zero;

		public float Scale { get; set; } = 1;



		/// <summary>
		/// Finds all hex corners that are on the outside of the area. All these edges will need to end up on a polygon boundary
		/// </summary>
		private HashSet<HexEdge> CreateFringe()
		{
			var result = new HashSet<HexEdge>();
			foreach (var cell in _cells)
			{
				foreach (var edge in cell.Edges())
				{
					if (!_cells.Contains(edge.OppositeCell()))
					{
						result.Add(edge.Normalize());
					}
				}
			}
			return result;
		}


		public Geometry CreateGeometry()
		{
			var boundaries =
				GetBoundaryEdges()
					.Select(b => HexUtil.GetVertices(b).Select(v => v.Normalize()).ToList())
					.Select(CreateLinearRing)
					.OrderByDescending(l => new NetTopologySuite.Geometries.Polygon(l).Area)
					.ToList();

			var outer = boundaries.First();
			var inner = boundaries.Skip(1).ToArray();

			return new NetTopologySuite.Geometries.Polygon(outer, inner, _geometryFactory);
		}

		private LinearRing CreateLinearRing(List<HexVertex> vertices)
		{
			var result = new LinearRing(vertices.Select(v => (v.Position * Scale + Center).ToCoordinate()).ToArray());
			return result;
		}


		public IEnumerable<List<HexEdge>> GetBoundaryEdges()
		{
			_fringe = CreateFringe();
			while (_fringe.Count > 0)
			{
				var poly = new List<HexEdge>();
				var first = _fringe.First();
				var edge = first;
				do
				{
					_fringe.Remove(edge);
					poly.Add(edge);


					var next = 
						edge
						.NeighbouringEdges()
						.Select(e => e.Normalize())
						.Where(_fringe.Contains)
						.Select(e => (HexEdge?)e)
						.FirstOrDefault();

					if (next == null)
					{
						break;
					}

					edge = next.Value;

				} while (_fringe.Count > 0);

				yield return poly;
			}
		}

		private readonly GeometryFactory _geometryFactory = NTSExtensions.GetNgGeometryServices().CreateGeometryFactory();

	}
}
