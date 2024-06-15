using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Maps.Nts;
using Ambacht.Common.Mathmatics;

namespace Railtools.Geometry
{
	public static class NtsExtensions
	{

		public static NetTopologySuite.Geometries.Geometry Buffer(this ITrajectory trajectory, float amount) =>
			trajectory switch
			{
				Line line => Buffer(line, amount),
				_ => throw new NotSupportedException()
			};

		public static NetTopologySuite.Geometries.LinearRing Buffer(this Line line, float amount)
		{
			var builder = new NtsBuilder();

			foreach (var extreme in BufferExtremesRing(amount))
			{
				builder.Add(line.GetPointWithDirection(extreme.X).Right(extreme.Y).Point.ToVector2());
			}

			return builder.CreateLinearRing();
		}


		private static IEnumerable<Vector2> BufferExtremesRing(float amount)
		{
			yield return new Vector2(0, amount);
			yield return new Vector2(1, amount);
			yield return new Vector2(1, -amount);
			yield return new Vector2(0, -amount);
		}

	}
}
