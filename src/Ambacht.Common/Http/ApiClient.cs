using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Reflection.Metadata;
using Ambacht.Common.Http;
using Ambacht.Common.Serialization;


namespace Ambacht.Common.Http
{
    public class ApiClient : IApiClient
    {

        public ApiClient(Func<HttpClient> httpClientFactory, IJsonSerializer serializer)
        {
            _httpClientFactory = httpClientFactory;
            _serializer = serializer;
        }

        private readonly Func<HttpClient> _httpClientFactory;
        private readonly IJsonSerializer _serializer;

        public async Task<TReponse> Send<TReponse>(HttpMethod method, string url, object payload, CancellationToken token = default)
        {
	        using var client = _httpClientFactory();
	        var request = new HttpRequestMessage(method, url);

	        if (payload != null)
	        {
		        request.Content = CreateJsonContent(payload);
	        }

	        var response = await client.SendAsync(request, token);
	        await ValidateResponse(response);
	        return await CreateResponse<TReponse>(response);
		}

        private async Task<T> CreateResponse<T>(HttpResponseMessage response)
        {
            return await ReadJsonContent<T>(response.Content);
        }

        private async Task ValidateResponse(HttpResponseMessage response)
        {
	        if (response.IsSuccessStatusCode)
	        {
				return;
	        }
	        var message = await response.Content.ReadAsStringAsync();
	        throw new HttpStatusException(response.StatusCode, message);
        }

		private HttpContent CreateJsonContent(object obj)
        {
            var json = _serializer.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
        }

        private async Task<T> ReadJsonContent<T>(HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return _serializer.DeserializeObject<T>(json);
        }


   
    }
}
