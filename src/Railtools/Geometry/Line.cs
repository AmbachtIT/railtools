﻿using System;
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

		public float Length() => Vector3.Distance(Start, End);
		public float GetDirection(float t) => _direction;

		public Vector3 GetPoint(float t) => MathUtil.Lerp(Start, End, t);
	}
}