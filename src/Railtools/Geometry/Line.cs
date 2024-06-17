using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;

namespace Railtools.Geometry
{
	public record struct Line : ITrajectory
	{
		public Line(Vector3 start, Vector3 end)
		{
			this.Start = start;
			this.End = end;

			var delta = (end - start).ToVector2();
			if (delta.LengthSquared() == 0)
			{
				_direction = 0;
			}
			else
			{
				_direction = delta.Angle();
			}
		}

		private float _direction;

		public Vector3 Start { get; }

		public Vector3 End { get; }

		public Vector3 StartPosition() => Start;

		public Vector3 EndPosition() => End;

		public float Length() => Vector2.Distance(Start.ToVector2(), End.ToVector2());
		public float GetDirection(float t) => _direction;

		public Vector3 GetPoint(float t) => MathUtil.Lerp(Start, End, t);
		public float Project(Vector3 position)
		{
			throw new NotImplementedException();
		}

		public Rectangle<float> Bounds() => RectangleUtil.Cover(new[]
		{
			Start, End
		}.Select(s => s.ToVector2()));

		public Range<float> VerticalBounds() => new Range<float>(Start.Z, End.Z);
	}
}
