using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Railtools.Interop.TrainPlayer
{

	[XmlType("layout")]
	public class TrainPlayerLayout
	{
		[XmlArray("parts")]
		public List<TrainPlayerPart> Parts { get; set; } = new ();

		[XmlArray("connections")]
		public List<TrainPlayerConnection> Connections { get; set; } = new ();

		[XmlArray("endpoints")]
		public List<TrainPlayerEndpoint> Endpoints { get; set; } = new ();



	}
}
