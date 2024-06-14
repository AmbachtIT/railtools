using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railtools.Tracks.Library
{
	public class TrackLibrary
	{

		private readonly List<SegmentType> _pieces = new List<SegmentType>();

		protected void Add(SegmentType type) => this._pieces.Add(type);

	}
}
