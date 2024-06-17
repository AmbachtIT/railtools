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

		public static NetTopologySuite.Geometries.Geometry Buffer(this ITrajectory trajectory, float amount) => trajectory.Buffer(amount, -amount);

		public static NetTopologySuite.Geometries.Geometry Buffer(this ITrajectory trajectory, float amount1, float amount2) =>
			trajectory switch
			{
				Line line => Buffer(line, amount1, amount2, 1),
				CircularArc arc => Buffer(arc, amount1, amount2, (int)Math.Max(arc.Length() / 4f, 2)),
				_ => throw new NotSupportedException()
			};

		private static NetTopologySuite.Geometries.LinearRing Buffer(this ITrajectory trajectory, float amount1, float amount2, int steps)
		{
			var builder = new NtsBuilder();

			foreach (var extreme in BufferExtremesRing(amount1, amount2, steps))
			{
				builder.Add(trajectory.GetPointWithDirection(extreme.X).Right(extreme.Y).Point.ToVector2());
			}

			return builder.CreateLinearRing();
		}


		private static IEnumerable<Vector2> BufferExtremesRing(float amount1, float amount2, int steps)
		{
			var stepSize = 1.0f / steps;
			for (var i = 0; i<=steps; i++)
			{
				var alpha = (float)i / steps;
				yield return new Vector2(alpha, amount1);
			}
			for (var i = steps; i >= 0; i--)
			{
				var alpha = (float)i / steps;
				yield return new Vector2(alpha, amount2);
			}
		}

	}
}
