using System;
using System.Collections.Generic;
using System.Linq;

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
            return !string.IsNullOrEmpty(path) &&
                (PublicPrefixes.Contains(path) || PublicPrefixes.Any(prefix => prefix.StartsWith(prefix , StringComparison.OrdinalIgnoreCase)));
        }
    }
}
