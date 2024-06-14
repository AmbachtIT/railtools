using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ambacht.Common.Mathmatics
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks><see cref="HexMath">HexMath.cs</see> for a description of the hexagonal coordinate system</remarks>
	/// <param name="X"></param>
	/// <param name="Y"></param>
	public struct HexCell
	{
		public HexCell(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public int X { get; }

		public int Y { get; }

		public HexCell CellAbove() => new(X, Y + 1);
		public HexCell CellBelow() => new(X, Y - 1);

		public HexCell CellLeftTop() => IsEvenColumn 
			? new(X - 1, Y)
			: new(X - 1, Y + 1);

		public HexCell CellLeftBottom() => IsEvenColumn
			? new(X - 1, Y - 1)
			: new(X - 1, Y);

		public HexCell CellRightTop() => IsEvenColumn
			? new(X + 1, Y)
			: new(X + 1, Y + 1);

		public HexCell CellRightBottom() => IsEvenColumn
			? new(X + 1, Y - 1)
			: new(X + 1, Y);


		[JsonIgnore()]
		public bool IsEvenColumn => MathUtil.Modulo(X, 2) == 0;



		public IEnumerable<HexCell> Neighbours()
		{
			yield return CellAbove();
			yield return CellLeftTop();
			yield return CellLeftBottom();
			yield return CellBelow();
			yield return CellRightBottom();
			yield return CellRightTop();
		}

		public HexEdge GetEdge(HexEdgeEnum edgeEnum) => new(this, edgeEnum);



		/// <summary>
		/// Yields all edges in counterclockwise fashion, starting with the top
		/// </summary>
		/// <returns></returns>
		public IEnumerable<HexEdge> Edges()
		{
			yield return new (this, HexEdgeEnum.Top);
			yield return new (this, HexEdgeEnum.LeftTop);
			yield return new (this, HexEdgeEnum.LeftBottom);
			yield return new (this, HexEdgeEnum.Bottom);
			yield return new (this, HexEdgeEnum.RightBottom);
			yield return new (this, HexEdgeEnum.RightTop);
		}


		public IEnumerable<HexVertex> Vertices()
		{
			yield return new (this, HexVertexEnum.Left);
			yield return new (this, HexVertexEnum.BottomLeft);
			yield return new (this, HexVertexEnum.BottomRight);
			yield return new (this, HexVertexEnum.Right);
			yield return new (this, HexVertexEnum.TopRight);
			yield return new (this, HexVertexEnum.TopLeft);
		}


		public static HexCell ById(string id)
		{
			var parts = id.Split(',');
			if (parts.Length != 2)
			{
				throw new ArgumentException();
			}

			return new(int.Parse(parts[0]), int.Parse(parts[1]));
		}

		[JsonIgnore()]
		public string Id => $"{X},{Y}";

		[JsonIgnore()]
		public Vector2 Position => HexMath.GetCenter(X, Y);

		public override string ToString() => Id;


		public static readonly HexCell Zero = new HexCell(0, 0);


		public static HexCell FromCenter(Vector2 center)
		{
			var (x, y) = HexMath.GetCoordinates(center);
			return new HexCell(x, y);
		}
	}
}
