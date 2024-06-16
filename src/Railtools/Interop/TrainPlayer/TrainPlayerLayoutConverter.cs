﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;
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
				foreach (var section in CreateSections(part))
				{
					result.Sections.Add(section);
				}
			}

			return result;
		}

		private IEnumerable<Section> CreateSections(TrainPlayerPart part) => part.Type switch
		{
			"Curve" => new[] { CreateCurve(part) }, 
			"Straight" => new [] { CreateStraight(part) },
			_ => Array.Empty<Section>()
		};

		private Section CreateStraight(TrainPlayerPart part)
		{
			var trajectory = CreateLine(part);
			return new Section(trajectory);
		}

		private Section CreateCurve(TrainPlayerPart part)
		{
			return CreateCurve(part.Drawings.Cast<TrainPlayerArc>().Single());
		}

		private Section CreateCurve(TrainPlayerArc arc)
		{
			var start = CoordsToVector3(arc.Point1);
			var end = CoordsToVector3(arc.Point2);
			return new Section(CircularArc.Create(
				start,
				MathUtil.DegreesToRadiansF(90 - arc.Direction),
				end,
				ToMMX(arc.Radius),
				MathUtil.DegreesToRadiansF(arc.Angle)
			));
		}

		private string RandomColor()
		{
			return "#" + random.Next(16777210).ToString("X6");
		}

		private Random random = new Random(19681);

		private Line CreateLine(TrainPlayerPart part)
		{
			var from = _endpoints[part.EndpointNrs[0]];
			var to = _endpoints[part.EndpointNrs[1]];
			return new Line(CoordsToVector3(from.Coord), CoordsToVector3(to.Coord));
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
