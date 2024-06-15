using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;

namespace Railtools.Tracks.Layout
{
	public abstract record class Section
	{
		public abstract Rectangle<float> Bounds();
	}
}
