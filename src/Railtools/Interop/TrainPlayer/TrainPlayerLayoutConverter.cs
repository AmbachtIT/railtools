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
				new Section(part.Drawings.Select(p => CreateTrajectory(p)))
			};
		}

		private ITrajectory CreateTrajectory(TrainPlayerDrawing drawing) => drawing switch
		{
			TrainPlayerLine line => CreateLine(line),
			TrainPlayerArc arc => CreateArc(arc),
			_ => throw new NotImplementedException()
		};

		private Line CreateLine(TrainPlayerLine line)
		{
			return new Line(CoordsToVector3(line.Point1), CoordsToVector3(line.Point2));
		}

		private ITrajectory CreateArc(TrainPlayerArc arc)
		{
			var start = CoordsToVector3(arc.Point1);
			var end = CoordsToVector3(arc.Point2);
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



		private Vector3 CoordsToVector3(string coords)
		{
			var parts = coords.Split(',');
			if (parts.Length < 2 || parts.Length > 3)
			{
				throw new InvalidOperationException();
			}

			var x = float.Parse(parts[0]);
			var y = float.Parse(parts[1]);
			var z = parts.Length > 2 ? float.Parse(parts[2]) : 0f;
			return new Vector3(
				(float)Math.Round(ToMMX(x), 1), 
				(float)Math.Round(ToMMY(y), 1),
				(float)Math.Round(ToMMX(z * 4.02204f), 1));
		}

		private float ToMMX(float value) => value * layout.ScaleX * 10;
		private float ToMMY(float value) => value * layout.ScaleY * 10;


	}
}
