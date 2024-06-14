using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Http
{
	[AttributeUsage(AttributeTargets.Interface)]
	public class GenerateApiClientAttribute : Attribute
	{
	}
}
