using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Reflection
{
	public class GuidCleaner
	{

		private readonly Dictionary<string, string> _ids = new Dictionary<string, string>();


		public void Clean(IEnumerable<Accessor<string>> accessors, string prefix)
		{
			foreach (var accessor in accessors)
			{
				var value = accessor.Get()?.ToLower();
				if (value == null)
				{
					continue;
				}

				if (!_ids.TryGetValue(value, out var replacement))
				{
					_ids.Add(value, replacement = $"{prefix}-{_ids.Count + 1}");
				}

				accessor.Set(replacement);
			}
		}

	}
}
