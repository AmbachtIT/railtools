using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Http;
using OperationCanceledException = System.OperationCanceledException;

namespace Ambacht.Common.Diagnostics
{
	public static class ExceptionExtensions
	{

		public static bool IsCanceledException(this Exception exception)
		{
			return exception is TaskCanceledException || exception is OperationCanceledException;
		}


		public static string CreateNiceMessage(this Exception ex)
		{
			var builder = new StringBuilder();
			if (ex is HttpStatusException hse)
			{
				builder.AppendLine($"HTTP/{(int) hse.Status} {hse.Status}");
				builder.AppendLine(hse.Message);
			}
			else
			{
				builder.AppendLine($"Type        : {ex.GetType().FullName}");
				builder.AppendLine($"Message     : {ex.Message}");
				builder.AppendLine($"Stack Trace : {ex.StackTrace}");
			}


			if (ex.InnerException != null)
			{
				builder.AppendLine();
				builder.AppendLine("Inner exception:");
				builder.AppendLine();
				builder.AppendLine(ex.InnerException.CreateNiceMessage());

			}

			return builder.ToString();
		}

	}
}
