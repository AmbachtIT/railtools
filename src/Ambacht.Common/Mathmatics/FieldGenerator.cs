using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Mathmatics
{
	public abstract class FieldGenerator
	{

		public float Distance { get; set; }

		public float Size { get; set; }

		public abstract IEnumerable<Vector2> GetPoints();

	}


	public class GridFieldGenerator : FieldGenerator
	{

		public override IEnumerable<Vector2> GetPoints()
		{
			for (var y = -Size; y <= Size; y += Distance)
			{
				for (var x = -Size; x <= Size; x += Distance)
				{
					yield return new Vector2(x, y);
				}
			}
		}
	}

	public class HexFieldGenerator : FieldGenerator
	{

		public override IEnumerable<Vector2> GetPoints()
		{
			var count = (int)Math.Ceiling(Size / Distance);
			for (var y = -count; y <= count; y++)
			{
				for (var x = -count; x <= count; x++)
				{
					yield return Distance * HexMath.GetCenter(x, y);
				}

			}
		}
	}

}
