using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;

namespace Ambacht.Common.Blazor
{
	/// <summary>
	/// Extension methods to facilitate using common reactive patterns
	/// </summary>
	/// <remarks>
	/// Source: Somewhere on stackoverflow.
	/// </remarks>
	public static class ReactiveExtensions
	{

		/// <summary>
		/// Returns an observable sequence of the value of a property when <paramref name="source"/> raises <seealso cref="INotifyPropertyChanged.PropertyChanged"/> for the given property.
		/// </summary>
		/// <typeparam name="T">The type of the source object. Type must implement <seealso cref="INotifyPropertyChanged"/>.</typeparam>
		/// <param name="source">The object to observe property changes on.</param>
		/// <returns>Returns an observable sequence of the property values as they change.</returns>
		public static IObservable<string> OnAnyPropertyChanges<T>(this T source)
			where T : INotifyPropertyChanged
		{
			return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
					handler => handler.Invoke,
					h => source.PropertyChanged += h,
					h => source.PropertyChanged -= h)
				.Select(t => t.EventArgs.PropertyName);
		}



		/// <summary>
		/// Returns an observable sequence of the value of a property when <paramref name="source"/> raises <seealso cref="INotifyPropertyChanged.PropertyChanged"/> for the given property.
		/// </summary>
		/// <typeparam name="T">The type of the source object. Type must implement <seealso cref="INotifyPropertyChanged"/>.</typeparam>
		/// <typeparam name="TProperty">The type of the property that is being observed.</typeparam>
		/// <param name="source">The object to observe property changes on.</param>
		/// <param name="property">An expression that describes which property to observe.</param>
		/// <returns>Returns an observable sequence of the property values as they change.</returns>
		public static IObservable<TProperty> OnPropertyChanges<T, TProperty>(this T source, Expression<Func<T, TProperty>> property)
			where T : INotifyPropertyChanged
		{
			return Observable.Create<TProperty>(o =>
			{
				var propertyName = property.GetPropertyInfo().Name;
				var propertySelector = property.Compile();
				return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
						handler => handler.Invoke,
						h => source.PropertyChanged += h,
						h => source.PropertyChanged -= h)
					.Where(e => e.EventArgs.PropertyName == propertyName)
					.Select(e => propertySelector(source))
					.Subscribe(o);
			});
		}

		public static void OnChangeUntilAsync<TProperty>(this IObservable<TProperty> observable,
			Func<CancellationToken, Task> task, IObservable<Unit> isDisposed) =>
			observable.OnChangeUntilAsync((_, token) => task(token), isDisposed);

		/// <summary>
		/// Performs an async task time the observable value changes
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnChangeUntilAsync<TProperty>(this IObservable<TProperty> observable, Func<TProperty, CancellationToken, Task> task, IObservable<Unit> isDisposed)
		{
			observable
				.DistinctUntilChanged()
				.Select(items => Observable.FromAsync(token => task(items, token)))
				.Switch()
				.TakeUntil(isDisposed)
				.Subscribe();
		}

		/// <summary>
		/// Performs an action any time the observable yields a new value, even if it is the same as the previous one
		/// </summary>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnNextUntil<T>(this IObservable<T> observable, Action<T> task, IObservable<Unit> isDisposed)
		{
			observable
				.Do(task)
				.TakeUntil(isDisposed)
				.Subscribe();
		}

		/// <summary>
		/// Performs an action any time the observable yields a new value, even if it is the same as the previous one
		/// </summary>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnNextUntil<T>(this IObservable<T> observable, Action task, IObservable<Unit> isDisposed) => observable.OnNextUntil(t => task(), isDisposed);

		/// <summary>
		/// Performs an async action any time the observable yields a new value, even if it is the same as the previous one
		/// </summary>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnNextUntilAsync<T>(this IObservable<T> observable, Func<T, CancellationToken, Task> task, IObservable<Unit> isDisposed)
		{
			observable
				.Select(t => Observable.FromAsync(token => task(t, token)))
				.Switch()
				.TakeUntil(isDisposed)
				.Subscribe();
		}


		/// <summary>
		/// Performs an action any time the observable yields a new value, even if it is the same as the previous one
		/// </summary>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnNextUntilAsync<T>(this IObservable<T> observable, Func<T, Task> task, IObservable<Unit> isDisposed) => observable.OnNextUntilAsync((t, _) => task(t), isDisposed);

		/// <summary>
		/// Performs an action any time the observable yields a new value, even if it is the same as the previous one
		/// </summary>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnNextUntilAsync<T>(this IObservable<T> observable, Func<Task> task, IObservable<Unit> isDisposed) => observable.OnNextUntilAsync((_, _) => task(), isDisposed);





		/// <summary>
		/// Performs an action any time the observable yields a new value that is different from the previous one
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnChangeUntil<TProperty>(this IObservable<TProperty> observable, Action<TProperty> task, IObservable<Unit> isDisposed)
		{
			if (typeof(TProperty) == typeof(Unit))
			{
				throw new InvalidOperationException("Don't use OnChangeUtil for Unit. Use OnNextUntil instead, since Unit will never change");
			}
			observable
				.DistinctUntilChanged()
				.Do(task)
				.TakeUntil(isDisposed)
				.Subscribe();
		}


		/// <summary>
		/// Performs an async action any time the observable yields a new value that is different from the previous one
		/// </summary>
		/// <param name="observable"></param>
		/// <param name="task"></param>
		/// <param name="isDisposed"></param>
		public static void OnNextUntilAsync(this IObservable<Unit> observable, Func<CancellationToken, Task> task, IObservable<Unit> isDisposed)
		{
			observable
				.Select(_ => Observable.FromAsync(task))
				.Switch()
				.TakeUntil(isDisposed)
				.Subscribe();
		}


		/// <summary>
		/// Gets property information for the specified <paramref name="property"/> expression.
		/// </summary>
		/// <typeparam name="TSource">Type of the parameter in the <paramref name="property"/> expression.</typeparam>
		/// <typeparam name="TValue">Type of the property's value.</typeparam>
		/// <param name="property">The expression from which to retrieve the property information.</param>
		/// <returns>Property information for the specified expression.</returns>
		/// <exception cref="ArgumentException">The expression is not understood.</exception>
		public static PropertyInfo GetPropertyInfo<TSource, TValue>(this Expression<Func<TSource, TValue>> property)
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
