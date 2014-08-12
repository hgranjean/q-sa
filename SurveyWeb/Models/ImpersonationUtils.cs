using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace SurveyWeb.Models
{
    internal static class ImpersonationUtils
    {
        public static bool IsImpersonating(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return false;
            }

            return principal.HasClaim("UserImpersonation", "true");
        }

        public static String GetOriginalUsername(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return String.Empty;
            }

            if (!principal.IsImpersonating())
            {
                return String.Empty;
            }

            var originalUsernameClaim = principal.Claims.SingleOrDefault(c => c.Type == "OriginalUsername");

            if (originalUsernameClaim == null)
            {
                return String.Empty;
            }

            return originalUsernameClaim.Value;
        }
    }
}