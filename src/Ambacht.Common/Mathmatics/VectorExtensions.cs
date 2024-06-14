using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Mathmatics
{
	public static class VectorExtensions
	{


		public static T Length<T>(this Vector2<T> v) where T : IFloatingPoint<T>, IFloatingPointIeee754<T>
		{
			return T.Sqrt(v.X * v.X + v.Y * v.Y);
		}


		public static Vector2<double> ToVector2D(this Vector2 v) => new(v.X, v.Y);
		public static Vector2<float> ToVector2F(this Vector2 v) => new(v.X, v.Y);


		public static Vector2 ToVector2(this Vector3 v) => new(v.X, v.Y);

		public static Vector2 ToVector2(this Vector2<float> v) => new(v.X, v.Y);

		public static Vector3 ToVector3(this Vector3<float> v) => new(v.X, v.Y, v.Z);
		public static Vector3 ToVector3(this Vector2 v, float z) => new(v.X, v.Y, z);

		public static Vector2<T> ToVector2<T>(this Vector3<T> v) where T : IFloatingPoint<T> => new(v.X, v.Y);


		public static Vector3<T> ToVector3<T>(this Vector2<T> v, T z) where T : IFloatingPoint<T> => new(v.X, v.Y, z);


		public static Vector3<float> ToVector3F(this Vector3 v) => new(v.X, v.Y, v.Z);







	}
}
