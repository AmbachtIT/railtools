using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Railtools.Geometry
{
	public interface ITrajectory
	{

		Vector3 StartPosition();
		Vector3 EndPosition();
		float Length();

	}
}
