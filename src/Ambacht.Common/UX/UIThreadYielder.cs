using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.UX
{
	public class UIThreadYielder
	{

		public TimeSpan MaxCpuTime { get; set; } = TimeSpan.FromMilliseconds(20);

		public TimeSpan UITime { get; set; } = TimeSpan.FromMilliseconds(10);

		public async Task AllowYield(CancellationToken token)
		{
			if (_lastTime != null && DateTime.UtcNow.Subtract(_lastTime.Value) > MaxCpuTime)
			{
				await Task.Delay(UITime, token);
			}
			_lastTime = DateTime.UtcNow;
			token.ThrowIfCancellationRequested();
		}

		private DateTime? _lastTime = null;



	}
}
