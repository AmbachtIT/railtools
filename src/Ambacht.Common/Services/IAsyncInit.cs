using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ambacht.Common.Services
{

    /// <summary>
    /// Indicates that this service needs to be initialized on application startup. The operation blocks application startup, so please don't do anything that takes too long here
    /// </summary>
    public interface IAsyncInit
    {

        Task InitAsync(CancellationToken token);

		/// <summary>
		/// Priority. Lower means earlier.
		/// </summary>
		int Priority { get; }

    }

    public static class IAsyncInitServiceProviderExtensions
    {

        public static async Task InitAsync(this IServiceProvider serviceProvider, CancellationToken token = default)
        {
	        var logger = serviceProvider.GetRequiredService<ILogger<IAsyncInit>>();
	        var all = serviceProvider.GetServices<IAsyncInit>().OrderBy(a => a.Priority).ToList();
	        foreach (var item in all)
            {
				logger.LogTrace($"Initializing item of type {item.GetType().FullName}");
                await item.InitAsync(token);
            }
        }

    }

}
