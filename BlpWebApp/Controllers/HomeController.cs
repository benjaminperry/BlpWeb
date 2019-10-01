using BlpWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BlpWebApp.Controllers
{
    public class HomeController : Controller
    {
        ILogger logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            logger.Log(LogLevel.Information, "This is a test log message from HomeController.Index.");
            
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            logger.Log(LogLevel.Information, "The /Home/Error page was requested!");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
