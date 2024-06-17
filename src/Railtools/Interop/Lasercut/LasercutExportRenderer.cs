using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Maps.Blazor.Vector;
using Ambacht.Common.Maps.Nts;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Buffer;
using Railtools.Geometry;
using Railtools.Tracks.Layouts;

namespace Railtools.Interop.Lasercut
{
	public class LasercutExportRenderer(Layout layout)
	{


		public Predicate<Section> IncludeRoadBed { get; set; } = s => s.Trajectories.Any(t => t.VerticalBounds().Max < 100);

		public FeatureCollection Render()
		{
			var result = new FeatureCollection();
			var basePlates = RenderRailBasePlates();
			result.Add(basePlates);
			foreach (var section in layout.Sections)
			{
				Render(result, section);
			}
			return result;
		}

		private IFeature RenderRailBasePlates()
		{
			var polygons = new List<NetTopologySuite.Geometries.Geometry>();
			foreach (var section in layout.Sections.Where(s => IncludeRoadBed(s)))
			{
				foreach (var trajectory in section.Trajectories)
				{
					var ring = (LinearRing) trajectory.Buffer(60);
					polygons.Add(new Polygon(ring).Buffer(1, EndCapStyle.Square));
				}
			}

			var union = polygons.RobustUnion();
			var result = new Feature()
			{
				Geometry = union
			};
			result.Stroke().Set("#00ff00");
			result.StrokeWidth().Set(0.5f);
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
			result.StrokeWidth().Set(0.1f);
			return result;
		}



	}
}
