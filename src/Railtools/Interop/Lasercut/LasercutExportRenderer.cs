using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Maps.Blazor.Vector;
using NetTopologySuite.Features;
using Railtools.Geometry;
using Railtools.Tracks.Layouts;

namespace Railtools.Interop.Lasercut
{
	public class LasercutExportRenderer(Layout layout)
	{

		public FeatureCollection Render()
		{
			var result = new FeatureCollection();
			foreach (var section in layout.Sections)
			{
				Render(result, section);
			}
			return result;
		}


		private void Render(FeatureCollection result, Section section)
		{
			result.AddRange(RenderRoadBed(section));
			result.AddRange(RenderRails(section));
		}

		private IEnumerable<IFeature> RenderRoadBed(Section section)
		{
			foreach (var trajectory in section.Trajectories)
			{
				yield return CreateBufferFeature(trajectory, -20, 20, "#ff0000");
			}
		}

		private IEnumerable<IFeature> RenderRails(Section section)
		{
			foreach (var trajectory in section.Trajectories)
			{
				var r1 = 16.5f / 2;
				yield return CreateBufferFeature(trajectory, r1 - 0.4f, r1 + 0.4f, "#ff0000");
				yield return CreateBufferFeature(trajectory, -r1 - 0.4f, -r1 + 0.4f, "#ff0000");

			}
		}


		private IFeature CreateBufferFeature(ITrajectory trajectory, float amount1, float amount2, string stroke)
		{
			var geometry = trajectory.Buffer(amount1, amount2);
			var result = new Feature()
			{
				Geometry = geometry
			};
			result.Stroke().Set(stroke);
			return result;
		}



	}
}
