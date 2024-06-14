using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Blazor.Services
{
	public interface ITextMeasurementService
	{

		Vector2 Measure(string @class, string text);

	}

	public class TextMeasurementService : ITextMeasurementService
	{
		public Vector2 Measure(string @class, string text)
		{
			if (text == null)
			{
				text = string.Empty;
			}
			return new Vector2(text.Length * fontSize * 0.4f, fontSize);
		}

		private float fontSize = 20;

	}
}
