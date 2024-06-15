using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;

namespace Railtools.Geometry
{
	public record struct PointWithDirection
	{
		public PointWithDirection(Vector3 point, float direction)
		{
			this.Point = point;
			this.Direction = direction;
		}


		public Vector3 Point { get;  }

		/// <summary>
		/// Angle of, in degrees. +X = 0, +Y = 0.5PI, -X=PI
		/// </summary>
		public float Direction { get; }


		/// <summary>
		/// Returns a point/direction pair oriented to the left relative to the current instance. Direction and height are unchanged
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		public PointWithDirection Left(float amount)
		{
			var delta = MathUtil.UnitFromAngle(Direction + MathF.PI / 2).ToVector3(0);
			return new(Point + delta * amount, Direction);
		}


		/// <summary>
		/// Returns a point/direction pair oriented to the right relative to the current instance. Direction and height are unchanged
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		public PointWithDirection Right(float amount) => Left(-amount);



	}
}
