using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Http
{
	public class HttpStatusException(HttpStatusCode status, string message = null) : ApplicationException(CreateMessage(status, message))
	{
		private static string CreateMessage(HttpStatusCode code, string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				return $"HTTP/{code}";
			}

			return message;
		}

		public HttpStatusCode Status { get; } = status;


	}
}
