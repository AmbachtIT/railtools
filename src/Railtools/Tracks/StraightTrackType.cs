using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railtools.Tracks
{
	public record class StraightTrackType : TrackType
	{

		/// <summary>
		/// Length in mm
		/// </summary>
		public double Length { get; set; }

	}
}
