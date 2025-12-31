using System;
using System.Security.Claims;

namespace ClosingTime.Extensions;

public static class ClaimsExtensions
{
    public static string GetAvatarUrl(this ClaimsPrincipal user)
    {
        var hash = user.FindFirstValue("AvatarHash");
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return $"https://cdn.discordapp.com/avatars/{id}/{hash}.png";
    }

    public static string Name(this ClaimsPrincipal user)
    {
        return user.FindFirstValue("DisplayName") ?? "Unknown";
    }
}
