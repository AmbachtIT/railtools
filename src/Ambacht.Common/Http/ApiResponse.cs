using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Http
{
	public record class ApiResponse
	{
		public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
		public string Message { get; set; }
		public bool Success { get; set; } = true;

		public static ApiResponse Ok(string message = null) => new ApiResponse()
		{
			Message = message,
			Success = true
		};

		public static ApiResponse Error(string message, HttpStatusCode code = HttpStatusCode.InternalServerError) => new ApiResponse()
		{
			Code = code,
			Message = message,
			Success = false
		};


		public static ApiResponse FromException(Exception ex)
		{
			return ex switch
			{
				HttpRequestException rex => Error(rex.Message, rex.StatusCode ?? HttpStatusCode.NotImplemented),
				_ => Error(ex.Message)
			};
		}

	}
}
