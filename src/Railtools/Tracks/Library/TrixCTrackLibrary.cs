using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railtools.Tracks.Library
{
	public class TrixCTrackLibrary : TrackLibrary
	{

		public TrixCTrackLibrary()
		{
			AddStraight(236.1);
			AddStraight(229.3);
			AddStraight(188.3);
			AddStraight(171.1);
			AddStraight(94.2);
			AddStraight(77.5);
			AddStraight(70.8);
			AddStraight(64.3);

			AddCurve(1, 30);
			AddCurve(1, 15);
			AddCurve(1, 7.5);

			AddCurve(2, 30);
			AddCurve(2, 15);
			AddCurve(2, 7.5);
			AddCurve(2, 24.3);
			AddCurve(2, 5.7);

			AddCurve(3, 30);
			AddCurve(3, 15);

			AddCurve(4, 30);
			AddCurve(5, 30);

			AddCurve(9, 12.1);
		}

		private void AddStraight(double length)
		{
			this.Add(CreateStraight(length));
		}

		private StraightType CreateStraight(double mm) => new StraightType()
		{
			Id = $"{Prefix}{Math.Round(mm):000}",
			Name = $"Straight {mm:0.0}mm",
			Length = mm,
		};

		private void AddCurve(int radius, double angle)
		{
			this.Add(CreateCurve(radius, angle));
		}

		private CurveType CreateCurve(int radius, double angle)
		{
			var mm = CTrack.GetRadius(radius);
			return new CurveType()
			{
				Id = $"{Prefix}{radius}{Math.Round(angle):00}",
				Name = $"Curve R{radius} {mm:0.0}mm {angle:0.0}\u00b0",
				Angle = angle,
				Radius = mm,
			};
		}



		protected const string Prefix = "62";

	}
}
