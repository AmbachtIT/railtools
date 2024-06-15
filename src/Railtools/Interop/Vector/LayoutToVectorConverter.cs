using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using Railtools.Geometry;
using Railtools.Tracks.Layout;

namespace Railtools.Interop.Vector
{
	public class LayoutToVectorConverter(TrackLayout layout)
	{

		public FeatureCollection Convert()
		{
			var result = new FeatureCollection();
			foreach (var section in layout.Sections)
			{
				var geometry = CreateGeometry(section);
				if (geometry != null)
				{
					result.Add(new Feature(geometry, CreateAttributes(section)));
				}
			}

			return result;
		}

		private IAttributesTable CreateAttributes(Section section)
		{
			return new AttributesTable();
		}

		private NetTopologySuite.Geometries.Geometry? CreateGeometry(Section section) => section switch
		{
			SimpleSection simple => CreateGeometry(simple),
			_ => null
		};

		private NetTopologySuite.Geometries.Geometry? CreateGeometry(SimpleSection simple) => simple.Trajectory switch
		{
			Line line => CreateGeometry(simple, line),
			_ => null
		};

		private NetTopologySuite.Geometries.Geometry CreateGeometry(SimpleSection simple, Line line)
		{
			return line.Buffer(16.5f);
		}


	}
}
