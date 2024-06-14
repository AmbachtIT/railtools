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
		}

		private void AddStraight(double mm)
		{
			this.Add(CreateStraight(mm));
		}

		private Straight CreateStraight(double mm) => new Straight()
		{
			Id = $"{Prefix}{Math.Round(mm):000}",
			Name = $"Straight {mm:0.0}mm",
			Length = mm,
		};

		private Curve CreateCurve(int radius, double angle)
		{
			var mm = _radii[radius];
			return new Curve()
			{
				Id = $"{Prefix}{radius}{Math.Round(angle):00}",
				Name = $"Curve R{radius} {mm:0.0}mm {angle:0.0}\u00b0",
				Angle = angle,
				Radius = mm,
			};
		}


		private static Dictionary<int, double> _radii = new Dictionary<int, double>()
		{
			{ 1, 360 },
			{ 2, 437.5 },
			{ 3, 515 },
			{ 4, 579.3 },
			{ 5, 643.6 },
			{ 9, 1114.6 },
		};

		protected const string Prefix = "62";

	}
}
