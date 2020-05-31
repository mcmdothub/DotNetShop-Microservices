using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFlix.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult CustomErrorCode(int statusCode)
        {
            // Interface IStatusCodeReExecuteFeature gives path and querystring the user tried to access
            // var errorInfo = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            // Handle error codes
            if (statusCode == 404)
            {
                ViewBag.ErrorMessage = @$"The page you are looking for doesn't exist.";
            }
            else
            {
                ViewBag.ErrorMessage = "Something went wrong. Please contact support if the problem persist.";
            }

            ViewBag.StatusCode = statusCode;
            return View("NotFound");
        }

        [Route("Error")]
        public IActionResult UnhandledExceptionHandler()
        {
            // Interface IExceptionHandlerPathFeature gives information about path and exception errors 
            // var errorPath = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.StatusCode = 404;
            ViewBag.ErrorMessage = "The page you are looking for doesn't exist.";
            return View("NotFound");
        }
    }
}