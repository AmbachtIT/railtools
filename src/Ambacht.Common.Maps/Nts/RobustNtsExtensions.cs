using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Services;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Prepared;
using NetTopologySuite.Operation.Union;

namespace Ambacht.Common.Maps.Nts
{
	public static class RobustNtsExtensions
	{


		public static Geometry RobustIntersection(this Geometry geom, Geometry other)
		{
			try
			{
				if (other == null || other.Area < 0.001)
				{
					return NetTopologySuite.Geometries.Polygon.Empty;
				}
				if (geom is GeometryCollection collection)
				{
					if (other is GeometryCollection collection2)
					{
						throw new NotImplementedException();
					}
					return RobustIntersectionCollection(collection, other);
				}
				if (other is GeometryCollection collection3)
				{
					return RobustIntersectionCollection(collection3, geom);
				}

				return geom.Intersection(other);
			}
			catch (TopologyException ex)
			{

				throw new RobustNtsException(ex.Message)
				{
					Geometries = new Dictionary<string, Geometry>()
					{
						{"geom", geom},
						{"other", other}
					},
					Coordinate = ex.Coordinate
				};
			}
		}

		private static Geometry RobustIntersectionCollection(this GeometryCollection collection, Geometry other)
		{
			var result = collection.Geometries.Select(g => g.RobustIntersection(other)).ToArray();
			return new GeometryCollection(result);
		}

		public static Geometry RobustDifference(this Geometry geom, Geometry other, double epsilon)
		{
			try
			{
				if (other == null || other.Area < epsilon)
				{
					return geom;
				}
				if (geom is GeometryCollection collection)
				{
					return RobustDifferenceCollection(collection.Geometries, other, epsilon);
				}

				if (other is GeometryCollection collection2)
				{
					var result = geom;
					foreach (var child in collection2.Geometries)
					{
						result = result.RobustDifference(child, epsilon);
					}

					return result;
				}

				return geom.Difference(other);
			}
			catch (TopologyException ex)
			{
				
				throw new RobustNtsException(ex.Message)
				{
					Geometries = new Dictionary<string, Geometry>()
					{
						{"geom", geom},
						{"other", other}
					},
					Coordinate = ex.Coordinate
				};
			}
		}

		private static Geometry RobustDifferenceCollection(this Geometry[] geometries, Geometry other, double epsilon)
		{
			var intersected = geometries.Select(g => g.RobustDifference(other, epsilon)).ToList();
			var flattened = intersected.SelectMany(g => g.Flatten()).ToArray();
			if (flattened.Length == 0)
			{
				return NetTopologySuite.Geometries.Polygon.Empty;
			}

			if (flattened.Length == 1)
			{
				return flattened[0];
			}

			return new GeometryCollection(flattened);
		}

		private static IEnumerable<NetTopologySuite.Geometries.Polygon> Flatten(this Geometry geom)
		{
			if (geom == null || geom.IsEmpty)
			{
				yield break;
			}

			if (geom is GeometryCollection collection)
			{
				foreach (var child in collection.Geometries)
				{
					foreach (var flattenedChild in child.Flatten())
					{
						yield return flattenedChild;
					}
				}
			}
			else if(geom is NetTopologySuite.Geometries.Polygon poly)
			{
				yield return poly;
			}
		}



		public static Geometry RobustUnion(this IEnumerable<Geometry> geoms)
		{
			var all = geoms.Where(g => g != null && !g.IsEmpty).ToList();
			if (all.Count == 0)
			{
				return NetTopologySuite.Geometries.Polygon.Empty;
			}

			if (all.Count == 1)
			{
				return all.Single();
			}

			return new CascadedPolygonUnion(all.Select(g => g.Copy().Buffer(0.01f)).ToArray()).Union();
		}

		public static Geometry RobustIntersection(this IPreparedGeometry geometry, Geometry other)
		{
			return geometry.Geometry.Intersection(other);
		}



		public static bool? RobustContains(this IPreparedGeometry geometry, Geometry other)
		{
			if (geometry.Geometry is GeometryCollection collection)
			{
				if (other is Point point)
				{
					return collection.Geometries.Any(g => g.Contains(point));
				}

				return null;
			}

			return geometry.Contains(other);
		}

	}
}
