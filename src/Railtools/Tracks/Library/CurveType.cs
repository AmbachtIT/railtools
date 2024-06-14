using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railtools.Tracks.Library
{
	public record class CurveType : SegmentType
	{

		/// <summary>
		/// Radius in mm
		/// </summary>
		public double Radius { get; set; }

		/// <summary>
		/// Angle in degrees
		/// </summary>
		public double Angle { get; set; }

	}
}
