using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlpWebApi.Controllers
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