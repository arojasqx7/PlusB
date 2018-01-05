using System.Security.Claims;
using System.Security.Principal;

namespace UI.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetCustomerId (this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("CustomerID");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}