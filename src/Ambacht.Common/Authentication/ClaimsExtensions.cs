using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Authentication
{
    public static class ClaimsExtensions
    {

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            var result = principal.GetClaim(ClaimTypes.Email) ?? principal.GetClaim("emailaddress");
            if (result == null)
            {
	            var upn = principal.GetClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn");
	            if (!string.IsNullOrEmpty(upn) && upn.LastIndexOf('.') > upn.LastIndexOf('@'))
	            {
		            if (upn.IndexOf('@') != -1)
		            {
			            result = upn; // This is an e-mail address.
		            }
	            }
	            // 
            }
			return result;
        }

        public static string GetName(this ClaimsPrincipal principal)
        {
            return principal.GetClaim(ClaimTypes.Name) ?? principal.GetClaim("name");
        }

        public static string GetPictureUrl(this ClaimsPrincipal principal)
        {
	        return principal.GetClaim("picture");
        }

		public static string GetClaim(this ClaimsPrincipal principal, string type)
        {
            return principal.Claims.Where(c => c.Type == type).Select(c => c.Value).FirstOrDefault();
        }


        public static string ToDebugString(this ClaimsPrincipal principal)
        {
	        var writer = new StringWriter();
	        writer.WriteClaimsPrincipal(principal);
			return writer.ToString();
        }

		public static void WriteClaimsPrincipal(this TextWriter writer, ClaimsPrincipal principal)
        {
	        writer.WriteLine("".PadRight(40, '-'));
	        writer.WriteLine("IDENTITIES:");
	        foreach (var identity in principal.Identities)
	        {
		        writer.WriteLine($"Name: {identity.Name}");
		        writer.WriteLine($"- AuthenticationType: {identity.AuthenticationType}");
		        writer.WriteLine($"- IsAuthenticated: {identity.IsAuthenticated}");
		        writer.WriteLine($"- Claims:");
		        foreach (var claim in identity.Claims)
		        {
			        writer.WriteLine($"  - {claim.Type}: {claim.Value}");
		        }
		        writer.WriteLine();
	        }
	        writer.WriteLine("".PadRight(40, '-'));
		}


	}

	public class ClaimsPrincipalBuilder
	{
		public ClaimsPrincipalBuilder(ClaimsPrincipal original)
		{
			_originalClaims = original.Claims.ToList();
			_authenticationType = original.Identity?.AuthenticationType;
		}

		public ClaimsPrincipalBuilder(string authenticationType)
		{
			_authenticationType = authenticationType;
		}

		private readonly List<Claim> _originalClaims = new List<Claim>();
		private readonly List<Claim> _addedClaims = new List<Claim>();
		private string _authenticationType;

		public ClaimsPrincipalBuilder AddClaim(string type, string value, bool allowMultipleValues = false)
		{
			if (!allowMultipleValues)
			{
				if (_originalClaims.Any(c => c.Type == type))
				{
					throw new InvalidOperationException("Original principal already has claim: " + type);
				}
				if (_addedClaims.Any(c => c.Type == type))
				{
					throw new InvalidOperationException("Claim already added: " + type);
				}
			}
			_addedClaims.Add(new Claim(type, value));
			return this;
		}

		public ClaimsPrincipal Build() => new ClaimsPrincipal(
			new ClaimsIdentity(_originalClaims.Concat(_addedClaims), 
			_authenticationType));

	}

}
