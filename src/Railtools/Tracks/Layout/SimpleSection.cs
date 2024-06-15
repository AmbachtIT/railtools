using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;
using Railtools.Geometry;
using Railtools.Tracks.Library;

namespace Railtools.Tracks.Layout
{

	/// <summary>
	/// A section containing two connectors. Used for straights, curves and flex track
	/// </summary>
	public record class SimpleSection : Section
	{

		public SimpleSection(ITrajectory trajectory)
		{
			this.Trajectory = trajectory;
		}

		public SectionType? Type { get; set; }

		public ITrajectory Trajectory { get; }

		public override Rectangle<float> Bounds()
		{
			return RectangleUtil.Cover(new[]
			{
				Trajectory.StartPosition().ToVector2(),
				Trajectory.EndPosition().ToVector2()
			});
		}
	}
}
