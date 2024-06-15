using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;

namespace Railtools.Tracks.Layout
{
	public class TrackLayout
	{

		public List<Section> Sections { get; set; } = new List<Section>();


		public Rectangle<float> Bounds() => RectangleUtil.Cover(Sections.Select(s => s.Bounds()));
	}
}
