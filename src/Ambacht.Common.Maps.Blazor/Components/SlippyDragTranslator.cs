using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Blazor.Components;
using Ambacht.Common.Maps.Projections;
using Ambacht.Common.Maps.Tiles;

namespace Ambacht.Common.Maps.Blazor.Components
{
	public class SlippyDragTranslator
	{
		private Vector2 _previous;

		public DragArgs HandleDragStarted(DragArgs args, SlippyMapView view, Projection projection)
		{
			return args with
			{
				LocalPosition = _previous = args.PixelPositionToWorld(view, projection)
			};
		}

		public DragArgs HandleDragged(DragArgs args, SlippyMapView view, Projection projection)
		{
			var current = args.PixelPositionToWorld(view, projection);
			var result = args with
			{
				LocalPosition = current,
				LocalDelta = current - _previous
			};
			_previous = current;
			return result;
		}

	}
}
