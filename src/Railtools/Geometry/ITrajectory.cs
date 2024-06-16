using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;

namespace Railtools.Geometry
{
	public interface ITrajectory
	{

		Vector3 StartPosition();
		Vector3 EndPosition();
		float Length();
		float GetDirection(float t);

		Vector3 GetPoint(float t);

		/// <summary>
		/// Returns the t closest to the specified position
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		float Project(Vector3 position);

		Rectangle<float> Bounds();


	}

	public static class TrajectoryExtensions
	{
		public static PointWithDirection GetPointWithDirection(this ITrajectory trajectory, float t) =>
			new PointWithDirection(
				trajectory.GetPoint(t),
				trajectory.GetDirection(t));
	}
}
