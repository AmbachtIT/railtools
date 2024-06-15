using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Blazor.Services;
using Ambacht.Common.Diagnostics;
using Railtools.Interop.Lasercut;
using Railtools.Test.Interop.TrainPlayer;

namespace Railtools.Test.Interop.Lasercut
{
	[TestFixture()]
	public class LasercutTests
	{

		[Test()]
		public async Task Render()
		{
			await using var provider = TestHelper.CreateEmptyProvider();
			var layout = TrainPlayerTests.ConvertSampleLayout();
			var writer = new ComponentWriter(provider);
			var path = TestHelper.GetPath(GetType(), "export.svg");
			await writer.Export<LasercutSvg>(path, export =>
				export.Set(c => c.Layout, layout));


		}

	}
}
