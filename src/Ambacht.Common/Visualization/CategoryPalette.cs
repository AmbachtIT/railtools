using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Visualization
{
	public class CategoryPalette<T>(string[] colors)
	{

		private int _index = 0;
		private readonly Dictionary<T, string> _items = new();

		public string GetColor(T key)
		{
			if (!_items.TryGetValue(key, out var value))
			{
				_items.Add(key, value = colors[_index]);
				_index = (_index + 1) % colors.Length;
			}

			return value;
		}



	}
}
