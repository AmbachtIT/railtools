using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railtools.Tracks.Library
{
	public record class StraightType : SegmentType
	{

		/// <summary>
		/// Length in mm
		/// </summary>
		public double Length { get; set; }

	}
}
