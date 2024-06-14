using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Diagnostics
{
	public class StopwatchWriter(TextWriter output)
	{

		private DateTime previous = DateTime.UtcNow;

		public void WriteLine(string message)
		{
			var now = DateTime.UtcNow;
			var duration = now - previous;
			output.WriteLine($"{message} - {duration}");
			previous = now;
		}

	}
}
