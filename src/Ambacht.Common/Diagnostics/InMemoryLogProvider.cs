using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Ambacht.Common.Diagnostics.InMemoryLogger;
using System.Xml.Linq;

namespace Ambacht.Common.Diagnostics
{

	/// <summary>
	/// Stores _ALL_ log messages into memory. Can be useful for specific debug scenario's
	/// </summary>
	public class InMemoryLogger(string name, InMemoryLoggerProvider provider) : ILogger
	{

		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
			Func<TState, Exception, string> formatter) =>
			provider.Log(name, logLevel, eventId, state, exception, formatter);


		public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;

		public class Entry
		{
			public string Name { get; set; }
			public LogLevel Level { get; set; }
			public EventId EventId { get; set; }
			public Exception Exception { get; set; }
			public string Message { get; set; }
		}

	}


	[ProviderAlias("InMemory")]
	public sealed class InMemoryLoggerProvider : ILoggerProvider, IObservable<Entry>, IEnumerable<Entry>
	{
		private readonly ConcurrentDictionary<string, InMemoryLogger> _loggers =
			new(StringComparer.OrdinalIgnoreCase);

		public InMemoryLoggerProvider()
		{
		}

		public void Log<TState>(string name, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var message = formatter(state, exception);
			var entry = new Entry()
			{
				Name = name,
				Level = logLevel,
				EventId = eventId,
				Message = message,
				Exception = exception
			};
			_entries.Add(entry);
			_current.OnNext(entry);
		}


		private List<Entry> _entries = new();
		private readonly Subject<Entry> _current = new Subject<Entry>();


		public ILogger CreateLogger(string categoryName) =>
			_loggers.GetOrAdd(categoryName, name => new (categoryName, this));

		public void Dispose()
		{
			_loggers.Clear();
		}


		public IDisposable Subscribe(IObserver<Entry> observer) => _current.Subscribe(observer);
		public IEnumerator<Entry> GetEnumerator() => _entries.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public static class InMemoryLoggerExtensions
	{
		public static ILoggingBuilder AddInMemoryLogger(
			this ILoggingBuilder builder)
		{
			var provider = new InMemoryLoggerProvider();
			builder.Services.AddSingleton<ILoggerProvider>(provider);
			builder.Services.AddSingleton<InMemoryLoggerProvider>(provider);


			return builder;
		}

	}

}
