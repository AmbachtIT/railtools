using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Authentication
{
	public interface IPrincipalService
	{

		Task<ClaimsPrincipal> GetCurrentPrincipal();

	}

	public class EmptyPrincipalService : IPrincipalService
	{
		public Task<ClaimsPrincipal> GetCurrentPrincipal()
		{
			return Task.FromResult(new ClaimsPrincipal());
		}
	}
}
