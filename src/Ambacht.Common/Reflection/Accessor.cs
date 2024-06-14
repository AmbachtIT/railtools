using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Reflection
{
	public class Accessor<TProperty>(Action<TProperty> setter, Func<TProperty> getter)
	{

		public void Set(TProperty value) => setter(value);

		public TProperty Get() => getter();

	}

	public static class Accessor
	{


		public static Accessor<TProperty> FromProperty<TObject, TProperty>(TObject obj,
			Expression<Func<TObject, TProperty>> getExpression)
		{
			var prop = GetPropertyInfo(getExpression);
			return new Accessor<TProperty>(
				value => prop.SetValue(obj, value),
				() => (TProperty) prop.GetValue(obj)
			);
		}


		public static Accessor<TProperty> FromDictionary<TKey, TProperty>(Dictionary<TKey, TProperty> dict, TKey key)
		{
			return new Accessor<TProperty>(
				value => dict[key] = value,
				() => (TProperty) dict.TryGet(key)
			);
		}



		/// <summary>
		/// Gets property information for the specified <paramref name="property"/> expression.
		/// </summary>
		/// <typeparam name="TSource">Type of the parameter in the <paramref name="property"/> expression.</typeparam>
		/// <typeparam name="TValue">Type of the property's value.</typeparam>
		/// <param name="property">The expression from which to retrieve the property information.</param>
		/// <returns>Property information for the specified expression.</returns>
		/// <exception cref="ArgumentException">The expression is not understood.</exception>
		public static PropertyInfo GetPropertyInfo<TSource, TValue>(Expression<Func<TSource, TValue>> property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}

			var body = property.Body as MemberExpression;
			if (body == null)
			{
				throw new ArgumentException("Expression is not a property", "property");
			}

			var propertyInfo = body.Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("Expression is not a property", "property");
			}

			return propertyInfo;
		}


	}
}
