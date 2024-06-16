using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Blazor.Services;
using Ambacht.Common.Diagnostics;
using Railtools.Geometry;
using Railtools.Interop.Lasercut;
using Railtools.Test.Interop.TrainPlayer;
using Railtools.Tracks.Layout;
using Railtools.Tracks.Library;

namespace Railtools.Test.Interop.Lasercut
{
	[TestFixture()]
	public class LasercutTests
	{

		[Test()]
		public async Task RenderSample()
		{
			await using var provider = TestHelper.CreateEmptyProvider();
			var layout = TrainPlayerTests.ConvertSampleLayout();
			var writer = new ComponentWriter(provider);
			var path = TestHelper.GetPath(GetType(), "export.svg");
			await writer.Export<LasercutSvg>(path, export =>
				export.Set(c => c.Layout, layout));


		}


		[Test()]
		public async Task RenderSimple()
		{
			await using var provider = TestHelper.CreateEmptyProvider();
			var layout = new TrackLayout();
			foreach (var piece in Spiral())
			{
				layout.Sections.Add(new Section(piece));
			}

			var writer = new ComponentWriter(provider);
			var path = TestHelper.GetPath(GetType(), "export-simple.svg");
			await writer.Export<LasercutSvg>(path, export =>
				export.Set(c => c.Layout, layout));


		}

		private IEnumerable<ITrajectory> Spiral()
		{
			var thirthyDeg = MathF.PI / 6;
			var center = new Vector2(2000, 2000);
			for (var i = 1; i <= 5; i++)
			{
				for (var angle = 0f; angle <= MathF.PI / 2; angle += thirthyDeg)
				{
					yield return new CircularArc(center, CTrack.GetRadius(i), angle + i, angle + i + thirthyDeg);
				}
			}
		}


	}
}
