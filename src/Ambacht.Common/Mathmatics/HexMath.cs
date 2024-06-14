using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Mathmatics
{

	public static class HexMath
	{
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// https://www.redblobgames.com/grids/hexagons/
		/// Orientation: Flat-top
		/// - Size = length of side, distance between hexagon center and vertices = 1
		/// - W = 2 * Size
		/// - H = SQRT(3) * Size
		/// - Horizontal spacing = 1.5 * Size = 1.5
		/// - Vertical spacing = SQRT(3)
		/// 
		///      ___
		///  ___/1,2\___/
		/// /0,2\___/2,2\
		/// \___/1,1\___/
		/// /0,1\___/2,1\
		/// \___/1,0\___/
		/// /   \___/
		/// </remarks>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Vector2 GetCenter(int x, float y)
		{

			if (MathUtil.Modulo(x, 2) != 0)
			{
				y += .5f;
			}

			return new Vector2(x * 1.5f, y * Sqrt3);
		}


		public static (int, int) GetCoordinates(Vector2 center)
		{
			var x = (int) Math.Round(center.X / 1.5f);
			var y = center.Y / Sqrt3;
			if (MathUtil.Modulo(x, 2) != 0)
			{
				y -= .5f;
			}

			return (x, (int)Math.Round(y));
		}


		public static readonly float Sqrt3 = MathF.Sqrt(3);
		public static readonly float Size = 1f;
		public static readonly float Width = Size * 2f;
		public static readonly float Height = Size * Sqrt3;
		public static readonly float HorizontalSpacing = Size * 1.5f;
		public static readonly float VerticalSpacing = Size * Sqrt3;
	}
}
