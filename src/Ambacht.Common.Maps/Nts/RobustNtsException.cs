using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Ambacht.Common.Maps.Nts
{
	public class RobustNtsException : Exception
	{

		public RobustNtsException(string message) : base(message)
		{
		}
		public Coordinate Coordinate { get; set; }


		public Dictionary<string, Geometry> Geometries { get; set; } = new Dictionary<string, Geometry>();
	}
}
