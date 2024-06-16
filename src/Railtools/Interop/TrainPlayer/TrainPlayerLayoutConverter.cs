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
			var arc = part.Drawings.Cast<TrainPlayerArc>().Single();
			var startPoint = _endpoints[part.EndpointNrs[0]];
			var endPoint = _endpoints[part.EndpointNrs[1]];
			return new Section(CircularArc.Create(
				CoordsToVector3(startPoint.Coord),
				MathUtil.DegreesToRadiansF(90 - startPoint.Direction),
				CoordsToVector3(endPoint.Coord),
				ToMMX(arc.Radius),
				MathUtil.DegreesToRadiansF(arc.Angle)
			));
		}


		private Section CreateCurve2(TrainPlayerPart part)
		{
			var arc = part.Drawings.Cast<TrainPlayerArc>().Single();
			var start = _endpoints[part.EndpointNrs[0]];
			var startPosition = CoordsToVector3(start.Coord);
			var startDirection = MathUtil.UnitFromAngle(MathUtil.DegreesToRadiansF(90 - start.Direction));
			return CreateCurves(part, arc)
				.MaxBy(s =>
				{
					var trajectory = s.Trajectories[0];
					var t = trajectory.Project(startPosition);
					var actualDirection = MathUtil.UnitFromAngle(trajectory.GetPointWithDirection(t).Direction);
					return Math.Abs(Vector2.Dot(startDirection, actualDirection));
				})!;
		}

		private IEnumerable<Section> CreateDebugCurves(TrainPlayerPart part)
		{
			var arc = part.Drawings.Cast<TrainPlayerArc>().Single();
			return CreateCurves(part, arc);
		}

		private IEnumerable<Section> CreateCurves(TrainPlayerPart part, TrainPlayerArc arc)
		{
			var line = CreateLine(part);

			var r = ToMMX(arc.Radius);
			var d = line.Length() / 2f;
			var l = MathF.Sqrt(r * r - d * d);


			foreach (var dir in new[] { 1, -1 })
			{
				var pi = dir * MathF.PI / 2;


				var c1 = line.GetPointWithDirection(0).Right(l * dir).Forward(d);
				var halfAngle = (float)MathUtil.DegreesToRadians(arc.Angle) / 2;

				var a1 = c1.Direction + pi - halfAngle;
				var a2 = c1.Direction + pi + halfAngle;

				yield return new Section(new CircularArc(c1.Point.ToVector2(), r, a1, a2, line.Start.Z, line.End.Z));
			}

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
