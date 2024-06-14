using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Reflection
{
	public class PropertyAccessor<TObject, TProperty>
	{

		public PropertyAccessor(Expression<Func<TObject, TProperty>> getItem)
		{
			_prop = Accessor.GetPropertyInfo(getItem);
		}

		private readonly PropertyInfo _prop;


		public void Set(TObject obj, TProperty value) => _prop.SetValue(obj, value);

		public TProperty Get(TObject obj) => (TProperty)_prop.GetValue(obj);

	}

	public static class PropertyAccessor<TObject>
	{
		public static PropertyAccessor<TObject, TProperty> Create<TProperty>(Expression<Func<TObject, TProperty>> getItem) => new PropertyAccessor<TObject, TProperty>(getItem);
	}
}
