using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Reactivity;
using Microsoft.AspNetCore.Components;

namespace Ambacht.Common.Blazor.Services
{
	public record class ToastService : ReactiveObject, IToastService
	{


		public void Success(string title, string message) => Add(title, message, ToastSeverity.Success);

		public void Info(string title, string message) => Add(title, message, ToastSeverity.Info);

		public void Warning(string title, string message) => Add(title, message, ToastSeverity.Warning);

		public void Error(string title, string message) => Add(title, message, ToastSeverity.Error);

		public void Add(string title, string message, ToastSeverity severity)
		{
			Message = new ToastMessage()
			{
				Message = message,
				Title = title,
				Severity = severity
			};
		}

		/// <summary>
		/// The message that's currently showing
		/// </summary>
		public ToastMessage Message
		{
			get => _message;
			set
			{
				_message = value;
				OnPropertyChanged();
			}
		}

		private ToastMessage _message;
	}

	public enum ToastSeverity
	{
		Success,
		Info,
		Warning,
		Error
	}

	public record class ToastMessage
	{
		public string Title { get; set; }
		public string Message { get; set; }
		public ToastSeverity Severity { get; set; }
	}
}
