using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Railtools.Interop.TrainPlayer
{

	[XmlType("connection")]
	public class TrainPlayerConnection
	{
		[XmlAttribute("endpoint1")]
		public int Endpoint1 { get; set; }

		[XmlAttribute("endpoint2")]
		public int Endpoint2 { get; set; }

	}
}
