using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Ambacht.Common.Maps.Data
{
	public class GeometryDataLayer
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<GeometryDatum> Data { get; set; } = new List<GeometryDatum>();
	}


	public record class GeometryDatum
	{
		public string Id { get; set; }
		public Dictionary<string, object> Props { get; set; } = new Dictionary<string, object>();
		public Geometry Geometry { get; set; }


		[JsonIgnore()]
		public string Name
		{
			get => Props.TryGet(nameof(Name))?.ToString();
			set => Props[nameof(Name)] = value;
		}

		[JsonIgnore()]
		public string Description
		{
			get => Props.TryGet(nameof(Description))?.ToString();
			set => Props[nameof(Description)] = value;
		}

		[JsonIgnore()]
		public double? Value
		{
			get
			{
				var obj = Props.TryGet(nameof(Value));
				if (obj == null)
				{
					return null;
				}
				return (double) obj;
			}
			set => Props[nameof(Value)] = value;
		}


		[JsonIgnore()]
		public string Fill
		{
			get => Props.TryGet(nameof(Fill))?.ToString();
			set => Props[nameof(Fill)] = value;
		}

		[JsonIgnore()]
		public string Stroke
		{
			get => Props.TryGet(nameof(Stroke))?.ToString();
			set => Props[nameof(Stroke)] = value;
		}

		[JsonIgnore()]
		public float? StrokeWidth
		{
			get
			{
				var obj = Props.TryGet(nameof(StrokeWidth));
				if (obj == null)
				{
					return null;
				}
				return (float)obj;
			}
			set => Props[nameof(StrokeWidth)] = value;
		}
	}

}
