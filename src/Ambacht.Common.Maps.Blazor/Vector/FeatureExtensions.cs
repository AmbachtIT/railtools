using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Reflection;
using NetTopologySuite.Features;

namespace Ambacht.Common.Maps.Blazor.Vector
{
	public static class FeatureExtensions
	{


		public static Accessor<string> Fill(this IFeature feature) => feature.Accessor<string>("fill");

		public static Accessor<string> Stroke(this IFeature feature) => feature.Accessor<string>("stroke");

		public static Accessor<float> StrokeWidth(this IFeature feature) => feature.Accessor<float>("stroke-width");

		public static Accessor<int> ZIndex(this IFeature feature) => feature.Accessor<int>("z-index");


		public static Accessor<T> Accessor<T>(this IFeature feature, string key)
		{
			var prefixed = $"ambacht:{key}";
			return new Accessor<T>(
				v => feature.SetValue(prefixed, v),
				() => feature.GetValue<T>(prefixed)
			);
		}


		public static void SetValue<T>(this IFeature feature, string key, T value)
		{
			if (feature.Attributes == null)
			{
				feature.Attributes = new AttributesTable();
			}
			if (!feature.Attributes.Exists(key))
			{
				feature.Attributes.Add(key, value);
			}
			else
			{
				feature.Attributes[key] = value;
			}
		}

		public static T GetValue<T>(this IFeature feature, string key)
		{
			if (!feature.Attributes.Exists(key))
			{
				return default;
			}
			return (T)feature.Attributes[key];
		}

		public static void AddRange<T>(this FeatureCollection collection, IEnumerable<T> features) where T : IFeature
		{
			foreach (var feature in features)
			{
				collection.Add(feature);
			}
		}

	}
}
