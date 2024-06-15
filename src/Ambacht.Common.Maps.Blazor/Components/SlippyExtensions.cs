using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Blazor.Components;
using Ambacht.Common.Maps.Nts;
using Ambacht.Common.Maps.Projections;
using Ambacht.Common.Maps.Tiles;
using Ambacht.Common.Mathmatics;

namespace Ambacht.Common.Maps.Blazor.Components
{
	public static class SlippyExtensions
	{

		public static Vector2 PixelPositionToWorld(this DragArgs args, SlippyMapView view, Projection projection)
		{
			return 
				view
					.ViewToLatLng(args.PixelPosition.ToVector2D())
					.ToCoordinate(projection).ToVector2().Cast<float>()
					.ToVector2();
		}


	}
}
