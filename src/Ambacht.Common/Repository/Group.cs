using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Repository
{
	public record class Group<TKey>
	{
		public TKey Key { get; set; }

		public int Count { get; set; }
	}

}
