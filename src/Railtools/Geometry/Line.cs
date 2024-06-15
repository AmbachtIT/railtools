using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Railtools.Geometry
{
	public record struct Line : ITrajectory
	{
		public Line(Vector3 start, Vector3 end)
		{
			this.Start = start;
			this.End = end;
		}

		public Vector3 Start { get; }

		public Vector3 End { get; }

		public Vector3 StartPosition() => Start;

		public Vector3 EndPosition() => End;

		public float Length() => Vector3.Distance(Start, End);
	}
}
