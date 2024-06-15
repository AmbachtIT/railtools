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

		[XmlAttribute("widthBMP")]
		public int BitmapWidth { get; set; }
		[XmlAttribute("heightBMP")]
		public int BitmapHeight { get; set; }

		[XmlAttribute("scaleX")]
		public float ScaleX { get; set; }

		[XmlAttribute("scaleY")]
		public float ScaleY { get; set; }

		[XmlAttribute("width")]
		public float Width { get; set; }

		[XmlAttribute("height")]
		public float Height { get; set; }



		[XmlArray("parts")]
		public List<TrainPlayerPart> Parts { get; set; } = new ();

		[XmlArray("connections")]
		public List<TrainPlayerConnection> Connections { get; set; } = new ();

		[XmlArray("endpoints")]
		public List<TrainPlayerEndpoint> Endpoints { get; set; } = new ();



	}
}
