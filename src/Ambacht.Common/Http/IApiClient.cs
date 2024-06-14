using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Serialization;
using Ambacht.Common.Services;


namespace Ambacht.Common.Http
{
    public interface IApiClient
    {
        Task<T> Send<T>(HttpMethod method, string url, object payload, CancellationToken token = default);

    }

    public static class ApiClientExtensions
    {

	    public static Task<T> Post<T>(this IApiClient client, string url, object payload, CancellationToken token = default) =>
		    client.Send<T>(HttpMethod.Post, url, payload, token);
	    public static Task<T> PostJson<T>(this IApiClient client, string url, object payload, CancellationToken token = default) =>
		    client.Send<T>(HttpMethod.Post, url, payload, token);

	    public static Task<T> Get<T>(this IApiClient client, string url, CancellationToken token = default) =>
		    client.Send<T>(HttpMethod.Get, url, null, token);
	    public static Task<T> GetJson<T>(this IApiClient client, string url, CancellationToken token = default) =>
		    client.Send<T>(HttpMethod.Get, url, null, token);


		public static FactoryBuilder<IApiClient> AddApiClientFactory(this IServiceCollection services)
        {
            services.AddFactory<IApiClient>();
            return new FactoryBuilder<IApiClient>(services);
        }

        public static FactoryBuilder<IApiClient> ConfigureApiClients(this IServiceCollection services)
        {
            return new FactoryBuilder<IApiClient>(services);
        }

        public static void AddApiClient(this FactoryBuilder<IApiClient> builder, string key)
        {
            builder.AddImplementation(key,
                sp =>
                {
                    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                    var serializer = sp.CreateFactory<string, IJsonSerializer>(key)();
                    return new ApiClient(() =>
                            httpClientFactory.CreateClient(key),
                        serializer);
                });
        }

        public static void AddApiClient(this FactoryBuilder<IApiClient> builder)
        {
            builder.AddImplementation(sp =>
                {
                    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                    var serializer = sp.CreateFactory<string, IJsonSerializer>("Default")();
                    return new ApiClient(() =>
                            httpClientFactory.CreateClient("Default"),
                        serializer);
                });
        }

    }
}
