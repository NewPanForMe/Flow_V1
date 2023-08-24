using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BMS.Controllers.Sys
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public string Check()
        {
            return $"BmsV1Service-Ok-【{DateTime.Now:yyyy/MM/dd HH:mm:ss}】";
        }
    }
}

