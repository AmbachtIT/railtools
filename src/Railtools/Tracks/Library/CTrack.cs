using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railtools.Tracks.Library
{
	public static class CTrack
	{


		public static float GetRadius(int r) => _radii[r];

		private static Dictionary<int, float> _radii = new Dictionary<int, float>()
		{
			{ 1, 360 },
			{ 2, 437.5f },
			{ 3, 515 },
			{ 4, 579.3f },
			{ 5, 643.6f },
			{ 9, 1114.6f },
		};


	}
}
