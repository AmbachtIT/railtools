using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Http
{
	public class AddHeaderMessageHandler : DelegatingHandler
	{
		public AddHeaderMessageHandler(Func<HttpRequestMessage, KeyValuePair<string, string>> getHeaderFunc)
		{
			this._getHeaderFunc = getHeaderFunc;
		}
		public AddHeaderMessageHandler(Func<HttpRequestMessage, KeyValuePair<string, string>> getHeaderFunc, DelegatingHandler innerHandler) : base(innerHandler)
		{
			this._getHeaderFunc = getHeaderFunc;
		}

		private readonly Func<HttpRequestMessage, KeyValuePair<string, string>> _getHeaderFunc;

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var header = _getHeaderFunc(request);
			request.Headers.Add(header.Key, header.Value);
			return base.SendAsync(request, cancellationToken);
		}
	}
}
