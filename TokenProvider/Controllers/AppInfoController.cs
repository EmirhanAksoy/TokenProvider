using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArcelikAuthProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppInfoController : ControllerBase
    {
        [HttpGet("GetVersion")]
        public IActionResult GetVersion()
        {
            return Ok(new { BackendVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString() });
        }

        [HttpGet("DoCheck")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
    }
}