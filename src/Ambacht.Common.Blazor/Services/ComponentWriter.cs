using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlazorTemplater;
using Microsoft.AspNetCore.Components;

namespace Ambacht.Common.Blazor.Services
{
	public class ComponentWriter
	{

		public ComponentWriter(IServiceProvider provider)
		{
			_provider = provider;
		}

		private readonly IServiceProvider _provider;


		public async Task Export<TComponent>(string outputPath,
			Action<ComponentRenderer<TComponent>> configureRenderer = null) where TComponent : ComponentBase
		{
			if (!outputPath.EndsWith(".html"))
			{
				throw new ArgumentException();
			}

			var result = await ExportFragment(configureRenderer);
			outputPath = outputPath.Replace(".html", result.Extension);
			await result.WriteAsync(outputPath);
		}



		public async Task<Result> ExportFragment<TComponent>(Action<ComponentRenderer<TComponent>> configureRenderer = null) where TComponent : ComponentBase
		{
			await using var writer = new StringWriter(CultureInfo.InvariantCulture);
			var extension = await Export(writer, configureRenderer);
			return new Result()
			{
				Extension = extension,
				Content = writer.ToString()
			};
		}



		private async Task<string> Export<TComponent>(TextWriter writer, Action<ComponentRenderer<TComponent>> configureRenderer = null) where TComponent : ComponentBase
		{
			var renderer = new ComponentRenderer<TComponent>()
				.AddServiceProvider(_provider);

			configureRenderer?.Invoke(renderer);

			var culture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				var fragment = renderer.Render();
				fragment = Clean(fragment);
				await writer.WriteAsync(fragment);

				if (fragment.StartsWith("<svg"))
				{
					return ".svg";
				}
				return ".html";
			}
			finally
			{
				await writer.FlushAsync();
				Thread.CurrentThread.CurrentCulture = culture;
			}
		}


		/// <summary>
		/// Removes DOM event handlers from the HTML, since the values of these will be different
		/// each time they are generating, making change detection more difficult.
		/// </summary>
		/// <param name="fragment"></param>
		/// <returns></returns>
		private string Clean(string fragment)
		{
			fragment = fragment.Replace("__internal_preventDefault_", "");
			foreach (var regex in EventNameRegexes())
			{
				fragment = regex.Replace(fragment, "");
			}
			return fragment;
		}


		private static IEnumerable<Regex> EventNameRegexes()
		{
			foreach (var eventName in EventNames())
			{
				yield return new Regex($" {eventName}=\\\"\\d*?\\\"");
				yield return new Regex($" {eventName}");
			}
		}

		private static IEnumerable<string> EventNames()
		{
			yield return "onclick";
			yield return "onmouseup";
			yield return "onmouseleave";
			yield return "onmousemove";
			yield return "onmousedown";
			yield return "onmouseout";
			yield return "onmousewheel";
			yield return "ontouchend";
			yield return "ontouchcancel";
			yield return "ontouchmove";
		}



		public class Result
		{
			public string Extension { get; set; }
			public string Content { get; set; }


			private async Task ExportSvg(TextWriter writer)
			{
				var svg = Content.Replace("<svg ", @"<svg version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" ");
				await writer.WriteAsync(svg);
			}

			private async Task ExportHtml(TextWriter writer)
			{
				await writer.WriteLineAsync("<html>");
				await writer.WriteLineAsync("<head>");
				await writer.WriteLineAsync("</head>");
				await writer.WriteLineAsync("<body>");
				await writer.WriteLineAsync(Content);
				await writer.WriteLineAsync("</body>");
				await writer.WriteLineAsync("</html>");
			}


			public async Task WriteAsync(string outputPath, string template = null)
			{
				await using var writer = new StreamWriter(outputPath, Encoding.UTF8, new FileStreamOptions()
				{
					Access = FileAccess.Write,
					Mode = FileMode.Create,
				});
				if (template != null)
				{
					ExportTemplate(writer, template);
				}
				if (Extension.EndsWith("svg"))
				{
					await ExportSvg(writer);
				}
				else
				{
					await ExportHtml(writer);
				}
			}

			private void ExportTemplate(StreamWriter writer, string template)
			{
				var index = template.IndexOf("@body", StringComparison.InvariantCulture);
				if (index == -1)
				{
					throw new InvalidOperationException("Template should have @body");
				}

				var span = template.AsSpan();
				writer.Write(span.Slice(0, index));
				writer.Write(Content);
				writer.Write(span.Slice(index + 5));
			}
		}

	}
}
