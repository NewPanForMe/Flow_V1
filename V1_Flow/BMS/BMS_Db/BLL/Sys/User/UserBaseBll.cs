using BMS_Db.EfContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ys.Base.Tools.xTool;
using Ys.Tools.Extra;
using Ys.Tools.Interface;
using Ys.Tools.Response;
namespace BMS_Db.BLL.Sys.User;
using BMS_Models.DbModels;
using Consul;
using System.Data;

/// <summary>
/// User基础操作类-增删改查
/// </summary>
public class UserBaseBll : IBll
{
    private readonly BmsV1DbContext _dbContext;
    private readonly ILogger<UserBaseBll> _logger;
    public UserBaseBll(BmsV1DbContext dbContext, ILogger<UserBaseBll> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// 新增用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public void Add(User user)
    {
        user.LoginPasswordSalt = Guid.NewGuid().ToString();
        user.LoginPassword = Md5Tools.MD5_32(user.LoginPassword + user.LoginPasswordSalt);
        user.JwtVersion = 1;
        user.Code = Guid.NewGuid().ToString();
        user.IsLock = false;
        user.ErrorCount = 0;
        user.ErrorCancelTime = DateTime.Now;
        _dbContext.User.Add(user);
        var roleList = user.RoleList();
        if (roleList.Count > 0)
        {
            roleList.ForEach(x =>
            {
                var userRole = new UserRole()
                {
                    Code = Guid.NewGuid().ToString(),
                    RoleCode = x,
                    RoleName = "",
                    UserCode = user.Code
                };
                _dbContext.UserRole.Add(userRole);

            });

        }
        _logger.LogWarning("新增用户：{user}", user);
    }

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public void Edit(User user)
    {
        var userEntityByCode = _dbContext.User.FirstOrDefault(x => x.Code.Equals(user.Code));
        userEntityByCode = userEntityByCode.NotNull("未找到数据");
        user.LoginPassword = string.IsNullOrEmpty(user.LoginPassword) ? userEntityByCode.LoginPassword : Md5Tools.MD5_32(user.LoginPassword + userEntityByCode.LoginPasswordSalt);
        user.JwtVersion = userEntityByCode.JwtVersion;
        user.ErrorCount = userEntityByCode.ErrorCount;
        user.ErrorCancelTime = userEntityByCode.ErrorCancelTime;
        user.LoginPasswordSalt = userEntityByCode.LoginPasswordSalt;
        _dbContext.User.Update(user);

        //先移除所有角色，在重新添加
        var roleList = user.RoleList();
        if (roleList.Count > 0)
        {
            var userRoles = _dbContext.UserRole.Where(x => x.UserCode == user.Code).ToList();
            _dbContext.UserRole.RemoveRange(userRoles);
            roleList.ForEach(x =>
            {
                var userRole = new UserRole()
                {
                    Code = Guid.NewGuid().ToString(),
                    RoleCode = x,
                    RoleName = "",
                    UserCode = user.Code
                };
                _dbContext.UserRole.Add(userRole);
            });
        }
        _logger.LogWarning("修改用户：{user}", user);

    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public void Delete(User user)
    {
        _dbContext.User.Remove(user);
        _logger.LogWarning("删除用户：{user}", user);

    }

    /// <summary>
    /// 获取用户
    /// </summary>
    /// <returns></returns>
    public async Task<List<User>> GetUser()
    {
        var listAsync = await _dbContext.User.AsNoTracking().ToListAsync();
        return listAsync;
    }




    /// <summary>
    /// 获取模块
    /// </summary>
    /// <returns></returns>
    public User GetUserEntityByCode(string code)
    {
        code.NotNull("传入编号为空");
        var user = _dbContext.User.FirstOrDefault(x => x.Code.Equals(code));
        user = user.NotNull("当前数据不存在");
        var userRoles = _dbContext.UserRole.Where(x => x.UserCode == code).ToList();
        if (userRoles.Count > 0)
        {
            var roles = new string[userRoles.Count];
            for (int i = 0; i < userRoles.Count; i++)
            {
                roles[i] = userRoles[i].RoleCode;
            }

            user.Roles = roles;

        }
        return user;
    }


    /// <summary>
    /// 获取模块
    /// </summary>
    /// <returns></returns>
    public User GetUserEntityByLoginName(string loginName)
    {
        loginName.NotNull("登录名为空");
        var user = _dbContext.User.FirstOrDefault(x => x.LoginName.Equals(loginName) || x.Phone == loginName);
        user = user.NotNull("当前数据不存在");
        var userRoles = _dbContext.UserRole.AsNoTracking().Where(x => x.UserCode == user.Code).ToList();
        if (userRoles.Count > 0)
        {
            var roles = new string[userRoles.Count];
            for (int i = 0; i < userRoles.Count; i++)
            {
                roles[i] = userRoles[i].RoleCode;
            }

            user.Roles = roles;

        }
        return user;
    }

    /// <summary>
    /// 获取模块
    /// </summary>
    /// <returns></returns>
    public User GetUserEntityByCodeWithNoTracking(string code)
    {
        code.NotNull("传入编号为空");
        var user = _dbContext.User.AsNoTracking().FirstOrDefault(x => x.Code.Equals(code));
        user = user.NotNull("当前数据不存在");
        var userRoles = _dbContext.UserRole.AsNoTracking().Where(x => x.UserCode == code).ToList();
        if (userRoles.Count > 0)
        {
            var roles = new string[userRoles.Count];
            for (int i = 0; i < userRoles.Count; i++)
            {
                roles[i] = userRoles[i].RoleCode;
            }

            user.Roles = roles;

        }
        return user;
    }

    /// <summary>
    /// 判断手机号是否已存在
    /// </summary>
    /// <returns></returns>
    public bool GetPhoneExist(string phone)
    {
        phone.NotNull("传入手机号为空");
        var user = _dbContext.User.Any(x => x.Phone == phone);
        return user;
    }
}