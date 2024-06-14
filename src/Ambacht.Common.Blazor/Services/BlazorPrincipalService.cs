using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace Ambacht.Common.Blazor.Services
{
	public class BlazorPrincipalService(AuthenticationStateProvider provider) : IPrincipalService
	{
		public async Task<ClaimsPrincipal> GetCurrentPrincipal()
		{
			var state = await provider.GetAuthenticationStateAsync();
			return state.User;
		}
	}
}
