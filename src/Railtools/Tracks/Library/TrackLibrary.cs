using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railtools.Tracks.Library
{
	public class TrackLibrary : IEnumerable<SectionType>
	{

		private readonly List<SectionType> _pieces = new List<SectionType>();

		protected void Add(SectionType type) => this._pieces.Add(type);


		public IEnumerator<SectionType> GetEnumerator()
		{
			return _pieces.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
