using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Blazor.Services;
using Ambacht.Common.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Railtools.Test
{
	public static class TestHelper
	{
		public static string GetPath(Type type, string filename)
		{
			var parts = type.Namespace.Split('.').Skip(2);
			return Path.Combine(SourceHelper.GetSourceRoot(new[]
			{
				"test",
				"Railtools.Test",
			}.Concat(parts).ToArray()), filename);
		}


		public static ServiceProvider CreateEmptyProvider()
		{
			var services = new ServiceCollection();

			services.AddTransient<IGetSizeService>(_ => new GetFixedSizeService()
			{
				GetSize = _ => new Vector2(1000, 1000)
			});

			return services.BuildServiceProvider();
		}
	}
}
