using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common
{
	public record struct CodeReference
	{
		public CodeReference(string system, string code)
		{
			this.System = system;
			this.Code = code;
		}

		public CodeReference()
		{
		}


		/// <summary>
		/// Identifies the system this code is a part of.
		/// </summary>
		public string System { get; set; }

		/// <summary>
		/// Uniquely identifies the object within this system.
		/// </summary>
		public string Code { get; set; }

		public bool IsEmpty() => System == null;

		public override string ToString() => $"{System}:{Code}";
	}
}
