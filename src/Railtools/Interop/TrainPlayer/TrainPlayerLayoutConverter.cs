using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;
using Railtools.Geometry;
using Railtools.Tracks.Layouts;
using Railtools.Tracks.Library;

namespace Railtools.Interop.TrainPlayer
{
	public class TrainPlayerLayoutConverter(TrainPlayerLayout layout)
	{

		private readonly Dictionary<int, TrainPlayerEndpoint> _endpoints = layout.Endpoints.ToDictionary(e => e.Nr);


		public Layout Convert()
		{
			var result = new Layout();
			foreach (var part in layout.Parts)
			{
				foreach (var section in CreateSections(part))
				{
					result.Sections.Add(section);
				}
			}

			return result;
		}

		private IEnumerable<Section> CreateSections(TrainPlayerPart part)
		{
			return new[]
			{
				new Section(part.Drawings.Select(drawing => CreateTrajectory(part, drawing)))
			};
		}

		private ITrajectory CreateTrajectory(TrainPlayerPart part, TrainPlayerDrawing drawing) => drawing switch
		{
			TrainPlayerLine line => CreateLine(part, line),
			TrainPlayerArc arc => CreateArc(part, arc),
			_ => throw new NotImplementedException()
		};

		private Line CreateLine(TrainPlayerPart part, TrainPlayerLine line)
		{
			return new Line(CoordsToVector3(part, line.Point1), CoordsToVector3(part, line.Point2));
		}

		private ITrajectory CreateArc(TrainPlayerPart part, TrainPlayerArc arc)
		{
			var start = CoordsToVector3(part, arc.Point1);
			var end = CoordsToVector3(part, arc.Point2);
			return CircularArc.Create(
				start,
				MathUtil.DegreesToRadiansF(90 - arc.Direction),
				end,
				ToMMX(arc.Radius),
				MathUtil.DegreesToRadiansF(arc.Angle)
			);
		}



		private string RandomColor()
		{
			return "#" + random.Next(16777210).ToString("X6");
		}

		private Random random = new Random(19681);


		private Vector3 CoordsToVector3(TrainPlayerPart part, string coords)
		{
			var result = CoordsToVector3(coords);
			return result with
			{
				Z = GetNearestHeight(part, result.X, result.Y)
			};
		}


		private Vector3 CoordsToVector3(string coords)
		{
			var parts = coords.Split(',');
			if (parts.Length < 2 || parts.Length > 3)
			{
				throw new InvalidOperationException();
			}

			var x = float.Parse(parts[0]);
			var y = float.Parse(parts[1]);
			var z = parts.Length > 2 ? float.Parse(parts[2]) : 0;
			return new Vector3(
				(float)Math.Round(ToMMX(x), 1), 
				(float)Math.Round(ToMMY(y), 1),
				(float)Math.Round(ToMMX(z * 4.02204f), 1));
		}

		private float GetNearestHeight(TrainPlayerPart part, float x, float y)
		{
			return part
				.EndpointNrs
				.Select(nr => CoordsToVector3(_endpoints[nr].Coord))
				.MinBy(v => Vector2.Distance(v.ToVector2(), new(x, y)))
				.Z;
		}

		private float ToMMX(float value) => value * layout.ScaleX * 10;
		private float ToMMY(float value) => value * layout.ScaleY * 10;


	}
}
