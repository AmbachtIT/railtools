using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Mathmatics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ambacht.Common.Blazor.Services
{

	public interface IGetSizeService
	{
		Task<Vector2> GetSizeAsync(ElementReference element);
	}

	public class JavascriptGetSizeService : IGetSizeService
	{

		public JavascriptGetSizeService(IJSRuntime runtime)
		{
			_runtime = runtime;
		}

		private readonly IJSRuntime _runtime;


		public async Task<Vector2> GetSizeAsync(ElementReference element)
		{
			var sizes = await _runtime.InvokeAsync<double[]>("Ambacht.getSize", element);
			return new Vector2((float)sizes[0], (float)sizes[1]);
		}
	}

	public class GetFixedSizeService : IGetSizeService
	{
		public Func<ElementReference, Vector2> GetSize { get; set; } = _ => Vector2.Zero;

		public Task<Vector2> GetSizeAsync(ElementReference element)
		{
			var result = GetSize(element);
			return Task.FromResult(result);
		}
	}
}
