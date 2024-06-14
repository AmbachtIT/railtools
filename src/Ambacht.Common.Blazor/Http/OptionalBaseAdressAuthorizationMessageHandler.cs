using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Blazor.Http;


/// <summary>
/// A <see cref="DelegatingHandler"/> that attaches access tokens to outgoing <see cref="HttpResponseMessage"/> instances.
/// Access tokens will only be added when the request URI is within the application's base URI.
/// </summary>
public class OptionalBaseAdressAuthorizationMessageHandler : OptionalAuthorizationMessageHandler
{
	/// <summary>
	/// Initializes a new instance of <see cref="BaseAddressAuthorizationMessageHandler"/>.
	/// </summary>
	/// <param name="provider">The <see cref="IAccessTokenProvider"/> to use for requesting tokens.</param>
	/// <param name="navigationManager">The <see cref="NavigationManager"/> used to compute the base address.</param>
	public OptionalBaseAdressAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager)
		: base(provider, navigationManager)
	{
		ConfigureHandler(new[] { navigationManager.BaseUri });
	}
}
