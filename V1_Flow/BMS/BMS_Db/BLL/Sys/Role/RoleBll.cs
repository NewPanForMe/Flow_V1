using BMS_Db.EfContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ys.Base.Tools.xTool;
using Ys.Tools.Extra;
using Ys.Tools.Interface;
using BMS_Models.DbModels;
using Consul;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Ys.Tools.Response;

namespace BMS_Db.BLL.Sys.Role;

public class RoleBll : IBll
{
    private readonly BmsV1DbContext _dbContext;
    private readonly ILogger<RoleBll> _logger;
    public RoleBll(BmsV1DbContext dbContext, ILogger<RoleBll> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    /// <summary>
    /// 新增用户
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public void Add(BMS_Models.DbModels.Role role)
    {
        role.Code = Guid.NewGuid().ToString();
        _dbContext.Role.Add(role);
        var moduleList = role.ModuleList();
        if (moduleList.Count > 0)
        {
            moduleList.ForEach(module =>
            {
                var moduleRole = new MenuRole()
                {
                    Code = Guid.NewGuid().ToString(),
                    ModuleCode = module,
                    RoleCode = role.Code
                };
                _dbContext.MenuRole.Add(moduleRole);
            });
        }
        _logger.LogWarning("新增角色：{role}", role);
    }

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public void Edit(BMS_Models.DbModels.Role role)
    {
        _dbContext.Role.Update(role);
        var moduleList = role.ModuleList();
        var menuRoles = _dbContext.MenuRole.Where(x => x.RoleCode == role.Code).ToList();
        _dbContext.MenuRole.RemoveRange(menuRoles);
        if (moduleList.Count > 0)
        {
            moduleList.ForEach(module =>
            {
                var moduleRole = new MenuRole()
                {
                    Code = Guid.NewGuid().ToString(),
                    ModuleCode = module,
                    RoleCode = role.Code
                };
                _dbContext.MenuRole.Add(moduleRole);
            });
        }

        _logger.LogWarning("修改角色：{role}", role);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public void Delete(BMS_Models.DbModels.Role role)
    {
        _dbContext.Role.Remove(role);
        _logger.LogWarning("删除角色：{user}", role);

    }

    /// <summary>
    /// 获取用户
    /// </summary>
    /// <returns></returns>
    public async Task<List<BMS_Models.DbModels.Role>> GetRole()
    {
        var listAsync = await _dbContext.Role.OrderBy(x => x.OrderBy).AsNoTracking().ToListAsync();
        return listAsync;
    }




    /// <summary>
    /// 获取模块
    /// </summary>
    /// <returns></returns>
    public BMS_Models.DbModels.Role GetRoleEntityByCode(string code)
    {
        code.NotNull("传入编号为空");
        var role = _dbContext.Role.AsNoTracking().FirstOrDefault(x => x.Code.Equals(code));
        role = role.NotNull("当前数据不存在");
        var menuRoles = _dbContext.MenuRole.Where(x => x.RoleCode == code).ToList();
        if (menuRoles.Count > 0)
        {
            var strings = new string[menuRoles.Count];
            for (int i = 0; i < menuRoles.Count; i++)
            {
                strings[i] = menuRoles[i].ModuleCode;
            }
            role.Modules = strings;
        }
        _logger.LogWarning("获取角色{code}：{user}", code, role);
        return role;
    }

    /// <summary>
    /// 获取Role的下拉
    /// </summary>
    /// <returns></returns>
    public async Task<List<SelectOption>> GetSelectOptions()
    {
        var selectOptions = new List<SelectOption>();
        var role = await _dbContext.Role.AsNoTracking().ToListAsync();
        role.ForEach(x =>
        {
            var select = new SelectOption() { Label = x.Name, Value = x.Code };
            selectOptions.Add(select);
        });
        return selectOptions;
    }

}