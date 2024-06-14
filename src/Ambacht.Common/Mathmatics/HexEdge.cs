using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Mathmatics
{
	public record struct HexEdge(HexCell Cell, HexEdgeEnum Edge)
	{


		public HexCell OppositeCell() => Edge switch
		{
			HexEdgeEnum.Top => Cell.CellAbove(),
			HexEdgeEnum.Bottom => Cell.CellBelow(),
			HexEdgeEnum.LeftBottom => Cell.CellLeftBottom(),
			HexEdgeEnum.RightBottom => Cell.CellRightBottom(),
			HexEdgeEnum.LeftTop => Cell.CellLeftTop(),
			HexEdgeEnum.RightTop => Cell.CellRightTop(),
			_ => throw new ArgumentOutOfRangeException()
		};

		public HexEdge Normalize() => Edge switch
		{
			HexEdgeEnum.Bottom => new(OppositeCell(), HexEdgeEnum.Top),
			HexEdgeEnum.RightTop => new(OppositeCell(), HexEdgeEnum.LeftBottom),
			HexEdgeEnum.RightBottom => new(OppositeCell(), HexEdgeEnum.LeftTop),
			_ => this
		};


		public IEnumerable<HexEdge> NeighbouringEdges()
		{
			foreach (var corner in Vertices())
			{
				foreach (var edge in corner.NeighbouringEdges())
				{
					if (!edge.Overlaps(this))
					{
						yield return edge;
					}
				}
			}
		}

		public IEnumerable<HexVertex> Vertices()
		{
			foreach (var corner in _cornerEnums[Edge])
			{
				yield return new HexVertex(Cell, corner);
			}
		}


		private static Dictionary<HexEdgeEnum, HexVertexEnum[]> _cornerEnums =
			new ()
			{
				{HexEdgeEnum.Bottom, new[] {HexVertexEnum.BottomLeft, HexVertexEnum.BottomRight }},
				{HexEdgeEnum.LeftBottom, new[] {HexVertexEnum.Left, HexVertexEnum.BottomLeft}},
				{HexEdgeEnum.LeftTop, new[] {HexVertexEnum.TopLeft, HexVertexEnum.Left }},
				{HexEdgeEnum.RightBottom, new[] {HexVertexEnum.BottomRight, HexVertexEnum.Right}},
				{HexEdgeEnum.RightTop, new[] {HexVertexEnum.Right, HexVertexEnum.TopRight}},
				{HexEdgeEnum.Top, new[] {HexVertexEnum.TopRight, HexVertexEnum.TopLeft}},
			};


		public bool Overlaps(HexEdge other) => Normalize().Equals(other.Normalize());

		public HexVertex? GetCommonVertex(HexEdge other)
		{
			foreach (var vertex1 in Vertices())
			{
				foreach (var vertex2 in other.Vertices())
				{
					if (vertex1.Overlaps(vertex2))
					{
						return vertex1;
					}
				}
			}

			return null;
		}

		public HexVertex GetOtherVertex(HexVertex vertex)
		{
			var candidates = Vertices().ToList();
			if (candidates[0].Overlaps(vertex))
			{
				return candidates[1];
			}
			if (candidates[1].Overlaps(vertex))
			{
				return candidates[0];
			}

			throw new InvalidOperationException();
		}
	}


	public enum HexEdgeEnum
	{
		Top, Bottom,
		LeftTop,
		LeftBottom,
		RightBottom,
		RightTop
	}

}
