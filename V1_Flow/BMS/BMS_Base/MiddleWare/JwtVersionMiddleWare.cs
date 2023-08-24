using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Ys.Tools.Models;
using Ys.Tools.MoreTool;

namespace BMS_Base.MiddleWare;

public class JwtVersionMiddleWare
{
    private readonly RequestDelegate _next;

    public JwtVersionMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var currentExpire = context.User.Claims.Count();
        var currentExpireTime = new DateTime();
        var currentRefreshExpireTime = new DateTime();
        if (currentExpire > 0)
        {
            var firstOrDefault = context.User.Claims.FirstOrDefault(x => x.Type == "exp");
            if (!string.IsNullOrEmpty(firstOrDefault?.Value))
            {
                var findLast = long.Parse(firstOrDefault?.Value ?? "0");
                currentExpireTime = DateTimeTools.GetDateTimeSeconds(findLast);
                Console.WriteLine($"过期时间：{currentExpireTime}");
            }
        }
        var refreshToken = context.Request.Headers["RefreshToken"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var resolveToken = TokenTools.ResolveToken(refreshToken.ToString());
            var refreshTokenExpire = resolveToken.FirstOrDefault(x => x.Type == "exp")?.Value;
            if (!string.IsNullOrEmpty(refreshTokenExpire))
            {
                var findLast = long.Parse(refreshTokenExpire ?? "0");
                currentRefreshExpireTime = DateTimeTools.GetDateTimeSeconds(findLast);
                Console.WriteLine($"RefreshToken过期时间：{currentRefreshExpireTime}");
            }
        }



        await _next(context);
    }


}