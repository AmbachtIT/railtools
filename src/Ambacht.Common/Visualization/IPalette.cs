using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Visualization
{
	public interface IPalette<in TKey>
	{

		string GetColor(TKey key);

	}
}
