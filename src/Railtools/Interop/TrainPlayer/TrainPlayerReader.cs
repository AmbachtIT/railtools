using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Railtools.Interop.TrainPlayer
{
    public class TrainPlayerReader
    {

		public TrainPlayerLayout Read(Stream stream) 
		{
			var serializer = new XmlSerializer(typeof(TrainPlayerLayout));
			return (TrainPlayerLayout) serializer.Deserialize(stream)!;
		}

    }
}
