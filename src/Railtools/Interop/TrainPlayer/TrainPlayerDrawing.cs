using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Railtools.Interop.TrainPlayer
{
	public class TrainPlayerDrawing
	{

		[XmlAttribute("width")]
		public string Width { get; set; }


		[XmlAttribute("pt1")]
		public string Point1 { get; set; }

		[XmlAttribute("pt2")]
		public string Point2 { get; set; }

	}

	public class TrainPlayerLine : TrainPlayerDrawing
	{

	}

	public class TrainPlayerArc : TrainPlayerDrawing
	{
		[XmlAttribute("direction")]
		public float Direction { get; set; }

		[XmlAttribute("angle")]
		public float Angle { get; set; }

		[XmlAttribute("radius")]
		public float Radius { get; set; }
	}
}
