using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Reactivity
{
	public class SimpleEvent : IObservable<Unit>
	{

		public void Trigger() => _subject.OnNext(Unit.Default);

		private readonly Subject<Unit> _subject = new Subject<Unit>();

		public IDisposable Subscribe(IObserver<Unit> observer) => _subject.Subscribe(observer);
	}
}
