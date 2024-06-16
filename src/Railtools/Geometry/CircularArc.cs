using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;
using Railtools.Interop.TrainPlayer;


namespace Railtools.Geometry
{
	public record struct CircularArc : ITrajectory
	{

		public CircularArc(Vector2 center, float radius, float startAngle, float endAngle, float startHeight = 0, float endHeight = 0)
		{
			if (Math.Abs(startAngle - endAngle) >= MathF.PI)
			{
				throw new ArgumentException("Arc angle must be less than 180 degrees");
			}
			this.Center = center;
			this.Radius = radius;
			this.StartAngle = startAngle;
			this.EndAngle = endAngle;
			this.StartHeight = startHeight;
			this.EndHeight = endHeight;
		}

		public Vector2 Center { get; }

		public float Radius { get; }

		/// <summary>
		/// Start angle in radians
		/// </summary>
		/// <remarks>
		/// If StartAngle &lt; EndAngle, this arc turns counterclockwise
		/// </remarks>
		public float StartAngle { get; }

		/// <summary>
		/// Start height
		/// </summary>
		public float StartHeight { get; }


		/// <summary>
		/// End angle in radians
		/// </summary>
		public float EndAngle { get; }

		/// <summary>
		/// End height
		/// </summary>
		public float EndHeight { get; }


		public float AngleT(float angle) => MathUtil.ReverseLerp(StartAngle, EndAngle, angle);

		public Vector2 AnglePosition2(float angle) => (MathUtil.UnitFromAngle(angle) * Radius) + Center;

		public float AngleHeight(float angle) => MathUtil.Lerp(StartHeight, EndHeight, AngleT(angle));

		public Vector3 AnglePosition(float angle) => AnglePosition2(angle).ToVector3(AngleHeight(angle));

		public Vector3 StartPosition() => AnglePosition(StartAngle);

		public Vector3 EndPosition() => AnglePosition(EndAngle);

		public float Length() => Math.Abs(EndAngle - StartAngle) * Radius;
		public float GetDirection(float t) => MathUtil.Lerp(StartAngle, EndAngle, t) - MathF.PI / 2f;

		public Vector3 GetPoint(float t) => 
			(MathUtil.UnitFromAngle(MathUtil.Lerp(StartAngle, EndAngle, t)) * Radius + Center)
			.ToVector3(MathUtil.Lerp(StartHeight, EndHeight, t));

		public float Project(Vector3 position)
		{
			var delta = position.ToVector2() - this.Center;
			var angle = delta.Angle();
			return MathUtil.ReverseLerp(StartAngle, EndAngle, angle);
		}


		public Rectangle<float> Bounds() => RectangleUtil.Cover(new[]
		{
			StartPosition(), EndPosition()
		}.Select(s => s.ToVector2()));


		public static CircularArc Create(Vector3 start, float startAngle, Vector3 end, float radius, float angle)
		{
			var startDirection = MathUtil.UnitFromAngle(startAngle);
			return CreateCandidates(start, end, radius, angle)
				.MaxBy(arc =>
				{
 					var t = arc.Project(start);
					var actualDirection = MathUtil.UnitFromAngle(arc.GetPointWithDirection(t).Direction);
					return Math.Abs(Vector2.Dot(startDirection, actualDirection));
				})!;
		}

		private static IEnumerable<CircularArc> CreateCandidates(Vector3 start, Vector3 end, float radius, float angle)
		{
			var line = new Line(start, end);

			var d = line.Length() / 2f;
			var l = MathF.Sqrt(radius * radius - d * d);


			foreach (var dir in new[] { 1, -1 })
			{
				var pi = dir * MathF.PI / 2;


				var c1 = line.GetPointWithDirection(0).Right(l * dir).Forward(d);
				var halfAngle = angle / 2;

				var a1 = c1.Direction + pi - halfAngle;
				var a2 = c1.Direction + pi + halfAngle;

				yield return new CircularArc(c1.Point.ToVector2(), radius, a1, a2, line.Start.Z, line.End.Z);
			}

		}

	}
}
