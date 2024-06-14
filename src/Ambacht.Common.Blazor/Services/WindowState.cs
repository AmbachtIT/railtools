using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Blazor.Services
{
	public record class WindowState
	{
		
		/// <summary>
		/// Size of the window
		/// </summary>
		public Vector2 Size { get; set; }

	}
}
