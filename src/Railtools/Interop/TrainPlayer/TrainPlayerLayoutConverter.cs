using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Railtools.Geometry;
using Railtools.Tracks.Layout;
using Railtools.Tracks.Library;

namespace Railtools.Interop.TrainPlayer
{
	public class TrainPlayerLayoutConverter(TrainPlayerLayout layout)
	{

		private Dictionary<int, TrainPlayerEndpoint> _endpoints = layout.Endpoints.ToDictionary(e => e.Nr);


		public TrackLayout Convert()
		{
			var result = new TrackLayout();
			foreach (var part in layout.Parts)
			{
				var section = CreateSection(part);
				if (section != null)
				{
					result.Sections.Add(section);
				}
			}

			return result;
		}

		private Section? CreateSection(TrainPlayerPart part) => part.Type switch
		{
			"Curve" => CreateCurve(part),
			"Straight" => CreateStraight(part),
			_ => null
		};

		private Section CreateStraight(TrainPlayerPart part)
		{
			var from = _endpoints[part.EndpointNrs[0]];
			var to = _endpoints[part.EndpointNrs[1]];
			var trajectory = new Line(CoordsToVector3(from.Coord), CoordsToVector3(to.Coord));
			return new SimpleSection(trajectory);
		}

		private Section? CreateCurve(TrainPlayerPart part)
		{
			return null;
		}

		private Vector3 CoordsToVector3(string coords)
		{
			var parts = coords.Split(',');
			if (parts.Length != 3)
			{
				throw new InvalidOperationException();
			}

			var x = float.Parse(parts[0]);
			var y = float.Parse(parts[1]);
			var z = float.Parse(parts[2]);
			return new Vector3(
				(float)Math.Round(x * layout.ScaleX, 1), 
				(float)Math.Round(y * layout.ScaleY, 1),
				(float)Math.Round(z * 40.2204 * layout.ScaleX, 1));
		}



		private Vector2 CoordsToVector2(string coords)
		{
			var parts = coords.Split(',');
			if (parts.Length != 2)
			{
				throw new InvalidOperationException();
			}

			var x = float.Parse(parts[0]);
			var y = float.Parse(parts[1]);
			return new Vector2((float)Math.Round(x * layout.ScaleX, 1), (float)Math.Round(y * layout.ScaleY, 1));
		}
	}
}
