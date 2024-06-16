using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;

namespace Railtools.Tracks.Layout
{
	public abstract record class Section
	{
		public string Fill { get; set; } = "#ff0000";

		/// <summary>
		/// Gauge in mm
		/// </summary>
		public float Gauge { get; set; } = 16.5f;

		public abstract Rectangle<float> Bounds();
	}
}
