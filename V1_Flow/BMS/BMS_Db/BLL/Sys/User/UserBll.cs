using System.Data;
using System.Security.Claims;
using BMS_Base.Config;
using BMS_Db.EfContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Ys.Base.Tools.xTool;
using Ys.Tools.Config;
using Ys.Tools.Exception;
using Ys.Tools.Extra;
using Ys.Tools.Interface;
using Ys.Tools.MoreTool;
using Ys.Tools.Response;

namespace BMS_Db.BLL.Sys.User;


public class UserBll : IBll
{
    private readonly BmsV1DbContext _dbContext;
    private readonly UserBaseBll _userBaseBll;
    private readonly ILogger<UserBll> _logger;
    private readonly IMemoryCache _memoryCache;
    public UserBll(BmsV1DbContext dbContext, ILogger<UserBll> logger, IMemoryCache memoryCache, UserBaseBll userBaseBll)
    {
        _dbContext = dbContext;
        _logger = logger;
        _memoryCache = memoryCache;
        _userBaseBll = userBaseBll;
    }

    /// <summary>
    /// 登录校验
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    public async Task<ApiResult> CheckAsync(string userName, string password)
    {
        var user = _userBaseBll.GetUserEntityByLoginName(userName);
        user = user.NotNull($"用户{userName}不存在");
        var passSecret = Md5Tools.MD5_32(password + user.LoginPasswordSalt).ToLower();
        Console.WriteLine($"passSecret={passSecret}");
        user.IsDelete.IsBool($"账户{userName}已删除");
        user.IsLock.IsBool($"账户{userName}已锁定");
        if (!user.LoginPassword.Equals(passSecret))
        {
            user.ErrorCount += 1;
            if (user.ErrorCount == 3)
            {
                user.IsLock = true;
                user.ErrorCancelTime = DateTime.Now.AddMinutes(SystemConfig.Instance.ErrorCount);
                await _dbContext.SaveChangesAsync();
                _logger.LogWarning("{userName}账户锁定，锁定时长：{ SystemConfig.Instance.ErrorCount}分钟", userName, SystemConfig.Instance.ErrorCount);
                return ApiResult.False($"您的账户已锁定，请于{SystemConfig.Instance.ErrorCount}分钟后重试");
            }
            await _dbContext.SaveChangesAsync();
            _logger.LogWarning("{userName}账户密码不正确", userName);
            return ApiResult.False("账户密码不正确");
        }
        user.JwtVersion++;
        user.ErrorCount = 0;
        var listClaims = new List<Claim>()
        {
            new (ClaimTypes.Name,user.Name ?? user.LoginName),
            new (ClaimTypes.NameIdentifier,user.Id.ToString()),
            new (ClaimTypes.Version,user.JwtVersion.ToString()),
            new ("UserCode",user.Code),
            new (ClaimTypes.Role,string.Join(',',user.Roles)),
            new ("ExpireTime",DateTime.Now.AddMinutes(TokenConfig.Instance.AccessExpires).ToString("yyyy/MM/dd HH:mm:ss")),
        };
        var token = TokenTools.Create(listClaims);
        listClaims = new List<Claim>()
        {
            new(ClaimTypes.Name,user.Name ?? user.LoginName),
            new (ClaimTypes.NameIdentifier,user.Id.ToString()),
            new (ClaimTypes.Version,user.JwtVersion.ToString()),
            new ("UserCode",user.Code),
            new (ClaimTypes.Role,string.Join(',',user.Roles)),
            new ("ExpireTime",DateTime.Now.AddHours(TokenConfig.Instance.RefreshToken).ToString("yyyy/MM/dd HH:mm:ss")),
        };
        var refreshToken = TokenTools.CreateRefreshToken(listClaims);
        _logger.LogWarning("{userName}登录成功，生成token【{token}】,refreshToken【{refreshToken}】", userName, token, refreshToken);
        await _dbContext.SaveChangesAsync();
        return ApiResult.True(new { token, user.JwtVersion, refreshToken, user.Name, user.Code });
    }

    /// <summary>
    /// 用于重新生成token
    /// </summary>
    public async Task<ApiResult> GenerateToken(string userCode)
    {
        var user = _dbContext.User.FirstOrDefault(x => x.Code == userCode);
        user = user.NotNull($"用户{user?.LoginName}不存在");
        user.JwtVersion++;
        user.ErrorCount = 0;
        var listClaims = new List<Claim>()
        {
            new(ClaimTypes.Name,user.Name ?? user.LoginName),
            new (ClaimTypes.NameIdentifier,user.Id.ToString()),
            new (ClaimTypes.Version,user.JwtVersion.ToString()),
            new ("UserCode",user.Code),
            new ("ExpireTime",DateTime.Now.AddMinutes(TokenConfig.Instance.AccessExpires).ToString("yyyy/MM/dd HH:mm:ss")),
        };
        var token = TokenTools.Create(listClaims);
        _logger.LogWarning("{userCode}刷新token成功，生成token【{token}】", user.LoginName, token);
        await _dbContext.SaveChangesAsync();
        return ApiResult.True(new { token, user.JwtVersion });
    }

}