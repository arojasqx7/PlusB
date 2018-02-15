using System.Security.Claims;
using System.Security.Principal;

namespace UI.Extensions
{
    public static class IdentityExtensions
    {
        //To handle Claims to get ID of client in Session. 
        public static string GetCustomerId (this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("CustomerID");
            return (claim != null) ? claim.Value : string.Empty;
        }

        //To handle Claims to get ID of Consultant in Session. 
        public static string GetConsultantId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ConsultantID");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}