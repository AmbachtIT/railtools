using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Http
{
	public class UrlBuilder
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request">The URI, without the base URI and without any query parameters</param>
		public UrlBuilder(string request)
		{
			_request = request;
		}

		private readonly string _request;
		private readonly StringBuilder _builder = new();


		public UrlBuilder AddParameter<T>(string name, T value, AddParameterBehaviour behaviour = AddParameterBehaviour.AddIfNotDefault)
		{
			if (behaviour == AddParameterBehaviour.Omit)
			{
				return this;
			}
			if (behaviour == AddParameterBehaviour.AddIfNotDefault)
			{
				if (object.Equals(value, default) || string.IsNullOrEmpty(value?.ToString()))
				{
					return this;
				}
			}

			if (_builder.Length == 0)
			{
				_builder.Append('?');
			}
			else
			{
				_builder.Append('&');
			}
			_builder.Append(name);
			_builder.Append('=');
			_builder.Append(WebUtility.UrlEncode(value?.ToString() ?? ""));
			return this;
		}

		public string Build() => $"{_request}{_builder}";


		public enum AddParameterBehaviour
		{
			Add,
			Omit,
			AddIfNotDefault
		}
	}

	

}
