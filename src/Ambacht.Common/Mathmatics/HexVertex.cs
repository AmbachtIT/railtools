using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Mathmatics
{
	public record struct HexVertex(HexCell Cell, HexVertexEnum Vertex)
	{

		public HexVertex Normalize() => Vertex switch
		{
			HexVertexEnum.TopLeft => new HexVertex(Cell.CellLeftTop(), HexVertexEnum.Right),
			HexVertexEnum.TopRight => new HexVertex(Cell.CellRightTop(), HexVertexEnum.Left),
			HexVertexEnum.BottomLeft => new HexVertex(Cell.CellLeftBottom(), HexVertexEnum.Right),
			HexVertexEnum.BottomRight => new HexVertex(Cell.CellRightBottom(), HexVertexEnum.Left),
			_ => this
		};


		public IEnumerable<HexEdge> NeighbouringEdges() =>
			Vertex switch
			{
				HexVertexEnum.Left => NeighbouringEdgesLeft(),
				HexVertexEnum.Right => NeighbouringEdgesRight(),
				_ => Normalize().NeighbouringEdges()
			};


		private IEnumerable<HexEdge> NeighbouringEdgesLeft()
		{
			yield return new HexEdge(Cell, HexEdgeEnum.LeftBottom);
			yield return new HexEdge(Cell, HexEdgeEnum.LeftTop);
			yield return new HexEdge(Cell.CellLeftTop(), HexEdgeEnum.Bottom);
		}

		private IEnumerable<HexEdge> NeighbouringEdgesRight()
		{
			yield return new HexEdge(Cell, HexEdgeEnum.RightBottom);
			yield return new HexEdge(Cell, HexEdgeEnum.RightTop);
			yield return new HexEdge(Cell.CellRightTop(), HexEdgeEnum.Bottom);
		}


		public bool Overlaps(HexVertex other) => Normalize().Equals(other.Normalize());

		public Vector2 Position => Cell.Position + _offsets[Vertex];


		private static Dictionary<HexVertexEnum, Vector2> _offsets = new Dictionary<HexVertexEnum, Vector2>()
		{
			{ HexVertexEnum.Left, new(-HexMath.Width / 2f, 0) },
			{ HexVertexEnum.TopLeft, new(-HexMath.Width/ 4f, HexMath.Height / 2f) },
			{ HexVertexEnum.TopRight, new(HexMath.Width/ 4f, HexMath.Height / 2f) },
			{ HexVertexEnum.Right, new(HexMath.Width / 2f, 0) },
			{ HexVertexEnum.BottomLeft, new(-HexMath.Width/ 4f, -HexMath.Height / 2f) },
			{ HexVertexEnum.BottomRight, new(HexMath.Width/ 4f, -HexMath.Height / 2f) },
		};

	}

	public enum HexVertexEnum
	{
		Left,
		Right,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}
}
