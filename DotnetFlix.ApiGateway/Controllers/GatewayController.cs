using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.ApiGateway.Controllers
{
    [Route("api/gateway")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        [HttpGet("running")]
        public ActionResult Startup()
        {
            return Ok("API Gateway running...");
        }
    }
}
