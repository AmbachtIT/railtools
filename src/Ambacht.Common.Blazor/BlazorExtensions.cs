using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Ambacht.Common.Blazor
{
	public static class BlazorExtensions
	{

		public static bool IsLocal(this IWebAssemblyHostEnvironment env) => env.BaseAddress.Contains("localhost");



		public static async Task<bool> IsInRole(this AuthenticationStateProvider provider, string role)
		{
			var state = await provider.GetAuthenticationStateAsync();
			if (state.User.Identity?.IsAuthenticated != true)
			{
				return false;
			}

			return state.User.IsInRole(role);
		}


	}
}
