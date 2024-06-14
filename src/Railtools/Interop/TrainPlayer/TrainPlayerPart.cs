using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Railtools.Interop.TrainPlayer
{
	[XmlType("part")]
	public class TrainPlayerPart
	{

		[XmlAttribute("type")]
		public string Type { get; set; }

		[XmlAttribute("id")]
		public int Id { get; set; }


		[XmlArray("endpointNrs")]
		[XmlArrayItem(typeof(int), ElementName = "endpointNr")]
		public List<int> EndpointNrs { get; set; } = new List<int>();


		[XmlArray("drawing")]
		[XmlArrayItem(typeof(TrainPlayerLine), ElementName = "line")]
		[XmlArrayItem(typeof(TrainPlayerArc), ElementName = "arc")]
		public List<TrainPlayerDrawing> Drawings { get; set; } = new List<TrainPlayerDrawing>();

	}
}
