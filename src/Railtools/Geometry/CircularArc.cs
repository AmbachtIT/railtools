using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;


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
		/// End angle in radians
		/// </summary>
		public float EndHeight { get; }


		public float AngleT(float angle) => MathUtil.ReverseLerp(StartAngle, EndAngle, angle);

		public Vector2 AnglePosition2(float angle) => (MathUtil.UnitFromAngle(angle) * Radius) + Center;

		public float AngleHeight(float angle) => MathUtil.Lerp(StartHeight, EndHeight, AngleT(angle));

		public Vector3 AnglePosition(float angle) => AnglePosition2(angle).ToVector3(AngleHeight(angle));

		public Vector3 StartPosition() => AnglePosition(StartAngle);

		public Vector3 EndPosition() => AnglePosition(EndAngle);

		public float Length() => Math.Abs(StartAngle - EndAngle) * Radius;
		public float GetDirection(float t) => MathUtil.Lerp(StartAngle, EndAngle, t) - MathF.PI / 2f;

		public Vector3 GetPoint(float t) => 
			(MathUtil.UnitFromAngle(MathUtil.Lerp(StartAngle, EndAngle, t)) * Radius + Center)
			.ToVector3(MathUtil.Lerp(StartHeight, EndHeight, t));
	}
}
