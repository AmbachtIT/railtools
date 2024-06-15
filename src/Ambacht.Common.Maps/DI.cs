using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Maps.Nts;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;

namespace Ambacht.Common.Maps
{
	public static class DI
	{

		public static IServiceCollection AddAmbachtCommonMaps(this IServiceCollection services)
		{
			NetTopologySuite.NtsGeometryServices.Instance = NTSExtensions.GetNgGeometryServices();
			return services;
		}

	}
}
