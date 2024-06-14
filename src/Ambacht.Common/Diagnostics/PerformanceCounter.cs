using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Diagnostics
{
	public class PerformanceCounter
	{

		public TextWriter Out { get; set; }
		public int WriteEveryCount { get; set; } = 1000;
		private int _currentCount;

		public void Reset() => _entries.Clear();


		public void Record(string key, ref DateTime utcTime)
		{
			var duration = DateTime.UtcNow - utcTime;
			Record(key, duration);
			utcTime = DateTime.UtcNow;
		}

		public void Record(string key, TimeSpan duration)
		{
			if (!_entries.TryGetValue(key, out var result))
			{
				_entries.Add(key, result = new Entry());
			}

			result.Count++;
			result.TotalTime += duration;

			_currentCount++;
			if (Out != null && WriteEveryCount > 0 && _currentCount > WriteEveryCount)
			{
				_currentCount = 0;
				Write(Out);
			}
		}


		public IDisposable Record(string key)
		{
			var start = DateTime.UtcNow;
			return new Finally(() =>
			{
				var duration = DateTime.UtcNow - start;
				Record(key, duration);
			});
		}

		private readonly Dictionary<string, Entry> _entries = new();


		private class Entry
		{
			public int Count { get; set; }
			public TimeSpan TotalTime { get; set; }
			public TimeSpan AverageTime => Count == 0 ? TimeSpan.Zero : TotalTime / Count;
		}



		public void Write(TextWriter writer)
		{
			foreach (var entry in _entries.OrderByDescending(e => e.Value.TotalTime))
			{
				writer.WriteLine($"- {entry.Key}: Count {entry.Value.Count}. Total {entry.Value.TotalTime}. Average {entry.Value.AverageTime}");
			}
		}



	}
}
