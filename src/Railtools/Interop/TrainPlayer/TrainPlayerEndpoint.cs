using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Railtools.Interop.TrainPlayer
{

	[XmlType("endpoint")]
	public class TrainPlayerEndpoint
	{

		[XmlAttribute("nr")]
		public int Nr { get; set; }

		[XmlAttribute("coord")]
		public string Coord { get; set; }

		[XmlAttribute("direction")]
		public double Direction { get; set; }

	}
}
