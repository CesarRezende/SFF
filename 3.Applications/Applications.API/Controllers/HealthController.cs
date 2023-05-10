using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace SFF.Applications.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("version")]
        public IActionResult Version()
        {
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            return Ok(version);
        }
    }
}
