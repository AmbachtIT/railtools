using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Visualization
{
	public static class ColorBrewer
	{

		public static readonly string[] RdYlGn9 = new[]
		{
			"#d73027","#f46d43","#fdae61","#fee08b","#ffffbf", "#d9ef8b", "#a6d96a", "#66bd63", "#1a9850"
		};

		/// <summary>
		/// Multi hue orange-red
		/// </summary>
		public static readonly string[] OrRd9 = new[]
		{
			"#fff7ec", "#fee8c8", "#fdd49e", "#fdbb84", "#fc8d59", "#ef6548", "#d7301f", "#b30000", "#7f0000"
		};

		public static readonly string[] Greens9 = new[]
		{
			"#f7fcf5", "#e5f5e0", "#c7e9c0", "#a1d99b", "#74c476", "#41ab5d", "#238b45", "#006d2c", "#00441b"		
		};
		
		public static readonly string[] Reds9 = new[]
		{
			"#fff5f0","#fee0d2","#fcbba1","#fc9272","#fb6a4a","#ef3b2c","#cb181d","#a50f15","#67000d"			
		};

		public static readonly string[] Purple9 = new[]
		{
			"#d8d0e0", "#c7b6d9", "#b69cd2", "#a582cb", "#9468c4", "#834ebd","#7234b6","#611aaf","#5000a8"
		};

		/*
		TODO: Move these to a separate class, since these are not the color brewer colors.

		public static readonly string[] Blues9 = new[]
		{
			"#F7FCFF","#E5F5FF","#C7E9FF","#A1D9FF","#74C4FF","#41ABFF","#238BFF","#006DFF" ,"#0044FF"
		};
		*/

		public static readonly string[] Blues9 = new[]
		{
			"#f7fbff","#deebf7","#c6dbef","#9ecae1","#6baed6","#4292c6","#2171b5","#08519c","#08306b"
		};


	}
}
