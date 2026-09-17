using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Constants
{
    public static class PublicPaths
    {
        private static readonly HashSet<string> PublicEndpoints = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "/",
            "/api/auth/login",
            "/api/auth/refresh",
            "/api/auth/register"
        };

        private static readonly string[] PublicPrefixes = new[]
        {
            "/swagger"
        };

        public static bool IsPublicPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            if (PublicEndpoints.Contains(path)) return true;

            return PublicPrefixes.Any(prefix => path.StartsWith(prefix , StringComparison.OrdinalIgnoreCase));
        }
    }
}
