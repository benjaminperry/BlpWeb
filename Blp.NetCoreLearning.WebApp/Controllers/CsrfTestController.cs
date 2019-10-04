using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blp.NetCoreLearning.WebApp.Controllers
{
    [Authorize]
    public class CsrfTestController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult IndexPost()
        {
            return View("Index");
        }
    }
}