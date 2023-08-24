using BMS_Db.EfContext;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Ys.Tools.Extra;
using Ys.Tools.Response;
using BMS_Db.BLL.Sys.User;

namespace BMS.Controllers.Sys
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BmsV1DbContext _dbContext;
        private readonly UserBll _userBll;
        private readonly ILogger<LoginController> _logger;

        public LoginController(UserBll userBll, ILogger<LoginController> logger, BmsV1DbContext dbContext)
        {
            _userBll = userBll;
            _logger = logger;
            _dbContext = dbContext;
        }
        [HttpPost]
        public Task<ApiResult> Check(JsonElement req)
        {
            var userName = req.GetJsonString("username").HasValueNoNameOrPwd("用户名为空");
            var password = req.GetJsonString("password").HasValueNoNameOrPwd("密码为空");
            _logger.LogInformation("{userName}登录", userName);
            return _userBll.CheckAsync(userName, password);
        }
    }
}
