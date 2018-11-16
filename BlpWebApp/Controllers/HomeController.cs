using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlpWebApp.Controllers
{
    [Authorize]
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
    }
}
