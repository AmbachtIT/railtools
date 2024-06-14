using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Blazor.Utils
{
	public record class DirtyTracker(string Name = null)
	{

		private bool _forceRerender = true;
		private object _last = null;


		public void ForceRender()
		{
			_forceRerender = true;
		}

		public bool ShouldRender(object state)
		{
			if (!_forceRerender && object.Equals(_last, state))
			{
				return false;
			}

			_forceRerender = false;
			_last = state;

			return true;
		}

	}
}
