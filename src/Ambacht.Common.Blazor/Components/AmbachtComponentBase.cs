using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ambacht.Common.Blazor.Components
{


	public class AmbachtComponentBase : ComponentBase, IDisposable
	{


		private Subject<Unit> _parametersSet = new();

		public IObservable<Unit> ParametersSet => _parametersSet.AsObservable();

		/// <summary>
		/// Turns a parameter property into IObservable using <see cref="ParametersSet"/> observable
		/// 
		/// It only emmits, when value is changed (DistinctUntilChanged)
		/// The observable completes on <see cref="Dispose"/> (TakeUntil(<see cref="Disposed")/>
		/// </summary>
		/// <param name="parameterSelector">Parameter Property to observe</param>
		/// <example>
		/// <![CDATA[
		/// this.ObserveParameter(() => Id)
		///     .Select((id, ct) => LoadAsync(id, ct)
		///     .Switch()
		///     .Subscribe()
		/// ]]>
		/// </example>
		public IObservable<T> ObserveParameter<T>(Func<T> parameterSelector)
		{
			return ParametersSet.Select(_ => parameterSelector())
				.DistinctUntilChanged()
				.TakeUntil(Disposed);
		}

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			await base.SetParametersAsync(parameters);
			_parametersSet.OnNext(Unit.Default);
		}

		private Subject<Unit> _disposed = new();

		public IObservable<Unit> Disposed => _disposed;

		protected IDisposable OnPropertyChanges<TItem, TProperty>(TItem source, Expression<Func<TItem, TProperty>> getProperty, Action<TProperty> action) where TItem: INotifyPropertyChanged
		{
			return source.OnPropertyChanges(getProperty)
				.Do(action)
				.TakeUntil(Disposed)
				.Subscribe();
		}


		protected IDisposable AfterDelayAsync(Func<CancellationToken, Task> task, TimeSpan delay)
		{
			return Observable
				.Return(Unit.Default)
				.Delay(delay, DefaultScheduler.Instance.DisableOptimizations()) // https://github.com/dotnet/reactive/issues/2061
				.Select(_ => Observable.FromAsync(task))
				.Switch()
				.TakeUntil(Disposed)
				.Subscribe();
		}

		public virtual void Dispose()
		{
			_disposed.OnNext(Unit.Default);
		}


	}

}
