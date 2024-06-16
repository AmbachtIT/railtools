﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;
using Railtools.Geometry;
using Railtools.Tracks.Library;

namespace Railtools.Tracks.Layout
{
	public record class Section
	{
		public Section(ITrajectory trajectory)
		{
			this.Trajectory = trajectory;
		}


		public string Fill { get; set; } = "#ff0000";

		/// <summary>
		/// Gauge in mm
		/// </summary>
		public float Gauge { get; set; } = 16.5f;


		public SectionType? Type { get; set; }

		public ITrajectory Trajectory { get; }

		public Rectangle<float> Bounds() => Trajectory.Bounds();
	}
}
