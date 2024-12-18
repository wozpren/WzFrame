using BootstrapBlazor.Components;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using WzFrame.Entity.Users;

namespace WzFrame.Shared.Extensions
{
    public static class BootstrapExtensions
    {

        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static bool Contains(this IEnumerable<string> list, IEnumerable<string> other)
        {
            return list.Intersect(other).Count() > 0;
        }

        public static List<string>? GetUserRoles(this ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                var roless = user.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(i => i.Value)
                    .ToList();
                return roless;
            }

            return null;
        }

        public static bool IncludeRole(this ClaimsPrincipal user, List<string> permission)
        {
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                var roles = user.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(i => i.Value)
                    .ToList();
                return permission.Contains(roles);
            }
            return false;
        }
    }
}
